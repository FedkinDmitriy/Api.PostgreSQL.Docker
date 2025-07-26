# Api.PostgreSQL.Docker
# Web API для управления клиентами и заказами

Этот проект представляет собой ASP.NET Core Web API для управления клиентами и их заказами. В проекте используются:

- Entity Framework Core
- PostgreSQL
- Docker + Docker Compose
- Swagger (для документации)
- DTO-модели
- Репозитории с интерфейсом `ICRUDable<T, TId>`
 
### 1. Установка зависимостей

Убедитесь, что у вас установлены:

- [.NET SDK 7.0 или выше](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/ru/) с компонентами для ASP.NET и Docker (опционально)

## 🐳 Запуск PostgreSQL в Docker

В проекте используется Docker Compose.

### Шаги:

1. Откройте терминал в корне проекта
2. Запустите контейнер:

```bash
docker-compose up -d
````

После запуска, PostgreSQL будет доступен на `localhost:5432`. Данные для подключения указаны в `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Username=postgres;Password=251187;Database=db"
}
```

---

## ⚙️ Миграции базы данных

После запуска контейнера и до запуска приложения выполните следующие команды в терминале:

### 1. Добавить миграцию:

```bash
dotnet ef migrations add InitialCreate --project Data --startup-project WebApp
```

### 2. Применить миграции:

```bash
dotnet ef database update --project Data --startup-project WebApp
```

> ⚠️ Убедитесь, что `dotnet-ef` установлен:
>
> ```bash
> dotnet tool install --global dotnet-ef
> ```

---

## 🚀 Запуск API

После запуска БД и применения миграций запустите Web API командой:

```bash
dotnet run --project WebApp
```

Swagger UI будет доступен по адресу:
📎 `https://localhost:<port>/swagger`


## 🧑‍💻 Примеры запросов (Swagger)

* `GET /api/clients` — получить список клиентов
* `GET /api/clients/{id}` — получить клиента с заказами
* `POST /api/orders` — создать заказ
* `DELETE /api/orders/{id}` — удалить заказ

---

## 📌 Заметки

* Используются DTO, чтобы изолировать слои API и БД.
* В будущем планируется полностью заменить использование моделей на DTO в сигнатурах методов контроллеров.

---

## 🛠 Авторы

* [@FedkinDmitriy](https://github.com/FedkinDmitriy)
