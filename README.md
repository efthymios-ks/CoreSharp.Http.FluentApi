# CoreSharp.Http.FluentApi 

[![Nuget](https://img.shields.io/nuget/v/CoreSharp.Http.FluentApi)](https://www.nuget.org/packages/CoreSharp.Http.FluentApi/)
[![Nuget](https://img.shields.io/nuget/dt/CoreSharp.Http.FluentApi)](https://www.nuget.org/packages/CoreSharp.Http.FluentApi/)

## Description 
`CoreSharp.Http.FluentApi` allows fluent http request building.  

## Features 
- Automatic json conversions using `Text.Json` or `Json.NET`. 
- Optional in-memory response caching for GET requests. 
- Specific and more meaningful exceptions (`HttpResponseException`, `TimeoutException`). 
- Use of `Stream` where applicable instead of eager converting entities to string. **[Optimizes memory consumption]** 
- Use of `HttpCompletionOption.ResponseHeadersRead` by default to all requests. **[Optimizes memory consumption and response times]** 
- Easily expandable. 

## Interface tree 
Building the request involves several configurable steps.  
From initiating the request builder the request to sending the request. 
```
HttpClient
└── IRequest (Headers, ThrowOnError, Timeout) 
    └── IRoute (Route) 
        └── IMethod (GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, TRACE) 
            |   └── GET 
            |   |   └── IQueryMethod (QueryParameters) 
            |   └── POST, PUT, PATCH 
            |       └── IContentResponse (Content) 
            └── IGenericResponse (Cache) 
                └── IJsonResponse 
                └── IXmlResponse 
```

## Usage 
Include `using CoreSharp.Http.FluentApi.Extensions;` 

## Examples 

### Responses
```CSharp 
    var response = await httpClient.Request()
                                   .Route("albums", 1)
                                   .Get()
                                   .SendAsync();
``` 
```CSharp 
    var responseAsString = await httpClient.Request()
                                           .Route("albums", 1)
                                           .Get()
                                           .String()
                                           .SendAsync();
``` 
```CSharp 
   using var responseAsStream = await httpClient.Request()
                                                .Route("albums", 1)
                                                .Get()
                                                .Stream()
                                                .SendAsync();
```
```CSharp 
    var responseAsBytes = await httpClient.Request()
                                          .Route("albums", 1)
                                          .Get()
                                          .Bytes()
                                          .SendAsync();
```
```CSharp 
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Check Content-Type response header 
                                 .To<IEnumerable<Album>()
                                 .SendAsync();
```
```CSharp 
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Forced json deserialization 
                                 .Json<IEnumerable<Album>()
                                 .SendAsync();
```
```CSharp 
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Forced xml deserialization 
                                 .Xml<IEnumerable<Album>()
                                 .SendAsync();
``` 

### HttpCompletionOption
```CSharp 
    await httpClient.Request()
                    .CompletionOption(HttpCompletionOption.ResponseContentRead)
                    .Route("albums")
                    .Get() 
                    .SendAsync();
```

### Timeout
```CSharp 
    try
    {
        await httpClient.Request()
                        .Timeout(TimeSpan.FromSeconds(15))
                        .Route("albums")
                        .Get() 
                        .SendAsync();
    }
    catch (TimeoutException ex)
    {
    }
```

### Error handling
```CSharp 
    try
    {
        // Throws by default 
        await httpClient.Request() 
                        .Route("wrong/url")
                        .Get()
                        .SendAsync();
    }
    catch (HttpResponseException ex)
    {   
        var method = ex.RequestMethod;
        var url = ex.RequestUrl;
        var status = ex.ResponseStatus;
        var content = ex.ResponseContent;
        var summary = ex.ToString(); 
    }
``` 
```CSharp 
    await httpClient.Request()
                    .IgnoreError()
                    .Route("wrong/url")
                    .Get()
                    .SendAsync();
```

### Add header 
```CSharp 
    await httpClient.Request()
                    // Cache-Control > max-age=604800 
                    .Header(HeaderNames.CacheControl, "max-age=604800")
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Add header ACCEPT 
```CSharp 
    await httpClient.Request()
                    // Accept > application/json 
                    .Accept(MediaTypeNames.Application.Json)
                    .Route("albums")
                    .Get()
                    .SendAsync();
```
```CSharp 
    await httpClient.Request()
                    // Accept > application/json 
                    .AcceptJson()
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Add header AUTHORIZATION 
```CSharp 
    await httpClient.Request()
                    // Authorization > Bearer accessTokenValue 
                    .Bearer("accessTokenValue")
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Request with key 
```CSharp 
    await httpClient.Request()
                    // /albums/1 
                    .Route("albums/1")
                    .Get()
                    .SendAsync();
```
```CSharp 
    var id = 1;
    await httpClient.Request()
                    // /albums/1 
                    .Route("albums", id)
                    .Get()
                    .SendAsync();
```
```CSharp 
    var id = 1;
    await httpClient.Request()
                    // /albums/1 
                    .Route($"albums/{id}")
                    .Get()
                    .SendAsync();
```

### Query parameter (GET)
```CSharp 
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
                    .Query("Id", 1).Query("Color", "Black").Query("CreationDate", DateTime.Now)
                    .SendAsync();
```
```CSharp 
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
                    .Query(new { Id = 1, Color = "Black", CreationDate = DateTime.Now })
                    .SendAsync();
``` 
```CSharp 
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&UserId=1&Title=My%20Title 
                    .Query(new Album { Id = 1, UserId = 1, Title = "My Title" })
                    .SendAsync();
```
 
### Cache response (GET generic requests) 
```CSharp
    await httpClient.Request()
                    .Route("albums", 1)
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    .SendAsync();
```
```CSharp
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .Json<IEnumerable<Album>>()
                    .Cache(TimeSpan.FromMinutes(15))
                    .SendAsync();
```

### Clear cached response 
```CSharp
    var shouldForceNew = GetShouldForceNew();
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    // Variable 
                    .ForceNew(shouldForceNew)
                    .SendAsync();
					
    static bool GetShouldForceNew()
        => ...;
```
```CSharp
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    // Func<bool>
                    .ForceNew(() => GetShouldForceNew())
                    .SendAsync();
					
    static bool GetShouldForceNew()
        => ...;
```
```CSharp
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    // Func<bool>
                    .ForceNew(GetShouldForceNew)
                    .SendAsync();
					
    static bool GetShouldForceNew()
        => ...;
```
```CSharp
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    // Task<Func<bool>>
                    .ForceNew(() => GetShouldForceNewAsync())
                    .SendAsync();
					
    static async Task<bool> GetShouldForceNewAsync()
        => ...;
```

### Content (POST, PUT, PATCH) 
```CSharp
    HttpContent content = GetHttpContent(...);
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .Content(content)
                    .SendAsync();
```
```CSharp
    var dynamicContent = new { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(dynamicContent)
                    .SendAsync();
```
```CSharp
    var album = new Album { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(album)
                    .SendAsync();
```
```CSharp
    var stringContent = @"{ ""Id"" = 1, ""UserId"" = 1, ""Title"" = ""My Title"" }";
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(stringContent)
                    .SendAsync();
```
