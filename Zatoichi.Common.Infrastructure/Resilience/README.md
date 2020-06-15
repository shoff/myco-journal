## .Resilience
This library contains objects that can be used to wrap database, api and service bus calls in pre-defined [Polly](https://github.com/App-vNext/Polly)
policies.

### Registering Policies

In an ASPNET/Services project, to add support for policies to your solution, in the `Startup` class for your application
locate the `public void ConfigureServices(IServiceCollection services)` method.

To use the built in values for the three policy types _(listed below)_ simply add the following line in the `ConfigureServices` 
method:

```csharp
    services.AddResilience();   
```

You can also define custom policy values by including the following configuration section in the appropriate appsettings.json 
file, or configuration server section: 

_(note the values listed in the example below are the defaults used if none are provided)_

```javascript
    "PolicyOptions": {

        // database policy values
        "DbCircuitBreakerErrorCount": "3",
        "DbCircuitBreakerDelay": "1000",
        "DbRetryCount": "3",
        "DbRetryBaseDelay": "1000",

        // http service call policy
        "ApiCircuitBreakerErrorCount": "3",
        "ApiCircuitBreakerDelay": "1000",
        "ApiRetryCount": "3",
        "ApiRetryBaseDelay": "1000",

        // service bus publish / subscribe policy
        "QueueCircuitBreakerErrorCount": "3",
        "QueueCircuitBreakerDelay": "1000",
        "QueueRetryCount": "3",
        "QueueRetryBaseDelay": "1000"

        // IO policy
        "IOCircuitBreakerErrorCount": "3",
        "IOCircuitBreakerDelay": "1000",
        "IORetryCount": "3",
        "IORetryBaseDelay": "1000",
  }
```

If you decide to define your own values then you will need to ensure that you have added a nuget reference to:

* Microsoft.Extensions.Options

Register your options as follows:

```csharp
    services.AddResilience(Configuration.GetSection("PolicyOptions"))
```




