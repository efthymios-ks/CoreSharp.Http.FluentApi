# CoreSharp.HttpClient.FluentApi 

## Description 
`CoreSharp.HttpClient.FluentApi` allows fluent http request building.  

## Features 
- Automatic json conversions using `Json.NET`. 
- Optional in-memory response caching for GET requests. 
- Specific and more meaningful exceptions. 
- Use of `Stream` where applicable instead of eager converting entities to string. **[Optimizes memory consumption]** 
- Use of `HttpCompletionOption.ResponseHeadersRead` by default to all requests. **[Optimizes memory consumption and response times]** 
- Easily expandable. 

## Installation 
Install via [nuget](https://www.nuget.org/packages/CoreSharp.HttpClient.FluentApi/).

## Interface tree 
Building the request involves several configurable steps.  
From initiating the request builder the request to sending the request. 
```
HttpClient
└── IRequest (Headers, ThrowOnError)
    └── IRoute (Route) 
        └── IMethod (GET, POST, PUT, PATCH, DELETE) 
            |   └── GET 
            |   |   └── IQueryMethod (QueryParameter) 
            |   └── POST, PUT, PATCH  
            |       └── IContentResponse (Content) 
            └── IGenericResponse (Optional) 
                └── IJsonResponse (Cache) 
```

## Usage 
```
using CoreSharp.HttpClient.FluentApi.Extensions;

public class IndexPage 
{ 
  //private HttpClient _httpClient = ...;
  
  //Methods
  protected override async Task OnInitializedAsync() 
  {
    await base.OnInitializedAsync(); 

    try
    { 
        //GET /albums and map to IEnumerable 
        var albums = await client
            .Request()
            .Route("albums")
            .Get()
            .Json<IEnumerable<Album>>()
            .SendAsync();

        //GET /posts and map to array with caching
        for (var i = 0; i < 3; i++)
        {
            await client
                .Request()
                .Route("posts")
                .Get()
                .Json<Post[]>()
                .Cache(TimeSpan.FromMinutes(5))
                .SendAsync();
        }

        //GET /users and map to HashSet 
        var users = await client
            .Request()
            .Route("users")
            .Get()
            .Json<HashSet<User>>()
            .SendAsync();

        //GET /users/2 and map to class 
        var user = await client
            .Request()
            .Route("users/2")
            .Get()
            .Json<User>()
            .SendAsync();

        //PATCH /users/2 and get HttpResponseMessage 
        user.Name = "Efthymios";
        using var response = await client
            .Request()
            .Route($"users/{user.Id}")
            .Patch()
            .Content(user)
            .SendAsync();
        var success = response.IsSuccessStatusCode;
        var json = await response.Content.ReadAsStringAsync();

        //Throw on failed request 
        await client
            .Request()
            .ThrowOnError()
            .Route("wrong/url")
            .Get()
            .SendAsync();
    }
    //IRestClient specific exception 
    catch (HttpResponseException ex)
    {
        var statusCode = ex.ResponseStatusCode;
        var method = ex.RequestMethod;
        var url = ex.RequestUrl;
        var status = ex.ResponseStatus;
        var content = ex.ResponseContent;
        var summary = ex.ToString();
        Console.WriteLine(summary);
    }
    //Other exceptions 
    catch (Exception ex)
    {
    }
  }
}
```
