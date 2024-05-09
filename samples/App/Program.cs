using App;
using CoreSharp.Http.FluentApi.Exceptions;
using CoreSharp.Http.FluentApi.Extensions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

// Methods 
var services = Startup.ConfigureServices();

try
{
    // "Inject" IHttpClientFactory
    var factory = services.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;

    // Create your HttpClient 
    using var client = factory.CreateClient("Default");

    // GET /albums and map to IEnumerable 
    var albums = await client
        .Request()
        .WithEndpoint("albums")
        .Get()
        .WithJsonDeserialize<IEnumerable<Album>>()
        .SendAsync();

    // GET /albums to string 
    var albumsJson = await client
        .Request()
        .WithEndpoint("albums")
        .Get()
        .ToString()
        .SendAsync();

    // GET /albums to byte[] 
    var albumsBytes = await client
        .Request()
        .WithEndpoint("albums")
        .Get()
        .ToBytes()
        .SendAsync();

    // GET /posts, map to array and cache  
    for (var i = 0; i < 5; i++)
    {
        var posts = await client
            .Request()
            .WithEndpoint("posts")
            .Get()
            .WithJsonDeserialize<Post[]>()
            .WithCache(TimeSpan.FromMinutes(5))
            .WithCacheInvalidation(() => i % 2 == 0)
            .SendAsync();
    }

    // GET /users/2 and map to class from json
    var user = await client
        .Request()
        .WithEndpoint("users", 2)
        .Get()
        .WithJsonDeserialize<User>()
        .SendAsync();

    // PATCH /users/2 and get HttpResponseMessage 
    user.Name = "Efthymios";
    using var response = await client
        .Request()
        .WithEndpoint("users", user.Id)
        .Patch()
        .WithJsonBody(user)
        .SendAsync();
    var success = response.IsSuccessStatusCode;
    var json = await response.Content.ReadAsStringAsync();

    // Throw on failed request 
    await client
        .Request()
        .WithEndpoint("wrong/url")
        .Get()
        .SendAsync();
}
// Timeout
catch (TimeoutException ex)
{
    Console.WriteLine(ex);
}
// Http request specific exception 
catch (HttpOperationException ex)
{
    var method = ex.RequestMethod;
    var url = ex.RequestUrl;
    var statusCode = ex.ResponseStatusCode;
    var logEntry = ex.LogEntry;
    var content = ex.ResponseContent;
    var summary = ex.ToString();
    Console.WriteLine(summary);
}
// Other exceptions 
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.ReadLine();
