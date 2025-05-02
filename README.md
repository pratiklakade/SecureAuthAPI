# 🔐 JWT Authentication API with Role-Based Authorization (ASP.NET Core)

This project is a complete implementation of **JWT Authentication**, **Refresh Token**, and **Role-Based Authorization** (Admin/User). Ideal for learning and job-ready backend development.

---

## 🧰 Tech Stack
- ✅ ASP.NET Core Web API (.NET 8)
- ✅ Entity Framework Core + SQL Server
- ✅ JWT (JSON Web Token)
- ✅ Role-Based Authorization (Admin/User)
- ✅ Refresh Token Implementation
- ✅ Password Hashing (SHA256)
- ✅ Swagger for API Testing

---

## 📦 Features

✅**User Registration & Login** with password hashing  
✅ **JWT Token** generation on login  
✅ **Password Hashing using SHA256**  
✅ **Role-Based Authorization**: Admin vs User access  
✅ **Refresh Token** endpoint to renew expired JWT 
✅ **Secure Endpoints** using `[Authorize(Roles = "...")]`
✅ **SQL Server DB with EF Core**  
✅ **Swagger** UI for API Testing  

---

## 📁 Endpoints

| Method | Endpoint            | Description               | Access |
|--------|---------------------|---------------------------|--------|
| POST   | `/api/auth/register`| Register new user         | Public |
| POST   | `/api/auth/login`   | Login and get tokens      | Public |
| POST   | `/api/auth/refresh` | Get new token from refresh| Public |
| GET    | `/api/admin/data`   | Admin-only data           | Admin  |
| GET    | `/api/user/data`    | User-only data            | User   |

---

## 🛠 Setup

```bash
git clone https://github.com/pratiklakade/SecureAuthAPI.git
cd SecureAuthAPI

Update appsettings.json with your SQL Server connection string

Run EF Core Migration:
dotnet ef migrations add InitialCreate
dotnet ef database update

Run the project:
dotnet run

🧪 Swagger UI
Visit: https://localhost:xxxx/swagger
You can test Register/Login/Refresh/Admin APIs here.
Test in Swagger
Register a user (/api/auth/register)
Login to get JWT (/api/auth/login)
Use JWT in Authorization Header (Bearer <token>)
Access secured routes based on role
Role-Based Access Example
Endpoint	Role Required
/api/secure/user-data	   User
/api/secure/admin-data	 Admin

Project Structure
/Controllers
  - AuthController.cs
  - AdminController.cs
  - UserController.cs
/Models
  - User.cs
  - LoginRequest.cs
  - RegisterRequest.cs
/Services
  - JwtService.cs
  - PasswordHasher.cs
/DTOs
  - TokenModel.cs
/Data
  - AppDbContext.cs
Program.cs

🧭 Future Development Plan
🔧 Feature	Description
🔒 Token Revocation	Blacklist tokens for logout or user disable flow.
🔁 Token Rotation	Auto-refresh token rotation to prevent reuse.
🌐 OAuth Integration	Google/Microsoft login via OAuth2.0.
🧑‍💻 Admin Dashboard	Frontend (Blazor/React) for managing users/roles.
🛡️ OWASP Protection	Add rate-limiting, input validation, etc.
📧 Email Features	Email verification + password reset flow.
📊 Audit Logs	Track login, refresh, and endpoint access in DB.

🔗 License
MIT License

📌 Author
👨‍💻 Pratik Lakade L.
📧 Email: pratiklakadepl@gmail.com
📎 LinkedIn: https://www.linkedin.com/in/pratik-lakade-9aaab6136/

