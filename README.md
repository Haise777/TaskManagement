<h1 align="center">
  <br>
  Task Management WebAPI
  <br>
</h1>

<h4 align="center">A Task assignment and Management WebAPI that you can employ on your bussiness to Assign, Delete and Modify all details of a Task, wheter it be it's Description, Priority or it's current assigned users. </h4>

<div align="center">
  
  <a href="">![Static Badge](https://img.shields.io/badge/ASP.NET-6.0-%233502b8?style=flat-square)</a>
  <a href="">![Static Badge](https://img.shields.io/badge/EF.Core-6.0.33-%237c00ad?style=flat-square)</a>
  <a href="">![Static Badge](https://img.shields.io/badge/DB-SQLServer-%230c00ad?style=flat-square)</a>
  <a href="">![GitHub License](https://img.shields.io/github/license/Haise777/OPZBot?style=flat-square&color=%23a38802)</a>
  
</div>

<p align="center">
  <a href="#key-features">Key Features</a> •
  <a href="#setting-up">Setting up</a> •
  <a href="#usage">Usage</a> •
  <a href="#credits">Credits</a>
</p>

Key Features
----

* JWT-Based Auth
  - The API issues a unique [JWT](https://jwt.io/) as a response from the user's succesful authentication
  - It contains all of the user's needed [Claims](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims) for the Authorization and various other processes to work with
  - Fully hashed and cryptographed in [HmacSHA256](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.hmacsha256) algorithm
  <br>
* Account management
  - Fully implemented endpoints that allow a client to Sign up, Login, change Username, Password and Delete it's account with all of their created tasks.
  - Admins can change other user's roles, usernames and delete their account.
  <br>
* Task Management
  - With the Task endpoint, a authenticated user can Fetch, Create, Delete, Modify, Assign or Deassign other users from their tasks.
  - Tasks can be filtered in the fetching Endpoint's URL
  - Other users Tasks can be fetched by passing their URL to the Endpoint
  - All changes related to their Tasks are always saved as a persistent data on a SQLServer Database
  - Admin accounts can Manage other user's Tasks.
  <br>
Setting up
----

<h3>Setting up</h3>  

* You need to have [.NET SDK 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or higher installed on your machine
* Access to a Microsoft SQLServer instance.
<br>
<h4>1. Clone the repository to your machine </h4>  

```bash
$ git clone https://github.com/Haise777/TaskManagement.git
```
<h4>2. Resolve all packets dependencies</h4>  

```bash
$ cd TaskManagement/WebAPI
$ dotnet restore
```
<h4>3. Build all projects</h4>  

```bash
$ dotnet build
```
<h4>4. Insert you Db Connection String</h4>  

```bash
$ cd WebAPI/TaskManagement.API/bin/Debug/net6.0/
# Edit the 'appsettings.Development.json' file and place your Connection String in the "TestDb" value.
```
<h4>5. Run the API</h4>  

```bash
$ cd WebAPI/TaskManagement.API/bin/Debug/net6.0/
$ ./TaskManagement.API
```

## Usage

***(In progress)*** It is designed to be used with a Frontend app as a client to call and manage it's operations.
But you can still test and utilize it to a extent by calling it's endpoint with a tool such as [Postman](https://www.postman.com/downloads/)

<h3>Here is *some* of the endpoints you could try</h3>

```
>[POST] http://localhost:5108/auth/signup
 Body Raw-Json: {
    "UserName": "Chris_Redfield",
    "Password": "BolderPuncher2",
    "Email": "fakeemail@mail.com"
}

>[POST] http://localhost:5108/auth/login
 Body Raw-Json: {
    "UserName": "Chris_Redfield",
    "Password": "BolderPuncher2",
}


# You have to copy the returned Token and set it to your Authorization header to proceed
# Headers -> Authorization : Bearer eyIJPWAjd198jdw890scp...

>[POST] http://localhost:5108/tasks/createtask
 Body Raw-Json: {
    "Title": "This is the first task",
    "Description": "This is a test task",
    "Priority": 3,
}

>[GET] http://localhost:5108/tasks/createtask
```

## Credits

This software uses the following open source packages:

- [ASP.NET](https://github.com/dotnet/aspnetcore/tree/main)
- [EF Core](https://github.com/dotnet/efcore)

---
*OPZBot is released under [BSD 3-Clause license](https://opensource.org/license/bsd-3-clause/)*

> *Contact me*\
> *Email:* [gashimabucoro@proton.me](mailto:gashimabucoro@proton.me) &nbsp;&middot;&nbsp;
> *Discord:* [@.haise_san](https://discord.com/users/374337303897702401)
