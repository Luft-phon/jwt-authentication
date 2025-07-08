<p align="center">
    <img alt="Fav Icon Png" src="https://github.com/KevinTrinh1227/Reactfolio/blob/master/public/assets/readme-icon.png" width="100"/>
</p>
<h1 align="center">JWT Authentication</h1> 

## 🌐 What is JWT?
JWT authentication (JSON Web Token authentication) is a popular user authentication mechanism in modern web applications, especially RESTful API systems.

## 📌 Prerequisite
- .NET 9 SDK
- Visual Studio 2022+ or Visual Studio Code
- Postman
- Microsoft SQL Server
- Required NuGet Packages (System.IdentityModel.Tokens.Jwt, Microsoft.AspNetCore.Authentication.JwtBearer)

## 🛠 Configuration Setup
- In appsetting.json, we must defind 
```
"AppSettings": {
  "Token": "your-256-bit-secret-key",
  "Issuer": "YourApp",
  "Audience": "YourAppUsers"
}
```
 to store authenticated configuration values

## 📁 Project Structure

```
JwtAuthentication/
├── Controller/    # Navigate logic code
├── Data/          # Connect to Database 
├── Entity/        # Objects
├── Models/        # Data Object Transfer
├── Services/      # Logic code
```
