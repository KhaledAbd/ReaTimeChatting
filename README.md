# RealTime Application.

<p align="center">
  Chatting Application
</p>  

&nbsp;

<p align="center">
  <img width="320" height="300" src="https://cdn2.vectorstock.com/i/1000x1000/42/21/man-and-woman-chatting-online-on-their-smartphones-vector-20584221.jpg">
</p>

# Developers

-[ Khaled Mohammed Elshoky -Full-Stack-](https://github.com/KhaledAbd/) Full-Stack Developer- Angular & Dot Net Core FrameWork.


> What is the language of the app?
- This app was built in english language.

&nbsp;

## Description
Mazia is a simple web application: 
- This application is cross-platform. So, you can run it in windows or linux or mac.. etc.
- Applay interval Web Applications.
- use SignalR RealTime Library from Microsoft.
- Angular 12.
- Asp.net Core.

&nbsp;

## Features Of App
- ✨ Use for selling and buying and record all opertion of a shop.
- ✨ Has a poweful Authorization & Authentication.
- ✨ Has inventory & record expenses and extra expenses and installments.

&nbsp;

> Recording data from the purchase and sale invoice and the invoices can be retrieved, and you can use the installment system and record warehouse outputs and their movement.

&nbsp;

## Tech

Asara uses a number of open source projects and frameworks to work properly:

- [AngularJS](https://angularjs.org/) - HTML & CSS enhanced for web apps!
- [VS Code Editor](https://code.visualstudio.com/) - awesome web-based text editor.
- [Twitter Bootstrap](https://getbootstrap.com/) - great UI boilerplate for modern web apps.
- [Npm](https://www.npmjs.com/) - install modules.
- [Dot Net core FrameWork](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0) - In Back-End.
- [MSSQL](https://www.mssql.com/) - DataBase Management System.
- [MSMS](https://www.mysql.com/products/workbench/) - Data Base Management tool.
- [Post Man](https://www.postman.com/) - For testing APIs in app.
- [Git](https://git-scm.com/).

&nbsp;

## Installation

Asara requires [npm](https://www.npmjs.com) 6+ to run.

Install the dependencies and devDependencies and start the Angular.


```sh
cd task-app
npm install #for install angular module
ng serve ## http:localhost:4200\item
##### run Asara.API web Restful Sevice
cd ../
dotnet restore   ####  for restore library files
dotnet ef database drop ### if you want remove data 
dotnet ef database migrations add m12m
dotnet watch run ## the data will seed automaticaly

### http:localhost:5000/login
```

For production environments:

```sh
ng build --prod ----optimization=fasle  
####for build default path ../Asara.Api/wwwroot you can change from angular.js
cd ../Asara.Api
dotnet deploy ### you can deploy it in IIs for windows & Apache for Linux
```
```

#### Link for assum you run correctly
```sh
127.0.0.1:5001
```

&nbsp;
