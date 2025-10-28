# 🎯 YouTube Arduino Tracker – Backend

## TO RUN:
 pwd                                                            
/Users/svitlanak/youtube-tracker-api
dotnet run --project youtubeTrackerApi/youtubeTrackerApi.csproj

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


- register a user
- login
- store a data (timestamp, data, user?)
- send a data to email
- send a tada for a report page

- pring pdf
- login ->  see a history -> (data, raw data, report data (report  generated))
            -downdoad a dataset, see/ download a report
            - delete a record
- logout 



- check regestration
- check data storage
- Later - AI integration
- save report to PDF (see repo)
https://bitbucket.org/svitlana_khmel/tracker-api/src/main/TrackerApi/Models/

// Server=localhost – Refers to a local SQL Server instance.

// Database=EyeTrackingDb – Specifies the name of the database.

// Trusted_Connection=True – Indicates Windows Authentication, a feature of SQL Server.

// MultipleActiveResultSets=true – A setting specific to SQL Server.

// TrustServerCertificate=True – Often used with SQL Server in development environments to bypass SSL certificate validation.
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

Method 2: Edit .csproj File Manually

Open your .csproj file
Add the package reference in the <ItemGroup> section:

<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />


# For PDF generation
```
dotnet add package itext7


```
Or if you prefer to install specific iText components separately:
```
dotnet add package itext7.pdfhtml
dotnet add package itext7.bouncy-castle-adapter
```
Add this to your .csproj file:
```
<PackageReference Include="itext7" Version="8.0.2" />
```
License Note !!!!
⚠️ Important: iText 7 has a dual license:

AGPL (free for open-source projects)
Commercial license (required for commercial/proprietary applications)

Make sure you comply with the appropriate license for your use case.
The main package itext7 should provide all the functionality your controller needs for basic PDF generation.




```
# Core Swagger packages
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Annotations

# For enhanced documentation
dotnet add package Microsoft.OpenApi.Models

```

##How to Test in Swagger UI:

Start your application
Navigate to https://localhost:5001 (or your configured URL)
Use the sample JSON for testing the analytics endpoint:

```
{
  "videoTitle": "Mentality MVP presentation",
  "videoUrl": "https://www.youtube.com/watch?v=vk3aYt7tLXY",
  "sessionStart": "2025-05-28T13:15:04Z",
  "startTime": 12,
  "endTime": 17,
  "gazeDataRecorded": true,
  "fullscreenUsed": false,
  "gazePoints": [
    { "x": 500, "y": 500 },
    { "x": 498, "y": 507 }
  ],
  "engagementSummary": [
    "Short viewing session (~5s)",
    "No pause or seek detected",
    "Viewer gaze detected, suggesting attention"
  ]
}

```
dotnet add package QuestPDF

### TO RUN 
dotnet run --project youtubeTrackerApi/youtubeTrackerApi.csproj
### SEE ENDPOINTS:
http://localhost:5118/swagger

