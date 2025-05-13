## Overview 

this is the API 
it's a RESTful API with layered archticture 


Controller → Service → Repo → DB
Controller ← DTO ← Service ← Entity ← Repo


it has 4 Different Models 

## Installation 

1. create a `appsettings.json` file and paste this stuff : 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultConnection": "yourconnectionstring"
    }, 
    "JWT" : { 
      "Issuer" : "yourissuer" ,
      "Audience" : "youraudience" ,
      "Key" : "yoursecretkey"
    }
}

```

> don't forget to replace `"yourconnectionstring"` with an actual connection string , and the other JWT issuer,audience,key values with your own 

2. run `dontnet restore` to install all dependancies 

3. make sure to make migration : `dotnet ef migrations add MigrationName`

4. then update the database : `dotnet ef database update`

that's basically it
now you can run the API `dotnet run` and access endpoints via swagger 