# Test
Тестовое задание на позицию разработчика в True Code

# Сервис получения курсов валют

Микросервисы на .NET 8 с автоматическим обновлением курсов валют раз в 60 минут с авторизацией через JWT.

## Архитектура

| Проект | Назначение | Порт |
|--------|-----------|------|
| `ApiGateway` | ApiGateway | 5000 |
| `UserService` | Регистрация / логин / выход | 5001 |
| `FinanceService` | Получение курса для пользователя, добавление курса в избранное, удаление курса | 5002 |
| `CurrencyWorker` | Фоновый сервис обновления курсов. Обращается на cbr.ru раз в 60 минут| — |
| `MigrationService` | Создание БД и миграция | — |

## Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- PostgreSQL 14+

## Настройка
Описание полного пути для тестирования

### 1. Создайте базу данных

```sql
CREATE DATABASE tz_db;
```

### 2. Укажите данные подключения

Отредактируйте строку подключения в трех файлах:

- `UserService/appsettings.json`
- `FinanceService/appsettings.json`
- `CurrencyWorker/appsettings.json`

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=tz_db;Username=postgres;Password=ВАШ_ПАРОЛЬ"
}
```

### 3. Примените миграции

```bash
dotnet run --project MigrationService
```
Сервис создает таблицы `user`, `currency`, `user_favorite_currency`.

### 4. Запустите фоновый сервис курсов

```bash
dotnet run --project CurrencyWorker
```

Сервис загрузит курсы с http://www.cbr.ru/scripts/XML_daily.asp и сохранит в таблицу `currency`. Обновляется каждый час. Для дальнейшей работы скопируйте CurrencyId нужных вам курсов

## Запуск

Запустите каждый сервис:

```bash
dotnet run --project UserService --launch-profile http
```

```bash
dotnet run --project FinanceService --launch-profile http
```

```bash
dotnet run --project ApiGateway --launch-profile http
```

## Тестирование

Swagger для ручного тестирования без API Gateway:
- UserService: http://localhost:5001/swagger
- FinanceService: http://localhost:5002/swagger

Полный путь тестирования через свагер следующий:
- После запуска всех трех сервисов перейдите на http://localhost:5001/swagger. В register введите логин и пароль, отправьте запрос. Затем через  login "войдите", ответом вернется токен, который необходимо скопировать. Далее на http://localhost:5002/swagger выберите Authorize в верхнем правом углу страницы и введите токен, полученный ранее. После "входа" выберите Post, который добавляет валюту в избранное. Вставьте скопированный раннее CurrencyId в поле ввода и отправьте запрос. Далее выполните Get, чтобы получить избранную валюту пользователя.   
---

# Команды для тестирования в консоли через API Gateway

### Регистрация
```bash
curl -X POST http://localhost:5000/api/auth/register -H "Content-Type: application/json" -d "{\"name\": \"USERNAME\", \"password\": \"QWERTY\"}"
```

---

### Логин

```bash
curl -X POST http://localhost:5000/api/auth/login -H "Content-Type: application/json" -d "{\"name\": \"USERNAME\", \"password\": \"QWERTY\"}"
```

Ответ:
```json
{ "token": "eyJ..." }
```

Скопируйте токен.

---

### Добавить валюту в избранное

Возьмите `currencyId` из таблицы `currency` и вставьте в запрос

```bash
curl -X POST http://localhost:5000/finance/favorites/{currencyId} -H "Authorization: Bearer ВАШ_ТОКЕН"
```


---

### Получить курсы избранных валют

```bash
curl http://localhost:5000/finance/rates -H "Authorization: Bearer ВАШ_ТОКЕН"
```

Ответ:
```json
[
  { "name": "USD", "rate": 90.50 },
]
```
---

### Удалить валюту из избранного

```bash
curl.exe -X DELETE http://localhost:5000/finance/favorites/{currencyId} -H "Authorization: Bearer ВАШ_ТОКЕН"
```

### Выход

```bash
curl.exe -X POST http://localhost:5000/api/auth/logout -H "Authorization: Bearer ВАШ_ТОКЕН"
```

---

# Тесты

```bash
dotnet test
```

# Что можно добавить или улучшить
- Заменить кэш на Redis. Не входило в задание, поэтому оставил ConcurrentDictionary в качестве хранилища.
- Контейнеризация. Хотел добавить докерфайлы и докер-компоуз, чтобы разворачивалось одной кнопкой (или командой), но не стал, не входило в задание.
- Добавить рефреш-токены.
- Пагинация для курсов. Сейчас эндпоинт возвращает все валюты, находящиеся в избранном.
- Полноценный API Gateway. Сейчас сервисы доступны в обход YARP.
- Вынести валидацию входных данных в отдельный слой.
