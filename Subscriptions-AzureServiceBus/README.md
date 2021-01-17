# Azure Service Bus Subscriptions Demo

## Technology Used
```
* GraphQL Asp.Net Subscriptions
* Azure Service Bus
```

This is an advanced demonstration showing one way to do out of process eventing with the subscription server in **GraphQL ASP.NET**.  

> This demo requires you to have your own Azure Service Bus namespace registered in Azure. 

### Launching the demo

1. In Visual Studio, open the solution 
2. Right click the solution in solution explorer and choose properties.
3. Select Start multiple projects and choose `QueryMutation-Server` and `Subscriptions-Server`
   * You'll need both to publish an event from a mutation and consume it in a subscription.
4. Enter your Azure Service Bus **connection string**, **topic** and **subscription** names, as required in the following files. (You'll need to have created these using the Azure portal.)
   * `src/QueryMutation-Server/appsettings.json`
   * `src/Subscriptions-Server/appsettings.json`
5. Run the solution


#### Register a Subscription
Using a GraphQL tool register a subscription to `http://localhost:5001/graphql` 
```graphql
subscription {
    widgetChanged(namelike: "*"){
        id
        name
        description
    }
}
```

#### Execute a Mutation
Using another window/tab submit a mutation to `http://localhost:5000/`
``` graphql
mutation {
    updateWidget(widgetData: {id: 1, name: "New Name", description: "New Desc"}){
        id
        name
    }
}
```

The event will be published to the service bus and consumed by the `Subscriptions-Server` where your registered subscription will be notified of the update. 


### Service Bus Connector Project
The `graphql-aspnet-azure-servicebus-connector.csproj` project contains the custom subscription publisher need to push events to the ASB as well as the background hosted service used by `Subscriptions-Server` to listen for events on an ASB topic. How these components are registered is shown in the `startup.cs` files of each server project


**Disclaimer:** Scalability is not a trivial subject. This project is only a demo to show how one can pass information through a message broker like Azure Service Bus to enable scalability. How you consume this data; the topics, subscriptions, message routing rules, dequeueing rules, serialization and deserialziation of events are all very complex topics that you'll need to address individually to achieve your goals.