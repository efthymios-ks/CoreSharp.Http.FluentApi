# CoreSharp.HttpClient.FluentApi

[![Nuget](https://img.shields.io/nuget/v/CoreSharp.HttpClient.FluentApi)](https://www.nuget.org/packages/CoreSharp.HttpClient.FluentApi)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.HttpClient.FluentApi&metric=coverage)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.HttpClient.FluentApi)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=efthymios-ks_CoreSharp.HttpClient.FluentApi&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=efthymios-ks_CoreSharp.HttpClient.FluentApi)
![GitHub License](https://img.shields.io/github/license/efthymios-ks/CoreSharp.HttpClient.FluentApi)

> HttpClient fluent request framework.

## Features
- Fluent syntax.
- Automatic JSON conversions using `Text.Json` or `Json.NET`.
- In-memory response caching for safe HTTP methods.
- Specific and more meaningful exceptions (`HttpResponseException`, `TimeoutException`).
- Use of `Stream` where applicable instead of eager converting entities to string. **[Optimizes memory consumption]**
- Use of `HttpCompletionOption.ResponseHeadersRead` by default to all requests. **[Optimizes memory consumption and response times]**

## Installation
Install the package with [Nuget](https://www.nuget.org/packages/CoreSharp.HttpClient.FluentApi).
```
dotnet add package CoreSharp.CoreSharp.HttpClient.FluentApi
```

## Examples
### Table of Contents
- [CoreSharp.HttpClient.FluentApi](#coresharphttpclientfluentapi)
	- [Features](#features)
	- [Installation](#installation)
	- [Examples](#examples)
		- [Table of Contents](#table-of-contents)
		- [Start a request chain](#start-a-request-chain)
		- [Headers](#headers)
			- [Accept](#accept)
			- [Authorization](#authorization)
		- [Query parameters](#query-parameters)
		- [Ignore error](#ignore-error)
		- [HttpCompletionOption](#httpcompletionoption)
		- [Timeout](#timeout)
		- [Endpoint](#endpoint)
		- [HTTP method](#http-method)
			- [Safe methods](#safe-methods)
			- [Unsafe methods](#unsafe-methods)
		- [Request body](#request-body)
			- [JSON request body](#json-request-body)
			- [XML request body](#xml-request-body)
		- [Response deserialization](#response-deserialization)
		- [Response caching](#response-caching)

### Start a request chain
```CSharp
HttpClient client = GetClient();
var request = client.Request();
```

### Headers
```CSharp
var response = client
	.Request()
	.WithHeader("Cache-Control", "max-age=3600")
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
IDictionary<string, string> headers = GetHeaders();
var response = client
	.Request()
	.WithHeaders(headers)
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

#### Accept
```CSharp
// Accept: text/xml
var response = client
	.Request()
	.Accept("text/xml")
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// Accept: application/json
var response = client
	.Request()
	.AcceptJson()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// Accept: application/xml
var response = client
	.Request()
	.AcceptXml()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

#### Authorization
```CSharp
// Authorization: 0imfnc8mVLWwsAawjYr4Rx
var response = client
	.Request()
	.WithAuthorization("0imfnc8mVLWwsAawjYr4Rx")
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
var response = client
	.Request()
	.WithBearerToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

### Query parameters
```CSharp
// GET > /users/?Id=1&Name="Makis"
var response = client
	.Request()
	.WithQuery("Id", 1)
	.WithQuery("Name","Makis")
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// GET > /users/?Id=1&Name="Makis"
var response = client
	.Request()
	.WithQuery(new
	{
		Id = 1,
		Name = "Makis"
	})
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// GET > /users/?Id=1&Name="Makis"
var queryParameters = new Dictionary<string, object>()
{
	{ "Id", 1 },
	{ "Name", "Makis" }
};
var response = client
	.Request()
	.WithQuery(queryParameters)
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

### Ignore error
```CSharp
// Propagates / throws exception
var response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// Silence / does not throw exception
var response = client
	.Request()
	.IgnoreError()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

### HttpCompletionOption
```CSharp
var response = client
	.Request()
	.WithCompletionOption(HttpCompletionOption.ResponseContentRead)
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

### Timeout
```CSharp
try
{
	var response = client
		.Request()
		.WithTimeout(TimeSpan.FromSeconds(15))
		.WithEndpoint("users")
		.Get()
		.SendAsync();
}
catch (TimeoutException timeoutException)
{
}
```

### Endpoint
```CSharp
// GET /users/
var response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
// GET /users/1/
int id = 1;
var response = client
	.Request()
	.WithEndpoint("users", id)
	.Get()
	.SendAsync();
```

```CSharp
// GET /users/1/
long id = 1;
var response = client
	.Request()
	.WithEndpoint("users", id)
	.Get()
	.SendAsync();
```

```CSharp
// GET /users/570ffbae-d1b8-4fad-9fef-5d058a283329/
Guid id = Guid.New();
var response = client
	.Request()
	.WithEndpoint("users", id)
	.Get()
	.SendAsync();
```

```CSharp
// GET /users/filter/new/
var response = client
	.Request()
	.WithEndpoint(new [] {"users", "filter", "new" })
	.Get()
	.SendAsync();
```

### HTTP method
#### Safe methods
```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Head()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Options()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Trace()
	.SendAsync();
```

#### Unsafe methods
```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Put()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Patch()
	.SendAsync();
```

```CSharp
var response = client
	.Request()
	.WithEndpoint("users")
	.Delete()
	.SendAsync();
```

### Request body
_(supported only with unsafe methods)_

```CSharp
HttpContent content = GetHttpContent();

var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithBody(content)
	.SendAsync();
```

```CSharp
string content = "data";
string mediaType = MediaTypeNames.Text.Plain;

var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithBody(content, mediaType)
	.SendAsync();
```

#### JSON request body
```CSharp
// Sets request Content-Type: application/json
// and HttpContent to StringContent.
var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithJsonBody(@"
	{
		""Name"": "Dave""
	}")
	.SendAsync();
```

```CSharp
// Serialize object to JSON stream,
// sets request Content-Type: application/json
// and HttpContent to StreamContent.
var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithJsonBody(new
	{
		Name = "Dave"
	})
	.SendAsync();
```

```CSharp
// Sets request Content-Type: application/json
// and HttpContent to StreamContent.
Stream stream = GetJsonStream(new
{
	Name = "Dave"
});

var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithJsonBody(stream)
	.SendAsync();
```

#### XML request body
```CSharp
// Serialize object to XML stream,
// sets request Content-Type: application/xml
// and HttpContent to StreamContent.
var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithXmlBody(new
	{
		Name = "Dave"
	})
	.SendAsync();
```

```CSharp
// Sets request Content-Type: application/xml
// and HttpContent to StreamContent.
Stream stream = GetXmlStream(new
{
	Name = "Dave"
});

var response = client
	.Request()
	.WithEndpoint("users")
	.Post()
	.WithXmlBody(stream)
	.SendAsync();
```

### Response deserialization
```CSharp
HttpResponseMessage response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.SendAsync();
```

```CSharp
byte[] response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.ToBytes()
	.SendAsync();
```

```CSharp
Stream response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.ToStream()
	.SendAsync();
```

```CSharp
string response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.ToString()
	.SendAsync();
```

```CSharp
// Deserializes JSON with System.Text.Json by default
User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>()
	.SendAsync();
```

```CSharp
// Deserialize with System.Text.Json
System.Text.Json.JsonSerializerOptions options = new();
User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>(options)
	.SendAsync();
```

```CSharp
// Deserialize with Newtonsoft.Json
Newtonsoft.Json.JsonSerializerSettings settings = new();
User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>(settings)
	.SendAsync();
```

```CSharp
User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithXmlDeserialize<User>()
	.SendAsync();
```

```CSharp
// Deserialize from string
static User Deserialize(String response) => ...;

User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithGenericDeserialize(Deserialize)
	.SendAsync();
```

```CSharp
// Deserialize from Stream
static User Deserialize(Stream response) => ...;

User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithGenericDeserialize(Deserialize)
	.SendAsync();
```

### Response caching
_(supported only with safe methods that return anything but  `HttpResponseMessage`)_

```CSharp
User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>()
	.WithCache(TimeSpan.FromMinutes(15))
	.SendAsync();
```

```CSharp
static bool CacheInvalidationFactory() = ...;

User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>()
	.WithCache(TimeSpan.FromMinutes(15))
	.WithCacheInvalidation(CacheInvalidationFactory)
	.SendAsync();
```

```CSharp
static Task<bool> CacheInvalidationFactory() = ...;

User response = client
	.Request()
	.WithEndpoint("users")
	.Get()
	.WithJsonDeserialize<User>()
	.WithCache(TimeSpan.FromMinutes(15))
	.WithCacheInvalidation(CacheInvalidationFactory)
	.SendAsync();
```