using CoreSharp.Extensions;
using CoreSharp.HttpClient.FluentApi.Domain.Models;
using CoreSharp.HttpClient.FluentApi.Extensions;
using CoreSharp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreSharp.HttpClient.FluentApi.Examples
{
    internal static class Program
    {
        //Methods 
        private static async Task Main()
        {
            //Services
            var services = Startup.ConfigureServices();

            try
            {
                //"Inject" IHttpClientFactory
                var factory = services.GetService<IHttpClientFactory>();

                //Create default HttpClient 
                var client = factory.CreateClient("Default");

                //GET /albums and map to IEnumerable 
                var albums = await client
                    .Request()
                    .CompletionOption(HttpCompletionOption.ResponseContentRead)
                    .Route("albums")
                    .Get()
                    .Json<IEnumerable<Album>>()
                    .SendAsync();

                //GET /posts and map to array and cache 
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

                //GET /users/2 and map to class 
                var user = await client
                    .Request()
                    .Route("users", 2)
                    .Get()
                    .Json<User>()
                    .SendAsync();

                //PATCH /users/2 and get HttpResponseMessage 
                user.Name = "Efthymios";
                using var response = await client
                    .Request()
                    .Route("users", user.Id)
                    .Patch()
                    .Content(user)
                    .SendAsync();
                var success = response.IsSuccessStatusCode;
                var json = await response.Content.ReadAsStringAsync();

                //Throw on failed request 
                await client
                    .Request()
                    .Route("wrong/url")
                    .Get()
                    .SendAsync();
            }
            //Http request specific exception 
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
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
