# 24-58743-2-Login-System
# C# Login, Registration and Logout System using SQL Server

## About the Project

This is a C# Windows Forms application for user registration, login, and logout. The application uses SQL Server to store user information. Users can create a new account, log in with their username and password, and access the dashboard after successful login.

## Technologies Used

* C#
* Windows Forms
* .NET Framework 4.7.2
* SQL Server
* ADO.NET
* Visual Studio

## How to Run the Project

### 1. Create the Database

First, open SQL Server Management Studio and run the `database.sql` file.

It will create:

* Database: `db_users`
* Table: `tbl_users`

It also creates a test account.

**Username:** `admin`
**Password:** `admin123`

### 2. Check App.config

Open the `App.config` file and check the connection string.

If your SQL Server name is different, change only the `Data Source` part.

For example:

xml
<connectionStrings>
    <add name="connString"
         connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=db_users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False" />
</connectionStrings>


If you are using another SQL Server instance, change:

text
Data Source=(localdb)\MSSQLLocalDB


to your own server name.

### 3. Run the Application

Open the solution file in Visual Studio and build the project.

Then run the application.

You can test the login with:

text
Username: admin
Password: admin123


## What I Changed

The original project was using Microsoft Access to store the user information. I changed the database connection from Access to SQL Server because SQL Server is more suitable for this project and it is also required for this lab.

### Files I Edited

I mainly changed these files:

* `frmLogin.cs`
* `frmRegister.cs`
* `frmDashboard.cs`
* `Program.cs`
* `App.config`

I also added:

* `database.sql`
* `README.md`

### Changed OleDb to SqlClient

The original application used `System.Data.OleDb` to connect with the Access database.

I replaced it with:

csharp
using System.Data.SqlClient;


I used `SqlConnection` and `SqlCommand` to connect and communicate with SQL Server.

The old Access connection was removed because the project no longer uses the `.mdb` database.

### Why I Put the Connection String in App.config

I kept the database connection string inside `App.config` instead of writing it separately in every form.

This makes the project easier to manage. If the SQL Server name or database changes, I only need to update the connection string in one place instead of changing the code in different forms.

The C# code reads the connection string using `ConfigurationManager`.

### Why I Used @username and @password

I used `@username` and `@password` as SQL parameters.

For example:

csharp
cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
cmd.Parameters.AddWithValue("@password", txtPassword.Text);


This is safer than directly putting the user's input inside the SQL query. It helps protect the application from SQL injection.

## Registration

The registration form checks that the username and password fields are not empty. It also checks whether the two passwords match and whether the username already exists.

If everything is correct, the new user is inserted into the SQL Server database.

## Login

The login form checks the entered username and password with the data stored in SQL Server.

If the information is correct, the Dashboard opens. If the password is wrong, an error message is shown.

## Logout

I changed the Logout button so that it returns the user to the Login screen instead of closing the complete application.

I also changed the startup form in `Program.cs` so that the application starts from the Login screen.

## Bonus: SHA-256 Password Hashing

For the bonus part, I changed the application to store passwords as SHA-256 hashes instead of storing the actual passwords as plain text.

When a user registers, the password is converted into a SHA-256 hash before it is saved in the database. When the user logs in, the entered password is hashed again and compared with the stored hash.

Plain-text passwords are unsafe because anyone who gets access to the database can directly see the users' passwords. Storing a hash instead means the original password is not directly stored in the database.

I used `SHA256` from `System.Security.Cryptography` for this.

## Screenshots

### Login Page

<img width="380" height="628" alt="Screenshot 2026-09-08 034407" src="https://github.com/user-attachments/assets/fea94d13-e717-4f75-b691-d5d917e2c6b2" />


### Registration Page

<img width="377" height="711" alt="Screenshot 2026-09-08 040936" src="https://github.com/user-attachments/assets/7fa2ce71-1fe3-4588-bfb7-bd64cb623893" />


### Dashboard
<img width="1013" height="743" alt="Screenshot 2026-09-08 041115" src="https://github.com/user-attachments/assets/5ad725aa-7eb7-4ed9-ad70-80bd57c88031" />

## Project Structure

Login-and-Register-main/
App.config
database.sql
Program.cs
frmLogin.cs
frmRegister.cs
frmDashboard.cs
README.md


## Conclusion

Through this project, I changed the original Login and Registration application from Microsoft Access to SQL Server. I also implemented parameterized SQL queries, proper logout functionality, and SHA-256 password hashing for better password security.
