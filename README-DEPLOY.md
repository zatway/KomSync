# KomSync — развёртывание

## Требования

- .NET 9 SDK
- Node.js 20+ (для UI)
- PostgreSQL 14+

## База данных

1. Создайте БД (например `komSync`).
2. Укажите строку подключения в `WebApi/appsettings.json` → `ConnectionStrings:DefaultConnection`.
3. Примените миграции из каталога `Infrastructure/Migrations`:

```bash
cd KomSync/WebApi
dotnet ef database update --project ../Infrastructure/Infrastructure.csproj
```

## API (KomSync/WebApi)

Переменные и секции:

- `ConnectionStrings:DefaultConnection` — PostgreSQL.
- `JwtSettings:Secret` — секрет JWT (≥ 32 символов).
- `SmtpEmail` — SMTP для писем (сброс пароля, напоминания, заявки). При `Enabled: false` письма пишутся в лог (`LoggingEmailSender`).
- `PasswordReset:FrontendBaseUrl` — URL фронтенда для ссылки «сброс пароля» (например `http://localhost:5173`).
- `DeadlineReminders` — фоновые напоминания о дедлайнах (`Enabled`, `IntervalHours`, `OffsetsDays`).
- `SeedAdmin` — опциональное создание администратора при старте (только Development).

Запуск:

```bash
cd KomSync/WebApi
dotnet run
```

По умолчанию Kestrel слушает URL из `launchSettings.json` / переменных окружения.

## UI (KomSync_Ui)

В `src/env.ts` задайте `VITE_API_BASE_URL` (базовый URL API, включая `/api/v1`).

```bash
cd KomSync_Ui
npm ci
npm run build
npm run preview
```

## Нагрузочное тестирование (приёмка)

Для отчёта по п. приёмки можно использовать [k6](https://k6.io/) или [NBomber](https://nbomber.com/): 100+ параллельных запросов к типовым GET (`/api/v1/projects`, `/api/v1/search?q=test`) с заголовком `Authorization: Bearer …`.

## Docker Compose (опционально)

В корне репозитория `docker-compose.yml` поднимает только PostgreSQL для локальной разработки. API и UI запускайте отдельно (см. выше).
