# 🏥 Home Healthcare Management System

A full-stack patient care coordination platform that connects **patients**, **caregivers**, and **administrators** in one secure system for visit scheduling, care plans, and health records.

Built with **Angular**, **ASP.NET Core Web API**, **Dapper**, and **SQL Server**.

---

## ✨ Features

- **Role-based access control** — separate dashboards and permissions for Admins, Caregivers, and Patients.
- **JWT authentication & authorization** protecting all sensitive patient health data.
- **Appointment / visit scheduling** with double-booking prevention (unique constraint + conflict check per caregiver/date/time slot).
- **Care plan management** — caregivers create and track medications, goals, and progress per patient.
- **Normalized SQL Server schema** for Users, Patients, Caregivers, Appointments, and Care Plans with referential integrity.
- **Repository Pattern + Dependency Injection** for a clean, testable backend architecture using Dapper for lightweight, high-performance data access.

## 🛠️ Tech Stack

| Layer      | Technology |
|------------|------------|
| Frontend   | Angular 17, TypeScript, RxJS |
| Backend    | ASP.NET Core 8 Web API, C# |
| Data Access| Dapper (Micro-ORM) |
| Database   | SQL Server (T-SQL) |
| Auth       | JWT Bearer Authentication, BCrypt password hashing |
| Docs       | Swagger / OpenAPI |

## 📁 Project Structure

```
HomeHealthcareManagementSystem/
├── backend/
│   ├── HomeHealthcare.API/
│   │   ├── Controllers/        # Auth, Patients, Caregivers, Appointments, CarePlans
│   │   ├── Models/             # Domain entities + DTOs
│   │   ├── Data/               # DapperContext
│   │   ├── Repositories/       # Interfaces + implementations (Repository Pattern)
│   │   ├── Services/           # AuthService, TokenService (JWT)
│   │   ├── Program.cs
│   │   └── appsettings.json
│   └── Database/
│       └── schema.sql          # Full SQL Server schema
└── frontend/
    └── src/app/
        ├── core/                # AuthService, AuthGuard, HTTP interceptor
        ├── models/              # TypeScript interfaces
        ├── components/
        │   ├── login/ register/ navbar/
        │   ├── admin-dashboard/
        │   ├── caregiver-dashboard/
        │   ├── patient-dashboard/
        │   ├── appointment-list/       # shared component
        │   └── care-plan-list/         # shared component
        └── app.module.ts
```

## 🚀 Getting Started

### 1. Database
Run the schema script against your local SQL Server instance:
```bash
sqlcmd -S localhost -i backend/Database/schema.sql
```
This creates the `HomeHealthcareDb` database with all tables (Users, Patients, Caregivers, Appointments, CarePlans).

### 2. Backend (ASP.NET Core API)
```bash
cd backend/HomeHealthcare.API
dotnet restore
dotnet run
```
- Update `appsettings.json` with your SQL Server connection string and a strong JWT secret.
- Swagger UI is available at `https://localhost:7101/swagger` in Development mode.

### 3. Frontend (Angular)
```bash
cd frontend
npm install
ng serve
```
- App runs at `http://localhost:4200`.
- Update `src/environments/environment.ts` if your API runs on a different port.

## 🔐 Authentication Flow

1. User registers as **Admin**, **Caregiver**, or **Patient** via `/api/auth/register`.
2. Passwords are hashed with BCrypt before storage.
3. On login, the API issues a JWT containing the user's role claim.
4. The Angular `AuthInterceptor` attaches the token to every outgoing request.
5. The `AuthGuard` restricts routes based on the role encoded in the token.

## 📌 API Endpoints (summary)

| Method | Endpoint | Description |
|--------|----------|--------------|
| POST | `/api/auth/register` | Register a new user (Patient/Caregiver/Admin) |
| POST | `/api/auth/login` | Authenticate and receive a JWT |
| GET  | `/api/patients` | List all patients (Admin/Caregiver) |
| GET  | `/api/caregivers` | List all caregivers |
| GET/POST | `/api/appointments` | List / book appointments |
| PUT  | `/api/appointments/{id}/status` | Mark visit completed / cancelled |
| GET/POST | `/api/careplans` | List / create care plans |

## 🧑‍💻 Author

**Asad Raza** — .NET Developer | C# | ASP.NET Core | Angular | Web APIs | SQL Server
[github.com/AsadRaza067](https://github.com/AsadRaza067)
