# Quick and dirty extension for basic .NET web api (in development)

## Configurations
### Swagger AUTH 
If you require Swagger UI Authentication include following in program.cs:
```
builder.Services.ConfigureSwaggerWithAuth(configuration, auth = true);
```
Above configures and setups authentication, use below to automatically fulfill form on user action:
```
app.ConfigureSwaggerUIWithAuth(configuration, auth = true);
```
Default auth is set to true, which means that it takes care of basic azure authorization configurations, otherwise at false you have to set it up yourself.
To make above defaults work you are required to have below settings in appsettings.json:
```
"AzureAd": {  
    "Instance": "https://login.microsoftonline.com",  
    "Domain": "xxx.onmicrosoft.com",  
    "TenantId": "{tenantId}}",
    "ClientId": "{clientId}",
    "Scopes": "api://{clientId}, user.read, profile, email, offline_access, openid",
    "CallbackPath": "/signin-oidc",
    "Audience": "clientId",
    "AudienceIDZ": "api://{clientId}",
    "Swagger": {
        "Api": {
            "Name": "My API",
            "Version": "v1"
        },
        "Scopes-optional": "api://{clientId}, user.read, profile, offline_access",
        "Scopes-selected": "api://{clientId}, user.read, profile, openid"
    }
}
```
### ERROR handling
For custom error handling include:
```
app.ConfigureExceptionMiddlewareExtension();
```
You can now extend the base class `ApiException` or `BusinessException/ValidationException` to use the custom error handler.

## Web API layer extendables
### Repository layer
To use the extendable repository layer, you have to create your own `BaseContext` that extends `DbBaseContext` from the library (`DbBaseContext` extends `DbContext` and has all the required base functionalities):
```
builder.Services.AddDbContext<DbBaseContext, BaseContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:ConnectionString"])
```
You can now extend `EntityCoreRepository` where you can find a lot of basic features ready to be used in service layer.

### Service layer
To use the extendable service layer, you have to extend `BusinessCoreService` and there will be basic features ready to be used in controller layer.

NOTE(service layers needs `IMapper`, `IValidator` and `IBusinessStrategy`) :
- `IMapper` - used for mapping entites and dto objects
- `IValidator` - can be used as validation for input data where u need it, inject `DefaultValidator` if you do not need it otherwise implement it in your own class
- `IBusinessStrategy` - basic service features do not include any logic besides retrieving, adding, filtering, pagination, updating, deletion... for additional rules in each of the listed features implement it in your own class otherwise inject `DefaultBusinessStrategy`

### Controller layer
Only includes a barebones `ApiControllerBase` and `LazyCoreController` that includes some of the functionalities from service layer.

## Utilities
## Web API layer extendables
Additional small utilities:
- `HtmlFormatter` - create HTML formats in a building block style
- `HttpConnector` - extendable DI for calling HTML documents
- `MailSystem` - extendable DI for sending email, requires appsettings.json:
```
"Mail-System": {
    "Smtp-Client": {
        "Host": "smtp.gmail.com",
        "Port": 587,
        "Credentials": {
            "E-Mail": "xxx@gmail.com",
            "Password": "xxx"
        }
   }
}
```
- `Validator` - if you are using the error handling from before, this can be used with `ValidationExcetpion` and `NumberValidator, ObjectValidator, StringValidator` to build validations for inputs