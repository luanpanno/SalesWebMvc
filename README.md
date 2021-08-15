# 🚀 Sales Web MVC

by Luan Panno

## ⚙️ Stack

- C#
- .NET
- ASP.NET
- Entity Framework Core
- MySQL

## 🔧 How to run this project using .NET CLI

1.  Go to `SalesWebMvc` folder inside `src`

            cd ./src/SalesWebMvc/

2.  Setup MySQL with docker-compose

            docker-compose up -d

3.  Run the migrations to create the database

            dotnet ef database update

4.  Run the project by the following line

            dotnet run

PS: You need Docker Compose to setup the MySQL. Otherwise, you must install MySQL and edit the `appsettings.json` with your own database credentials.
