## Creating Custom Directives

This example project shows how to create and apply Type System and Execution directives.

_Project Type_: .NET  7.0

_Solution:_ `Custom-Directives.sln`

## Type System Directive: @toUpper

The  `@toUpper` directive is a type system directive applie dto to schema field definition. This directive extends the field's resolver and converts any generated string to an UPPER CASE string. 

In this example we apply the directive during Startup.cs:

```csharp
  services.AddGraphQL(o =>
    {
        o.ApplyDirective<ToUpperDirective>()
            .WithArguments(new object[0])
            .ToItems(schemaItem => schemaItem.IsField<Donut>("name"));
    });
```

The `.ToItems()` method is a filter, similar to `.Where()` in Linq to select which schema items to apply the directive to when the schema is created. In this example we use the `.IsField()` extension method to determine that
the schema item is a field of the `Donut` graph type and has the name "name". There are many such helper extensions with many options to target most use cases.

Alternatively we could have done an explicit declaration of the directive on the `Name` property of the `Donut` class and generate the same outcome:

```csharp
public class Donut
{
    [ApplyDirective(typeof(ToUpperDirective))]
    public string Name{ get; set; }
}
```

The method you choose to apply your type system directives largely depends on your style and preference. However, if you don't have direct access to the source code you may be forced to using the `.ApplyDirectve()` method.


##  Execution Directive: @toSarcastic

Execution directives are applied to query documents at run time and may affect the query in a number of ways.  In this example we apply the `@toSarcastic` directive to extend the field resolver and change the name to `sArCaTiC cAsInG`.  Try this query in your favorite query tool:

```graphql
query {
  donut(id:5) {
    id
    name @toSarcastic
    flavor
  }
}
```

Notice that even though the `@toUpper` is applied to the field definition, the `@toSarcastic` directive takes effect since its evaluated at runtime, after the resolver edited by the `@toUpper` directive has already executed.

See the [documentation](https://graphql-aspnet.github.io/docs/advanced/directives) for all the details related to both directive types.
