# Goiar simple Cqrs

## How to use it

### Simple implemantation with log event handler

First you have to set up your application, so in the `startup.cs` add

``` csharp
public class Startup 
{
    //...
    public void ConfigureServices(IServiceCollection services)
    {
        //...
        services.AddCommandHandlersFromAssemblyOf(typeof(ACommandHandlerPivotClass));
        
        services.AddQueryHandlersFromAssemblyOf(typeof(AQueryHandlerPivotClass));
        //...
    }
    //...
}
```

This two lines will search for every implementation of `IQueryHandler<>`, `ICommandHandler<>` and `ICommandHandler<,>` on the assembly where the given class is Created. You can use these two lines as many times as you want.