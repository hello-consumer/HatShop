﻿# HatShop

## Overview

The HatShop Application is built using ASP.Net Core 2.1 and uses libraries like Bootstrap 4.1.3 and EntityFramework Core 2.  It is an E-commerce Application designed to demonstrate ASP.Net Core fundamental concepts.

## Getting Started
- Clone the application from GitHub.
- Download and Install Visual Studio Community and Configure for the DotNetCore Development workspace.
- Make sure to download the .NET Core 2.1 SDK or runtime libraries
- Double Click the .sln file to open the project
- Hit "Play" to run

## Connection String Initialization

The ABCReport controller is configured to communicate with a database with the ABC Manufacturing Database Schema.  The connection string must be set prior to using the report.  From the command line:

`dotnet user-secrets set "ConnectionStrings:ABCConnection" "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ABC;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"`

💯 

## Data Initialization

###
`dotnet ef database update`
This will deploy the latest schema to the DefaultConnection connecting string

### Adding Roles
`dotnet HatShop.dll createroles`
This will automatically create Administrator, ProductManager and MarketingManager in the database

### Add user to Roles
`dotnet HatShop.dll assignusertorole [USERNAME] [ROLENAME]`
Adds a registered user to the role

### Adding Mock Data
`dotnet HatShop.dll addproducts`
This will add sample products, categories, and reviews