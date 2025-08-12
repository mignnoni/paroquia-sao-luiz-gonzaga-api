# 🏛️ São Luiz Gonzaga Parish - API

A complete API for parish management, developed in .NET 9 with modular architecture and Clean Architecture.

## 📋 About the Project

This API was specifically developed to manage the activities of **São Luiz Gonzaga Parish**, offering comprehensive functionalities for parish administration, member management, mass scheduling, and notifications.

## 🎯 Main Features

### 🔐 **Identity Module (IdentityProvider)**

- **Authentication and Authorization**

  - JWT Token login
  - User creation
  - Password recovery via email
  - Password reset
  - User deletion
  - Role-based permissions system

- **Available Roles:**
  - `admin` - Full administrator
  - `supervisor` - Manager
  - `member` - Member

### 📊 **Parish Management Module (ParishManagement)**

#### **Mass Management**

- **Mass Locations**

  - Create/edit/delete mass locations
  - Designate headquarters
  - Configure addresses

- **Mass Scheduling**
  - Create schedules by day of the week
  - Manage mass times
  - Add/remove specific times
  - Update existing schedules

#### **Member Management**

- **Active Members**

  - Regular member registration
  - Different types: Regular, Manager, Admin
  - Soft delete control

- **Pending Members**
  - Email invitation system
  - Confirmation tokens
  - New member approval

#### **Other Schedules**

- **Content Types:**

  - `News` - News
  - `GroupsAndServices` - Groups and Services
  - `Sacrament` - Sacraments
  - `TransparencyPortal` - Transparency Portal

- **Features:**
  - File upload
  - AWS S3 integration
  - Rich Text title and content
  - Type categorization

### 📧 **Notifications Module (Notification)**

- **Automated Emails**

  - HTML templates
  - Password recovery
  - Password change confirmation
  - New member invitations
  - Responsive design

- **Messaging System**
  - RabbitMQ integration
  - Asynchronous processing
  - Integration events between modules

## 🏗️ Architecture

### **Used Patterns**

- **Clean Architecture** - Clear separation of responsibilities
- **Domain-Driven Design (DDD)** - Domain-based modeling
- **CQRS** - Command and query separation
- **Mediator Pattern** - Decoupled communication with MediatR
- **Repository Pattern** - Data layer abstraction
- **Unit of Work** - Transaction control

### **Modular Structure**

```
src/
├── API/                          # Presentation layer
│   └── ParoquiaSLG.API/         # Main API
├── BuildingBlocks/              # Shared components
│   └── BuildingBlocks/          # Common infrastructure
└── Modules/                     # Business modules
    ├── IdentityProvider/        # Authentication and authorization
    ├── Notification/            # Notification system
    └── ParishManagement/        # Parish management
```

### **Layers per Module**

- **Application** - Use cases and commands/queries
- **Domain** - Entities, aggregates and business rules
- **Infrastructure** - Concrete implementations
- **Persistence** - Data access and migrations
- **Endpoints** - API contracts (IdentityProvider)
- **IntegrationEvents** - Events between modules

## 🛠️ Technologies Used

### **Framework and Runtime**

- .NET 9.0
- ASP.NET Core Web API
- Minimal APIs

### **Database**

- PostgreSQL
- Entity Framework Core 9.0
- Npgsql (PostgreSQL Driver)
- Automatic migrations

### **Patterns and Libraries**

- **MediatR** - Mediator pattern
- **Ardalis.Result** - Result pattern
- **Ardalis.Specification** - Specification pattern
- **FluentValidation** - Data validation

### **Infrastructure**

- **RabbitMQ** - Message broker
- **AWS S3** - File storage
- **SMTP** - Email sending

### **Authentication**

- JWT Bearer Tokens
- ASP.NET Core Identity
- Role and permission system

## 🚀 How to Run

### **Prerequisites**

- .NET 9.0 SDK
- PostgreSQL
- RabbitMQ (optional for development)
- AWS S3 account (for file uploads)

### **Setup**

1. **Clone the repository**

```bash
git clone <repository-url>
cd paroquia-sao-luiz-gonzaga-api
```

2. **Configure the database**

```bash
# Set environment variable or configure in appsettings.json
export DATABASE_URL="Host=localhost;Database=paroquia_db;Username=user;Password=password"
```

3. **Run migrations**

```bash
dotnet ef database update --project src/Modules/IdentityProvider/Modules.IdentityProvider.Persistence
dotnet ef database update --project src/Modules/ParishManagement/Modules.ParishManagement.Persistence
```

4. **Configure environment variables (optional)**

```bash
# RabbitMQ
export RABBITMQ_HOST=localhost
export RABBITMQ_PORT=5672
export RABBITMQ_USERNAME=guest
export RABBITMQ_PASSWORD=guest

# AWS S3
export AWS_ACCESS_KEY_ID=your_access_key
export AWS_SECRET_ACCESS_KEY=your_secret_key
export AWS_REGION=your_region
export S3_BUCKET_NAME=your_bucket

# Email SMTP
export SMTP_HOST=smtp.gmail.com
export SMTP_PORT=587
export SMTP_FROM_EMAIL=your-email@gmail.com
export SMTP_API_KEY=your_app_password
```

5. **Run the application**

```bash
dotnet run --project src/API/ParoquiaSLG.API
```

## 📝 License

This project is proprietary to São Luiz Gonzaga Parish.
