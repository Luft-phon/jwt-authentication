<p align="center">
    <img alt="Fav Icon Png" src="https://github.com/KevinTrinh1227/Reactfolio/blob/master/public/assets/readme-icon.png" width="100"/>
</p>
<h1 align="center">JWT Authentication</h1> 

## 🌐 What is JWT?
JWT authentication (JSON Web Token authentication) is a popular user authentication mechanism in modern web applications, especially RESTful API systems.

## 🌟  Main Features 
- Register new user
- Login with authorized & authenticated user account

## 📌 Prerequisite
- .NET 9 SDK
- Visual Studio 2022+ or Visual Studio Code
- Postman
- Microsoft SQL Server
- Required NuGet Packages (System.IdentityModel.Tokens.Jwt, Microsoft.AspNetCore.Authentication.JwtBearer)

## 🛠 Getting Started
1. Clone this repository
```
git clone https://github.com/Luft-phon/jwt-authentication.git
```
2. Open the project folder
3. In appsetting.json, we must defind  
```
"AppSettings": {
  "Token": "your-256-bit-secret-key",
  "Issuer": "YourApp",
  "Audience": "YourAppUsers"
}
```
 to store authenticated configuration values

 4. Add Migration
```
## Using Package Manager Console in Visual Studio
Add-Migration NameMigration
```
5. Update Database
```
Update-Database
```

## 📁 Project Structure

```
JwtAuthentication/
├── Controller/    # Navigate logic code
├── Data/          # Connect to Database 
├── Entity/        # Objects
├── Models/        # Data Object Transfer
├── Services/      # Logic code
```
