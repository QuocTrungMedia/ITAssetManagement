# IT Asset Management System

A web application for managing employees, computers and IT equipment allocation.

The project was built with ASP.NET Core MVC, Entity Framework Core and SQL Server.

## Screenshots

### Dashboard

![Dashboard]
<img width="1902" height="965" alt="image" src="https://github.com/user-attachments/assets/2da058a7-801c-495d-b797-07b32106de4e" />


### Employee Management

![Employee Management]
<img width="1896" height="961" alt="image" src="https://github.com/user-attachments/assets/a9b9a771-b21c-4ce6-9515-54bc367a74f4" />


### Computer Management

![Computer Management]
<img width="1900" height="967" alt="image" src="https://github.com/user-attachments/assets/050c1406-442f-43b8-8bb1-d368ccf8496b" />


## Main Features

- Dashboard displaying employee and computer statistics
- Employee management: create, update, view and delete
- Computer management: create, update, view and delete
- Assign computers to employees
- Display employees who have not received a computer
- Search employees by name, email or phone number
- Filter employees by department
- Search computers by name, operating system or employee
- Filter computers by status and allocation
- Prevent deleting employees who are currently using computers
- Form validation and operation notifications
- Responsive user interface

## Technologies

- ASP.NET Core MVC 8
- Entity Framework Core 8
- SQL Server
- Razor Views
- Bootstrap 5
- HTML and CSS
- Visual Studio 2022

## Project Structure

```text
ITAssetManagement
├── Controllers
├── Data
├── Migrations
├── Models
├── Views
├── wwwroot
├── appsettings.json
└── Program.cs
```

## Database Structure

The project currently contains two main tables:

- `Employees`: stores employee information
- `Computers`: stores computer information and employee assignments

One employee can be assigned multiple computers. A computer can be unassigned or assigned to one employee.

## Requirements

Before running the project, install:

- Visual Studio 2022
- .NET 8 SDK
- SQL Server
- SQL Server Management Studio

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/QuocTrungMedia/ITAssetManagement.git
```

### 2. Open the project

Open the following solution file with Visual Studio:

```text
ITAssetManagement.sln
```

### 3. Configure the database connection

Open:

```text
ITAssetManagement/appsettings.json
```

Update `YOUR_SERVER_NAME` with your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ITAssetManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

Example:

```text
Server=LAPTOP-NAME\MSSQLSERVER02
```

Do not add SQL Server passwords to the repository.

### 4. Create the database

In Visual Studio, open:

```text
Tools → NuGet Package Manager → Package Manager Console
```

Run:

```powershell
Update-Database
```

Entity Framework Core will create the database and its tables.

### 5. Run the application

Press:

```text
Ctrl + F5
```

The application will open in the browser.

## Author

Developed by [QuocTrungMedia](https://github.com/QuocTrungMedia)
