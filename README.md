# 🎯 YouTube Arduino Tracker – Backend

This is the **backend API** for the **YouTube Arduino Tracker Chrome Extension**. It is built using **ASP.NET Core 8** and serves as the main data layer and service provider for tracking, storing, and serving information captured by the Chrome extension connected to Arduino-based hardware.

---

## 🚀 Tech Stack

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
  - SQL Server
  - In-Memory DB (for testing)
- **Swagger / OpenAPI**
- **ASP.NET Core Identity**
- **PDFSharp** (for PDF generation)

---

## 📦 Key NuGet Packages

| Package                                              | Purpose                                  |
|------------------------------------------------------|------------------------------------------|
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore`  | User authentication and identity support |
| `Microsoft.AspNetCore.OpenApi`                       | OpenAPI/Swagger integration              |
| `Microsoft.EntityFrameworkCore.*`                    | Database access (SQL Server, InMemory)   |
| `Microsoft.EntityFrameworkCore.Tools` & `Design`     | EF Core CLI and design-time tools        |
| `Microsoft.Extensions.Configuration.Json`            | JSON-based configuration                 |
| `PdfSharp`                                           | PDF generation                           |
| `Swashbuckle.AspNetCore`                             | Swagger UI for API documentation         |

---

## 🛠️ Development Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server (optional if using InMemory DB for dev/testing)

### Getting Started

1. **Clone the repository**  
   ```bash
   git clone https://svetlana_khmel@bitbucket.org/svitlana_khmel/youtube-tracker-api.git
```
Restore packages

```
???
dotnet restore
```
Apply migrations (if using SQL Server)

```
dotnet ef database update
```
Run the API
```
dotnet run
```

###View API docs
### Navigate to: https://localhost:5001/swagger

###🧪 Testing
You can use the in-memory database to mock and test data interactions locally without a real DB.

###📄 License
MIT or specify your license here.


@todo::

- check regestration
- check data storage
- Later - AI integration
- save report to PDF (see repo)
https://bitbucket.org/svitlana_khmel/tracker-api/src/main/TrackerApi/Models/