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
```
    var response = await httpClient.Request()
                                   .Route("albums", 1)
                                   .Get()
                                   .SendAsync();
``` 
```
    var responseAsString = await httpClient.Request()
                                           .Route("albums", 1)
                                           .Get()
                                           .String()
                                           .SendAsync();
``` 
```
   using var responseAsStream = await httpClient.Request()
                                                .Route("albums", 1)
                                                .Get()
                                                .Stream()
                                                .SendAsync();
```
```
    var responseAsBytes = await httpClient.Request()
                                          .Route("albums", 1)
                                          .Get()
                                          .Bytes()
                                          .SendAsync();
```
```
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Check Content-Type response header 
                                 .To<IEnumerable<Album>()
                                 .SendAsync();
```
```
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Forced json deserialization 
                                 .Json<IEnumerable<Album>()
                                 .SendAsync();
```
```
    var albums = await httpClient.Request()
                                 .Route("albums")
                                 .Get()
                                 // Forced xml deserialization 
                                 .Xml<IEnumerable<Album>()
                                 .SendAsync();
``` 

### HttpCompletionOption
```
    await httpClient.Request()
                    .CompletionOption(HttpCompletionOption.ResponseContentRead)
                    .Route("albums")
                    .Get() 
                    .SendAsync();
```

### Timeout
```
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
```
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
```
    await httpClient.Request()
                    .IgnoreError()
                    .Route("wrong/url")
                    .Get()
                    .SendAsync();
```

### Add header 
```
    await httpClient.Request()
                    // Cache-Control > max-age=604800 
                    .Header(HeaderNames.CacheControl, "max-age=604800")
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Add header ACCEPT 
```
    await httpClient.Request()
                    // Accept > application/json 
                    .Accept(MediaTypeNames.Application.Json)
                    .Route("albums")
                    .Get()
                    .SendAsync();
```
```
    await httpClient.Request()
                    // Accept > application/json 
                    .AcceptJson()
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Add header AUTHORIZATION 
```
    await httpClient.Request()
                    // Authorization > Bearer accessTokenValue 
                    .Bearer("accessTokenValue")
                    .Route("albums")
                    .Get()
                    .SendAsync();
```

### Request with key 
```
    await httpClient.Request()
                    // /albums/1 
                    .Route("albums/1")
                    .Get()
                    .SendAsync();
```
```
    var id = 1;
    await httpClient.Request()
                    // /albums/1 
                    .Route("albums", id)
                    .Get()
                    .SendAsync();
```
``` 
    var id = 1;
    await httpClient.Request()
                    // /albums/1 
                    .Route($"albums/{id}")
                    .Get()
                    .SendAsync();
```

### Query parameter (GET)
```
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
                    .Query("Id", 1).Query("Color", "Black").Query("CreationDate", DateTime.Now)
                    .SendAsync();
```
```
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
                    .Query(new { Id = 1, Color = "Black", CreationDate = DateTime.Now })
                    .SendAsync();
``` 
```
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    // albums/?Id=1&UserId=1&Title=My%20Title 
                    .Query(new Album { Id = 1, UserId = 1, Title = "My Title" })
                    .SendAsync();
```
 
### Cache response (GET generic requests) 
```
    await httpClient.Request()
                    .Route("albums", 1)
                    .Get()
                    .To<Album>()
                    .Cache(TimeSpan.FromMinutes(15))
                    .SendAsync();
```
```
    await httpClient.Request()
                    .Route("albums")
                    .Get()
                    .Json<IEnumerable<Album>>()
                    .Cache(TimeSpan.FromMinutes(15))
                    .SendAsync();
```

### Content (POST, PUT, PATCH) 
```
    HttpContent content = GetHttpContent(...);
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .Content(content)
                    .SendAsync();
```
```
    var dynamicContent = new { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(dynamicContent)
                    .SendAsync();
```
```
    var album = new Album { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(album)
                    .SendAsync();
```
```
    var stringContent = @"{ ""Id"" = 1, ""UserId"" = 1, ""Title"" = ""My Title"" }";
    await httpClient.Request()
                    .Route("albums")
                    .Post()
                    .JsonContent(stringContent)
                    .SendAsync();
```
