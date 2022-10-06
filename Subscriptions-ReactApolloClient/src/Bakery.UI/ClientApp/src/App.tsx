import React from 'react';
import './App.css';

import { ApolloClient, ApolloProvider } from '@apollo/client';
import { InMemoryCache, HttpLink } from '@apollo/client';
import { split } from '@apollo/client';
import { getMainDefinition } from '@apollo/client/utilities';
import { GraphQLWsLink } from '@apollo/client/link/subscriptions';
import { createClient } from 'graphql-ws';
import Bakery from './components/bakery';

const httpLink = new HttpLink({
    uri: 'http://localhost:5001/graphql',
});

// configure for graphql-transport-ws protocol
const wsLink = new GraphQLWsLink(createClient({
    url: 'ws://localhost:5001/graphql',
}));

// create a split link network layer to use web sockets for subscriptions
// and http for query and mutations
const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);
        return (
            definition.kind === 'OperationDefinition' &&
            definition.operation === 'subscription'
        );
    },
    wsLink,
    httpLink
);

const client = new ApolloClient({
    link: splitLink,
    cache: new InMemoryCache(),
});

function App() {
    return (
        <ApolloProvider client={client}>
            <Bakery />
        </ApolloProvider>
    );
}

export default App;
