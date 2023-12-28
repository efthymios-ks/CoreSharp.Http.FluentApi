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

## Usage 
Include `using CoreSharp.Http.FluentApi.Extensions;` 

## Examples 

### Responses
```CSharp 
    var response = await httpClient
		.Request()
		.WithEndpoint("albums", 1)
		.Get()
		.SendAsync();
``` 
```CSharp 
    var responseAsString = await httpClient
		.Request()
		.WithEndpoint("albums", 1)
		.Get()
		.ToString()
		.SendAsync();
``` 
```CSharp 
   using var responseAsStream = await httpClient
		.Request()
		.WithEndpoint("albums", 1)
		.Get()
		.ToStream()
		.SendAsync();
```
```CSharp 
    var responseAsBytes = await httpClient
		.Request()
		.WithEndpoint("albums", 1)
		.Get()
		.ToBytes()
		.SendAsync();
```
```CSharp 
    var albums = await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get() 
		.WithJsonDeserialize<IEnumerable<Album>()
		.SendAsync();
```
```CSharp 
    var albums = await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get() 
		.WithXmlDeserialize<IEnumerable<Album>()
		.SendAsync();
``` 

### HttpCompletionOption
```CSharp 
    await httpClient
		.Request()
		.WithCompletionOption(HttpCompletionOption.ResponseContentRead)
		.WithEndpoint("albums")
		.Get() 
		.SendAsync();
```

### Timeout
```CSharp 
    try
    {
        await httpClient
			.Request()
			.WithTimeout(TimeSpan.FromSeconds(15))
			.WithEndpoint("albums")
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
		await httpClient
			.Request() 
			.WithEndpoint("wrong/url")
			.Get()
			.SendAsync();
    }
    catch (HttpOperationException ex)
    {   
		  var method = ex.RequestMethod;
		  var url = ex.RequestUrl;
		  var statusCode = ex.ResponseStatusCode;
		  var logEntry = ex.LogEntry;
		  var content = ex.ResponseContent;
		  var summary = ex.ToString();
    }
``` 
```CSharp 
    await httpClient
		.Request()
		// Won't throw on failed status or http request exception.
		.IgnoreError()
		.WithEndpoint("wrong/url")
		.Get()
		.SendAsync();
```

### Headers
```CSharp 
    await httpClient
		.Request()
		// Cache-Control > max-age=604800 
		.WithHeader(HeaderNames.CacheControl, "max-age=604800")
		.WithEndpoint("albums")
		.Get()
		.SendAsync();
``` 
```CSharp 
    await httpClient
		.Request()
		// Accept > application/json 
		.Accept(MediaTypeNames.Application.Json)
		.WithEndpoint("albums")
		.Get()
		.SendAsync();
```
```CSharp 
    await httpClient
		.Request()
		// Accept > application/json 
		.AcceptJson()
		.WithEndpoint("albums")
		.Get()
		.SendAsync();
```
```CSharp 
    await httpClient
		.Request()
		// Authorization > {apiKey}
		.WithAuthorization("{apiKey}")
		.WithEndpoint("albums")
		.Get()
		.SendAsync();
```

```CSharp 
    await httpClient
		.Request()
		// Authorization > Bearer {accessToken}
		.WithBearerToken("{accessToken}")
		.WithEndpoint("albums")
		.Get()
		.SendAsync();
```

### Request with key 
```CSharp 
    await httpClient
		.Request()
		// /albums/1 
		.WithEndpoint("albums/1")
		.Get()
		.SendAsync();
```
```CSharp 
    var id = 1;
    await httpClient
	.Request()
                    // /albums/1 
                    .WithEndpoint("albums", id)
                    .Get()
                    .SendAsync();
```
```CSharp 
    var id = 1;
    await httpClient
		.Request()
		// /albums/1 
		.WithEndpoint($"albums/{id}")
		.Get()
		.SendAsync();
```

### Query parameters
```CSharp 
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		// albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
		.WithQuery("Id", 1).WithQuery("Color", "Black").WithQuery("CreationDate", DateTime.Now)
		.SendAsync();
```
```CSharp 
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		// albums/?Id=1&Color=Black&CreationDate=2021-11-27T11%3A41%3A06 
		.WithQuery(new { Id = 1, Color = "Black", CreationDate = DateTime.Now })
		.SendAsync();
``` 
```CSharp 
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		// albums/?Id=1&UserId=1&Title=My%20Title 
		.WithQuery(new Album { Id = 1, UserId = 1, Title = "My Title" })
		.SendAsync();
```
 
### Cache response (GET, HEAD, OPTIONS, TRACE methods only) 
```CSharp
    await httpClient
		.Request()
		.WithEndpoint("albums", 1)
		.Get()
		.ToString()
		.WithCache(TimeSpan.FromMinutes(15))
		.SendAsync();
```
```CSharp
    var shouldForceNew = GetShouldForceNew();
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		.ToString()
		.WithCache(TimeSpan.FromMinutes(15))
		// Variable 
		.WithCacheInvalidation(() => shouldForceNew)
		.SendAsync();
```
```CSharp
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		.ToString()
		.WithCache(TimeSpan.FromMinutes(15))
		// Func<bool>
		.WithCacheInvalidation(() => ShouldInvalidateCache())
		.SendAsync();
					
    static bool ShouldInvalidateCache()
        => ...;
```
```CSharp
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Get()
		.ToString()
		.WithCache(TimeSpan.FromMinutes(15))
		// Task<Func<bool>>
		.ForceNew(() => ShouldInvalidateCacheAsync())
		.SendAsync();
					
    static Task<bool> ShouldInvalidateCacheAsync()
        => ...;
```

### Content (POST, PUT, PATCH) 
```CSharp
    HttpContent content = GetHttpContent(...);
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Post()
		.WithContent(content)
		.SendAsync();
```
```CSharp
    var dynamicContent = new { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Post()
		.WithJsonContent(dynamicContent)
		.SendAsync();
```
```CSharp
    var album = new Album { Id = 1, UserId = 1, Title = "My Title" };
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Post()
		.WithJsonContent(album)
		.SendAsync();
```
```CSharp
    var stringContent = @"{ ""Id"" = 1, ""UserId"" = 1, ""Title"" = ""My Title"" }";
    await httpClient
		.Request()
		.WithEndpoint("albums")
		.Post()
		.WithJsonContent(stringContent)
		.SendAsync();
```
