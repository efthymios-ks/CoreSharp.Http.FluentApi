using CoreSharp.Http.FluentApi.Exceptions;
using CoreSharp.Http.FluentApi.Extensions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace App;

internal static class Program
{
    //Methods 
    [SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Pending>")]
    private static async Task Main()
    {
        //Services
        var services = Startup.ConfigureServices();

        try
        {
            //"Inject" IHttpClientFactory
            var factory = services.GetService(typeof(IHttpClientFactory)) as IHttpClientFactory;

            //Create your HttpClient 
            var client = factory.CreateClient("Default");

            //GET /albums and map to IEnumerable 
            var albums = await client.Request()
                                     .Route("albums")
                                     .Get()
                                     .Json<IEnumerable<Album>>()
                                     .SendAsync();

            //GET /albums to string 
            var albumsJson = await client.Request()
                                         .Route("albums")
                                         .Get()
                                         .String()
                                         .SendAsync();

            //GET /albums to byte[] 
            var albumsBytes = await client.Request()
                                          .Route("albums")
                                          .Get()
                                          .Bytes()
                                          .SendAsync();

            //GET /posts, map to array and cache 
            for (var i = 0; i < 3; i++)
            {
                var posts = await client.Request()
                                        .Route("posts")
                                        .Get()
                                        .Json<Post[]>()
                                        .Cache(TimeSpan.FromMinutes(5))
                                        .SendAsync();
            }

            //GET /users/2 and map to class automatically based on Content-Type header
            var user = await client.Request()
                                   .Route("users", 2)
                                   .Get()
                                   .To<User>()
                                   .SendAsync();

            //PATCH /users/2 and get HttpResponseMessage 
            user.Name = "Efthymios";
            using var response = await client.Request()
                                             .Route("users", user.Id)
                                             .Patch()
                                             .JsonContent(user)
                                             .SendAsync();
            var success = response.IsSuccessStatusCode;
            var json = await response.Content.ReadAsStringAsync();

            //Throw on failed request 
            await client.Request()
                        .Route("wrong/url")
                        .Get()
                        .SendAsync();
        }
        //Timeout
        catch (TimeoutException ex)
        {
            Console.WriteLine(ex);
        }
        //Http request specific exception 
        catch (HttpResponseException ex)
        {
            var method = ex.RequestMethod;
            var url = ex.RequestUrl;
            var statusCode = ex.ResponseStatusCode;
            var status = ex.ResponseStatus;
            var content = ex.ResponseContent;
            var summary = ex.ToString();
            Console.WriteLine(summary);
        }
        //Other exceptions 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }
}
