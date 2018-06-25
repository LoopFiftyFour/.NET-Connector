# .NET-Connector
.NET Wrapper for Loop54 JSON V3 API

## How to install
The Loop54 Connector is available for download as a NuGet package. For installation instructions, see https://www.nuget.org/packages/Loop54.Connector/.

Requires .NET Framework 4.5 or later alternatively .NET Standard 2.0 or later.

## How to use
The Loop54 Connector is easily configured if running ASP.NET or ASP.NET Core.

### ASP.NET Core
Simply add the following code to the service configuration in your startup.cs file:

    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddLoop54("https://helloworld.54proxy.com");
    // or services.AddLoop54(new Loop54Settings("https://helloworld.54proxy.com")); for more options

An `ILoop54Client` is then injectable to your middleware or controllers. See [official ms docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1) for more information about how that works.

### ASP.NET
Call the following static method once in the beginning of you application life cycle. For example in the `Application_Start` method in the Global.asax.cs file:
    
    Loop54ClientManager.StartUp("https://helloworld.54proxy.com");
    // or Loop54ClientManager.StartUp(new Loop54Settings("https://helloworld.54proxy.com")); for more options
    
The `ILoop54Client` is then attainable with the following method:

    ILoop54Client client = Loop54ClientManager.Client();
    
This `ILoop54Client` could of course be handled by a dependency injection system. Simply register the instance as a singleton in the framework of your choice. 

### Using the `ILoop54Client`
The `ILoop54Client` contains methods for making all public API calls to the Loop54 e-commerce search engine. It contains both synchronous and asynchronous variants of all methods.

    string searchQuery = "banana";
    SearchRequest request = new SearchRequest(searchQuery);
    SearchResponse response = await _loop54Client.SearchAsync(request);
    
To add parameters to the `SearchRequest` you could do this:

    request.ResultsOptions.Skip = 0;
    request.ResultsOptions.Take = 10;
    request.ResultsOptions.AddDistinctFacet<string>("Manufacturer");
    request.ResultsOptions.AddRangeFacet<double>("Price");

All other request types also have their own types to use:

    var createEventRequest = new CreateEventsRequest(event);
    var autoCompleteRequest = new AutoCompleteRequest(query);
    var getRelatedEntitiesRequest = new GetRelatedEntitiesRequest(entityType, entityId);
    var getEntitiesRequest = new GetEntitiesRequest();
    var getEntitiesByAttributeRequest = new GetEntitiesByAttributeRequest(attributeName, attributeValue);

### But wait! I'm not using ASP.NET Core or ASP.NET!
There is still hope for you. If not using the above mentioned frameworks you can implement some of the functionality yourself and use the client just as well. You will need to implement the `IRemoteClientInfoProvider` interface and the `IRemoteClientInfo` interface and after doing that you can create a new instance of `ILoop54Client` like this:

    IRemoteClientInfoProvider myRemoteClientInfoProvider = new MySuperAwesomeCustomRemoteClientInfoProvider();
    Loop54Settings settings = new Loop54Settings("https://helloworld.54proxy.com");
    ILoop54Client loop54Client = new Loop54Client(new RequestManager(settings), myRemoteClientInfoProvider);
    
And you are good to go!

## Features
- Native support for search, autoComplete, createEvent, getEntities, getEntitiesByAttribute and getRelatedEntities call. With intuitive APIs to call them.
- Handles user identification using random-generated cookies.
- Uses X-Forwarded-For as client IP if it's available.
- Configurable HTTP timeout.
- GZIP support.
- Relays HTTP data to engine:
    - Referer
    - UserAgent
    - Library version
