## Logging Provider

This example project shows how to act on the structured logging events to generate a JSON document of the properties they contain.

_Project Type_: ASP.NET Core 3.0

_Solution:_ `LoggingProvider.sln`

### Example Output

```javascript
[
  {
    eventId: 85120,
    dateTimeUTC: "2019-10-08T23:27:32.7330316+00:00",
    logEntryId: "119904ccd1b74d15b9dd8d4e16b0cd8f",
    eventName: "GraphQL Schema Route Registered",
    schemaType: "GraphQL.AspNet.Schemas.GraphSchema",
    route: "/graphql",
    scopeId: "4df2634ccd2241548113ada294297365"
  },
  {
    eventId: 85100,
    dateTimeUTC: "2019-10-08T23:27:40.4598314+00:00",
    logEntryId: "dbe8104f70f94e54bc604e6125bfd742",
    eventName: "GraphQL Schema Instance Created",
    schemaType: "GraphQL.AspNet.Schemas.GraphSchema",
    instanceName: "-Default-",
    graphTypeCount: 16,
    supportedOperations: ["query"],
    graphTypes: [
      {
        typeName: "__DirectiveLocation",
        typeKind: "ENUM",
        typeType: "GraphQL.AspNet.Schemas.TypeSystem.DirectiveLocation",
        isPublished: true
      },
      {
        typeName: "__TypeKind",
        typeKind: "ENUM",
        typeType: "GraphQL.AspNet.Schemas.TypeSystem.TypeKind",
        isPublished: true
      },
      {
        typeName: "__Directive",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedDirective",
        fieldCount: 4,
        isPublished: true
      },
      {
        typeName: "__EnumValue",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedEnumValue",
        fieldCount: 4,
        isPublished: true
      },
      {
        typeName: "__Field",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedField",
        fieldCount: 6,
        isPublished: true
      },
      {
        typeName: "__InputValue",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedInputValueType",
        fieldCount: 4,
        isPublished: true
      },
      {
        typeName: "__Schema",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedSchema",
        fieldCount: 5,
        isPublished: true
      },
      {
        typeName: "__Type",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Internal.Introspection.Model.IntrospectedType",
        fieldCount: 9,
        isPublished: true
      },
      {
        typeName: "include",
        typeKind: "DIRECTIVE",
        typeType: "GraphQL.AspNet.Directives.Global.IncludeDirective",
        isPublished: true
      },
      {
        typeName: "skip",
        typeKind: "DIRECTIVE",
        typeType: "GraphQL.AspNet.Directives.Global.SkipDirective",
        isPublished: true
      },
      {
        typeName: "DonutFlavor",
        typeKind: "ENUM",
        typeType: "GraphQL.AspNet.Examples.LoggingProvider.Model.DonutFlavor",
        isPublished: true
      },
      {
        typeName: "Donut",
        typeKind: "OBJECT",
        typeType: "GraphQL.AspNet.Examples.LoggingProvider.Model.Donut",
        fieldCount: 4,
        isPublished: true
      },
      {
        typeName: "Query",
        typeKind: "OBJECT",
        typeType: null,
        fieldCount: 3,
        isPublished: true
      },
      {
        typeName: "Boolean",
        typeKind: "SCALAR",
        typeType: "System.Boolean",
        isPublished: true
      },
      {
        typeName: "Int",
        typeKind: "SCALAR",
        typeType: "System.Int32",
        isPublished: true
      },
      {
        typeName: "String",
        typeKind: "SCALAR",
        typeType: "System.String",
        isPublished: true
      }
    ],
    scopeId: "5cf1b96866b14560a8961253b96950f5"
  },
  {
    eventId: 85110,
    dateTimeUTC: "2019-10-08T23:27:40.4841785+00:00",
    logEntryId: "26334067123a408e92c85ba3fdb440fc",
    eventName: "GraphQL Schema Pipeline Instance Created",
    schemaType: "GraphQL.AspNet.Schemas.GraphSchema",
    middlewareCount: 4,
    middleware: [
      "GraphQL Metrics Middleware",
      "GraphQL Security Middleware",
      "GraphQL Directive Middleware",
      "GraphQL Field Resolution Middleware"
    ],
    scopeId: "17be0c12ea824a898b9dfc733b13af73"
  },
  {
    eventId: 86000,
    dateTimeUTC: "2019-10-08T23:27:40.5228883+00:00",
    logEntryId: "a2e22b3ef7554563824852cde410347c",
    eventName: "GraphQL Request Received",
    operationRequestId: "e078c3dcbe7242758a3ae9ac28ee0db9",
    userName: null,
    queryOperationName: null,
    queryText: "query {\n  donut(id : 15){\n    id\n    name\n    flavor\n  }\n}",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86400,
    dateTimeUTC: "2019-10-08T23:27:40.6529116+00:00",
    logEntryId: "0c4fa736a02e4b479bdbeceea1daca6c",
    eventName: "GraphQL Query Plan Generated",
    schemaType: "GraphQL.AspNet.Schemas.GraphSchema",
    isValid: true,
    operationCount: 1,
    estimatedComplexity: 7.5581994,
    maxDepth: 2,
    queryPlanId: "4112bbc7f87441848ea33ebef5b20215",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86500,
    dateTimeUTC: "2019-10-08T23:27:40.6847433+00:00",
    logEntryId: "ffbec6f6120943dda94a0e1e7f3aeeb2",
    eventName: "GraphQL Field Resolution Started",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    executionMode: "PerSourceItem",
    path: "[query]/donut",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86520,
    dateTimeUTC: "2019-10-08T23:27:40.6980272+00:00",
    logEntryId: "4ffe82754b4746ebabb40b780dea6406",
    eventName: "GraphQL Authorization Started",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    path: "[query]/donut",
    userName: null,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86530,
    dateTimeUTC: "2019-10-08T23:27:40.7100963+00:00",
    logEntryId: "2584b29b2f1a406b8ab636ef7bac501f",
    eventName: "GraphQL Authorization Completed",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    path: "[query]/donut",
    userName: null,
    status: "Skipped",
    messages: [],
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86600,
    dateTimeUTC: "2019-10-08T23:27:40.7260805+00:00",
    logEntryId: "23ecfa96a5a14b03a3d24117c4ef9128",
    eventName: "GraphQL Field Controller Action Started",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    controllerTypeName: "GraphQL.AspNet.Examples.LoggingProvider.Controllers.BakeryController",
    actionName: "donut",
    path: "[query]/donut",
    sourceObjectType: "GraphQL.AspNet.Examples.LoggingProvider.Model.Donut",
    isAsync: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86610,
    dateTimeUTC: "2019-10-08T23:27:40.7434378+00:00",
    logEntryId: "f920c9b355d842cb870e27fd3d8c65ce",
    eventName: "GraphQL Field Controller Action Model Validated",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    controllerTypeName: "GraphQL.AspNet.Examples.LoggingProvider.Controllers.BakeryController",
    actionName: "donut",
    path: "[query]/donut",
    isValid: true,
    modelItems: [],
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86640,
    dateTimeUTC: "2019-10-08T23:27:40.7597542+00:00",
    logEntryId: "a89cb133f49b4c849b25008d0c0a2c4b",
    eventName: "GraphQL Field Controller Action Completed",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    controllerTypeName: "GraphQL.AspNet.Examples.LoggingProvider.Controllers.BakeryController",
    actionName: "RetrieveDonut",
    path: "[query]/donut",
    resultTypeName: "GraphQL.AspNet.Execution.GraphFieldRequest",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86599,
    dateTimeUTC: "2019-10-08T23:27:40.77802+00:00",
    logEntryId: "fd113b07098e44eb83e756e9dab47a3a",
    eventName: "GraphQL Field Resolution Completed",
    pipelineRequestId: "04db6b5278d94e859eb2aa5262575aeb",
    path: "[query]/donut",
    typeExpression: "Donut",
    hasData: true,
    resultIsValid: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86500,
    dateTimeUTC: "2019-10-08T23:27:40.8021313+00:00",
    logEntryId: "95920f0e534046dda1c1d097261bff61",
    eventName: "GraphQL Field Resolution Started",
    pipelineRequestId: "04ce491fa5e944acb2c60a078a9aab08",
    executionMode: "PerSourceItem",
    path: "[type]/Donut/Id",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86520,
    dateTimeUTC: "2019-10-08T23:27:40.810226+00:00",
    logEntryId: "7c87b10669d247e2bf4cef7b50cb0091",
    eventName: "GraphQL Authorization Started",
    pipelineRequestId: "04ce491fa5e944acb2c60a078a9aab08",
    path: "[type]/Donut/Id",
    userName: null,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86530,
    dateTimeUTC: "2019-10-08T23:27:40.8198733+00:00",
    logEntryId: "10cf9cd22f6d415a92b69b46903d17e2",
    eventName: "GraphQL Authorization Completed",
    pipelineRequestId: "04ce491fa5e944acb2c60a078a9aab08",
    path: "[type]/Donut/Id",
    userName: null,
    status: "Skipped",
    messages: [],
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86599,
    dateTimeUTC: "2019-10-08T23:27:40.8292905+00:00",
    logEntryId: "441cbb5c5f514a9095592749e7ba592f",
    eventName: "GraphQL Field Resolution Completed",
    pipelineRequestId: "04ce491fa5e944acb2c60a078a9aab08",
    path: "[type]/Donut/Id",
    typeExpression: "Int!",
    hasData: true,
    resultIsValid: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86500,
    dateTimeUTC: "2019-10-08T23:27:40.8391089+00:00",
    logEntryId: "aea7ee5495be415d95b21a24e2bb1ba1",
    eventName: "GraphQL Field Resolution Started",
    pipelineRequestId: "0f489ec1988e4cbb804dfa80faaa9f80",
    executionMode: "PerSourceItem",
    path: "[type]/Donut/Name",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86520,
    dateTimeUTC: "2019-10-08T23:27:40.8471454+00:00",
    logEntryId: "b60877d003bc4bec8e7281b343f72880",
    eventName: "GraphQL Authorization Started",
    pipelineRequestId: "0f489ec1988e4cbb804dfa80faaa9f80",
    path: "[type]/Donut/Name",
    userName: null,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86530,
    dateTimeUTC: "2019-10-08T23:27:40.8563504+00:00",
    logEntryId: "289c4c731004486f9b9a34b63a32bbaf",
    eventName: "GraphQL Authorization Completed",
    pipelineRequestId: "0f489ec1988e4cbb804dfa80faaa9f80",
    path: "[type]/Donut/Name",
    userName: null,
    status: "Skipped",
    messages: [],
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86599,
    dateTimeUTC: "2019-10-08T23:27:40.8652823+00:00",
    logEntryId: "742c0cf499354b95ab1a5e148cbc078e",
    eventName: "GraphQL Field Resolution Completed",
    pipelineRequestId: "0f489ec1988e4cbb804dfa80faaa9f80",
    path: "[type]/Donut/Name",
    typeExpression: "String",
    hasData: true,
    resultIsValid: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86500,
    dateTimeUTC: "2019-10-08T23:27:40.8736226+00:00",
    logEntryId: "e61cf4aeb7644defb4fc0433a7a0d696",
    eventName: "GraphQL Field Resolution Started",
    pipelineRequestId: "10f451f95d06454380ed0d69202d178f",
    executionMode: "PerSourceItem",
    path: "[type]/Donut/Flavor",
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86520,
    dateTimeUTC: "2019-10-08T23:27:40.8815039+00:00",
    logEntryId: "0e4c8c673b574e358706b49f6fbba3c6",
    eventName: "GraphQL Authorization Started",
    pipelineRequestId: "10f451f95d06454380ed0d69202d178f",
    path: "[type]/Donut/Flavor",
    userName: null,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86530,
    dateTimeUTC: "2019-10-08T23:27:40.8896333+00:00",
    logEntryId: "cbdc0df3f7894b15b85c15001caf31c1",
    eventName: "GraphQL Authorization Completed",
    pipelineRequestId: "10f451f95d06454380ed0d69202d178f",
    path: "[type]/Donut/Flavor",
    userName: null,
    status: "Skipped",
    messages: [],
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86599,
    dateTimeUTC: "2019-10-08T23:27:40.8991272+00:00",
    logEntryId: "46a09362f8914e5e9b8064ca07a4e002",
    eventName: "GraphQL Field Resolution Completed",
    pipelineRequestId: "10f451f95d06454380ed0d69202d178f",
    path: "[type]/Donut/Flavor",
    typeExpression: "DonutFlavor!",
    hasData: true,
    resultIsValid: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  },
  {
    eventId: 86700,
    dateTimeUTC: "2019-10-08T23:27:40.9126613+00:00",
    logEntryId: "084c939ce2bb4097843e1c395c7d9dd8",
    eventName: "GraphQL Request Completed",
    operationRequestId: "e078c3dcbe7242758a3ae9ac28ee0db9",
    hasErrors: false,
    hasData: true,
    scopeId: "8ecade1aee63431885416183070a2dfa"
  }
];
```
