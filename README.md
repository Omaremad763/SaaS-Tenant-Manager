[![SaaS System CI/CD Pipeline](https://github.com/Omaremad763/SaaS-Tenant-Manager/actions/workflows/Predeploy.yml/badge.svg?branch=PreDeploy)](https://github.com/Omaremad763/SaaS-Tenant-Manager/actions/workflows/Predeploy.yml)
[![Unit Tests](https://img.shields.io/badge/Unit_Tests-Passed-brightgreen?style=flat&logo=github)](https://github.com/Omaremad763/SaaS-Tenant-Manager/actions)
[![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=flat&logo=.net&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![Angular](https://img.shields.io/badge/Angular_21-DD0031?style=flat&logo=angular&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![Git Hooks](https://img.shields.io/badge/Git_Hooks-Enabled-blue?style=flat&logo=git&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![Nginx](https://img.shields.io/badge/Nginx-009639?style=flat&logo=nginx&logoColor=white)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean--Architecture-blueviolet?style=flat)](https://github.com/Omaremad763/SaaS-Tenant-Manager)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat)](https://github.com/Omaremad763/SaaS-Tenant-Manager/blob/main/LICENSE)

#  SaaS Tenant Manager

---

**A learning-focused project that explores how real SaaS applications isolate tenants, provision databases dynamically, organize business logic, and build scalable backend architectures.**


# 📚 Table of Contents

- [📖 Why I Built This Project](#-why-i-built-this-project)
- [🎯 What I Wanted to Learn](#-what-i-wanted-to-learn)
- [🏢 The Core Idea](#-the-core-idea)
- [🛠 Technology Stack](#-technology-stack)
- [🏗 System Architecture](#-system-architecture)
- [🧩 Engineering Decisions](#-engineering-decisions)
- [🏢 Multi-Tenant Architecture](#-multi-tenant-architecture)
- [⚙️ Automatic Tenant Provisioning](#️-automatic-tenant-provisioning)
- [🔄 Request Lifecycle](#-request-lifecycle)
- [🗃 Master Database vs Tenant Database](#-master-database-vs-tenant-database)
- [📂 Inside the Repository](#-inside-the-repository)
- [🔐 Authentication](#-authentication)
- [📦 Current Modules](#-current-modules)
- [🐳 Docker Support](#-docker-support)
- [🚀 Getting Started](#-getting-started)
- [📚 What This Project Taught Me](#-what-this-project-taught-me)
- [🚀 What's Next?](#-whats-next)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)
- [👨‍💻 Author](#-author)
- [⭐ Support](#-support)

---

# 📖 Why I Built This Project

Most learning projects stop after implementing authentication and CRUD operations.

I wanted to go beyond that.

Instead of asking:

> **"How do I build a logistics management system?"**

I asked different questions.

- How does a SaaS application onboard a new company?
- How does every customer get their own database?
- How can one application communicate with multiple databases?
- How does the backend know which database belongs to which tenant?
- How can features change depending on a customer's subscription?
- How should a large backend project be organized?

This project became my playground for exploring those ideas.

The logistics domain simply provided a realistic scenario to apply them.

# 🎯 What I Wanted to Learn

The goal of this project wasn't to build a production-ready logistics platform.

Instead, I wanted to understand how modern SaaS systems are designed internally.

Throughout the project I explored:

- Designing a Database-per-Tenant architecture.
- Working with multiple EF Core DbContexts.
- Creating tenant databases automatically.
- Running EF Core migrations programmatically.
- Building custom ASP.NET Core middleware.
- Separating business logic using Clean Architecture.
- Organizing application flows with CQRS.
- Implementing feature-based subscriptions.
- Tracking tenant activity.
- Structuring a project that can continue growing over time.

Every new feature was added because it introduced a new architectural concept worth learning.

# 🏢 The Core Idea

Imagine three logistics companies using the same SaaS application.

Each company expects:

- Complete data isolation.
- Independent business operations.
- Different subscription plans.
- Different enabled features.

This project explores one possible solution.

Instead of storing all companies inside the same database, every tenant receives a dedicated SQL Server database while the platform maintains a single Master Database for shared platform information.

This architecture is commonly known as **Database-per-Tenant Multi-Tenancy**, and implementing it became the central learning objective of this project.

---

# 🛠 Technology Stack

The project combines several technologies to explore different aspects of modern SaaS application development.

| Category | Technologies |
|----------|--------------|
| Backend | ASP.NET Core, C# |
| Frontend | Angular |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | JWT |
| Architecture | Clean Architecture, CQRS |
| Design Patterns | Repository Pattern, Unit of Work |
| Mapping | AutoMapper |
| Containerization | Docker, Docker Compose |
| DevOps | GitHub Actions, Husky |

---

# 🏗 System Architecture

Rather than placing all logic inside controllers, the application is divided into independent layers where each one has a clear responsibility.

```
                    Angular Client
                           │
                           ▼
                  ASP.NET Core API
                           │
            ┌──────────────┴──────────────┐
            │     Middleware Pipeline     │
            └──────────────┬──────────────┘
                           │
                           ▼
                  Application Layer
                     (CQRS & Services)
                           │
                           ▼
                     Domain Layer
                  (Business Rules Only)
                           │
                           ▼
                 Infrastructure Layer
          (Repositories • EF Core • Services)
                           │
              ┌────────────┴────────────┐
              ▼                         ▼
       Master Database          Tenant Database
```

The goal of this architecture is not complexity.

It is separation.

Every layer focuses on one responsibility, making the project easier to understand, maintain, and extend.

---

# 🧩 Engineering Decisions

One thing I learned while building this project is that architectural decisions are just as important as writing code.

Here are some of the decisions I made and why I made them.

| Decision | Why? |
|----------|------|
| Database-per-Tenant | To understand tenant isolation and dynamic database provisioning. |
| Two DbContexts | To separate platform data from tenant business data. |
| Clean Architecture | To keep business logic independent from frameworks. |
| CQRS | To organize application operations into clear responsibilities. |
| Repository Pattern | To decouple data access from business logic. |
| Unit of Work | To coordinate repository operations consistently. |
| Middleware Pipeline | To move cross-cutting concerns away from controllers. |
| Domain Events | To automate tenant provisioning after registration. |

These decisions were made primarily for learning purposes and to better understand how large applications are structured.

---

# 🏢 Multi-Tenant Architecture

The platform uses a **Database-per-Tenant** strategy.

Instead of storing every tenant inside the same database, each tenant owns an independent SQL Server database.

```
                   Master Database
             ┌────────────────────────┐
             │ Tenants                │
             │ Users                  │
             │ Plans                  │
             │ Features               │
             └──────────┬─────────────┘
                        │
        ┌───────────────┼───────────────┐
        ▼               ▼               ▼

   Tenant A DB     Tenant B DB     Tenant C DB

   Clients         Clients         Clients
   Shipments       Shipments       Shipments
   Items           Items           Items
```

The Master Database stores platform-level information.

Each Tenant Database stores only business data that belongs to a single tenant.

This isolation became the core architectural idea behind the project.

---

# ⚙️ Automatic Tenant Provisioning

Creating a new tenant involves much more than inserting a record into a database.

When a company registers, the application automatically provisions everything required for that tenant.

```
Register Company
        │
        ▼
Create Tenant
        │
        ▼
TenantCreatedEvent
        │
        ▼
DbMigrationService
        │
        ▼
Create SQL Database
        │
        ▼
Run EF Core Migrations
        │
        ▼
Tenant Ready
```

This workflow was one of the most enjoyable parts of the project because it introduced me to runtime database creation and automated migrations.

---

# 🔄 Request Lifecycle

Every request passes through several stages before reaching the business logic.

```
Angular Client

        │

        ▼

ASP.NET Core API

        │

        ▼

JWT Authentication

        │

        ▼

Tenant Security Middleware

        │

        ▼

Feature Access Middleware

        │

        ▼

Usage Tracking Middleware

        │

        ▼

API Logging Middleware

        │

        ▼

Controller

        │

        ▼

CQRS

        │

        ▼

Service

        │

        ▼

Repository

        │

        ▼

Tenant Database
```

Each middleware has a dedicated responsibility, allowing controllers to stay clean and focused on handling requests.

---

# 🗃 Master Database vs Tenant Database

One of the main concepts I wanted to explore was how multiple databases can work together inside a single application.

Instead of using one database for everything, the project separates platform information from business information.

### Master Database

Stores platform-wide data:

- Tenants
- Users
- Subscription Plans
- Feature Flags
- API Logs

### Tenant Database

Stores business data:

- Clients
- Shipments
- Shipment Items

Using two DbContexts made this separation much cleaner and easier to maintain.

---

# 📂 Inside the Repository

The solution is organized into multiple projects, each with a specific responsibility.

```
SaaS Tenant Manager
│
├── API                 → REST API & Middleware
├── Application         → CQRS, DTOs, Contracts
├── Domain              → Business Entities
├── Infrastructure      → EF Core, Repositories, Services
├── Front               → Angular Application
├── SaaSTestProject     → Unit Tests
│
├── docker-compose.yml
└── GitHub Actions
```

### API

Handles incoming HTTP requests and exposes the application's endpoints.

Responsibilities include:

- Controllers
- Authentication
- Authorization
- Middleware
- Exception Handling

---

### Application

Contains the application's use cases.

Responsibilities include:

- CQRS
- DTOs
- Service Contracts
- Repository Contracts
- AutoMapper
- Domain Events

---

### Domain

The center of the application.

Contains only business entities.

No Entity Framework.

No Controllers.

No Infrastructure.

Only business rules.

---

### Infrastructure

Responsible for everything related to implementation.

Including:

- Entity Framework Core
- SQL Server
- Repositories
- Unit of Work
- Services
- Migrations

---

### Front

Angular application responsible for interacting with the backend through REST APIs.

Current pages include:

- Login
- Tenant Registration
- Dashboard
- Shipment Management
- Client Management
- Administration Panel

---

# 🔐 Authentication

Authentication is implemented using JWT.

Once authenticated, every request goes through multiple validation stages before reaching the business logic.

Authentication Flow

```
Login

↓

JWT Token

↓

API

↓

Authentication

↓

Tenant Validation

↓

Feature Validation

↓

Business Logic
```

This layered approach keeps controllers lightweight while centralizing security concerns inside middleware.

---

# 📦 Current Modules

The project currently includes the following modules.

| Module | Purpose |
|---------|---------|
| Authentication | User authentication and authorization |
| Tenant Management | Tenant onboarding and provisioning |
| Subscription | Subscription plans and feature flags |
| Shipment | Shipment management |
| Client | Client management |
| Administration | SaaS administration dashboard |
| Monitoring | API usage tracking and logging |

---

# 🐳 Docker Support

The repository contains everything needed to run the application using Docker.

Included files:

- Dockerfile (API)
- Dockerfile (Angular)
- Docker Compose

This allows the frontend and backend to run consistently across different environments.

---

# 🚀 Getting Started

## Clone the Repository

```bash
git clone https://github.com/omaremad763/SaaS-Tenant-Manager.git

cd SaaS-Tenant-Manager
```

---

## Configure Environment

Create a `.env` file using the provided example.

```bash
cp example.env .env
```

Update your SQL Server connection strings before running the application.

---

## Run Backend

```bash
cd API

dotnet restore

dotnet run
```

---

## Run Frontend

```bash
cd Front

npm install

ng serve
```

---

## Run with Docker

```bash
docker compose up --build
```

---

# 📚 What This Project Taught Me

Looking back, this project became much more than a logistics application.

It became a hands-on exploration of software architecture.

Some of the concepts I gained practical experience with include:

- Designing a Database-per-Tenant architecture.
- Working with multiple EF Core DbContexts.
- Creating databases dynamically.
- Running EF Core migrations programmatically.
- Building custom middleware.
- Structuring applications using Clean Architecture.
- Separating reads and writes using CQRS.
- Organizing repositories and services.
- Understanding how SaaS applications isolate tenant data.

Perhaps the biggest lesson was realizing that building scalable software is often more about architecture and responsibility separation than about writing more code.

---

# 🚀 What's Next?

Although the project already demonstrates many architectural concepts, there are several ideas I'd like to explore in the future.

Some possible improvements include:

- Refresh Tokens
- Background Jobs
- Email Notifications
- File Storage
- Rate Limiting
- Distributed Caching
- OpenTelemetry
- Kubernetes
- Payment Integration
- Horizontal Scaling

As I continue learning, this repository will continue evolving.

---

# 🤝 Contributing

Suggestions, discussions, and pull requests are always welcome.

If you have ideas for improving the architecture or introducing additional SaaS concepts, feel free to contribute.

---

# 📄 License

This project is licensed under the MIT License.

See the LICENSE file for more details.

---

# 👨‍💻 Author

## Omar Emad

Software Engineer | Full Stack Development

LinkedIn

> https://www.linkedin.com/in/omar-abusaif/

Portfolio

> (https://omar-emad.vercel.app

---

# ⭐ Support

If this project helped you learn something new about multi-tenant architecture or SaaS application design, consider giving the repository a ⭐.

It helps other developers discover the project and encourages future improvements.
