## Authorization Example

This example project shows how to implement custom authorization using a mocked user store with ASP.NET Identity. The concept is the same regardless of your chosen authentication provider. 

* _Solution:_ `Authorization.sln`


When running this example, check out the `BakeryController` and note that the `allDonuts` field requires authorization while the `donut` field does not.


### Submit a Query with a Restricted field

1. Execute the following query with your tool of choice. Notice that the `donut` field will return data but the `allDonuts` field returns null. It also adds an error message to the response indicating that access was denied.

```graphql
query {
  donut(id: 8){
    id
    flavor
  }
  allDonuts{
    id
    name
    flavor
  }
}
```


2. Add the following header to your HTTP Post request in a manner appropriate for your tool of choice to authenticate the request.  Now the data is returned for all the donuts.
```
 Authorization: abc123
 ```

 _Note_: This header is just an easy example to demonstrate passing of an authenticated user through to GraphQL.  In a real project, you'd likely be using a cookie value or a bearer token depending on your security implementation.
  

### Change the Authorization Method
3. Edit `Startup.cs` and change the authorization method to `PerRequest`

```csharp
services.AddGraphQL(options =>
{
    options.AuthorizationOptions.Method = AuthorizationMethod.PerRequest;
});
```

This enforces broader restrictions on the query processor.  Now when a query is submitted that includes a field the user doesn't have access to, the entire request is rejected, not just the one field that was unauthorized.
