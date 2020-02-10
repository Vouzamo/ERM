import React, { useContext } from 'react';
import { Route } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { ApolloProvider } from 'react-apollo';
import ApolloClient from "apollo-client";
import { InMemoryCache } from "apollo-cache-inmemory";
import { split } from 'apollo-link';
import { HttpLink } from 'apollo-link-http';
import { WebSocketLink } from 'apollo-link-ws';
import { getMainDefinition } from 'apollo-utilities';

import Layout from './Layout';

import { globalContext } from './utils/GlobalContext';
import NotificationsEmitter from './utils/NotificationsEmitter';

import AuthenticatedRoute from './components/AuthenticatedRoute';

import { Home } from './routes/Home';
import { Secure } from './routes/Secure';
import { Types } from './routes/Types';
import { Nodes } from './routes/Nodes';
import { Edges } from './routes/Edges';

export default function AppInner() {

    const { state } = useContext(globalContext);

    const httpLink = new HttpLink({
        uri: 'https://localhost:44328/graphql',
        headers: {
            Authorization: `Bearer ${state.authentication.token ?? ''}`
        }
    });

    const wsLink = new WebSocketLink({
        uri: 'ws://localhost:56432/graphql',
        options: {
            reconnect: true
        }
    });

    const link = split(
        // split based on operation type
        ({ query }) => {
            const definition = getMainDefinition(query);
            return (
                definition.kind === 'OperationDefinition' &&
                definition.operation === 'subscription'
            );
        },
        wsLink,
        httpLink,
    );

    const client = new ApolloClient({
        link: link,
        cache: new InMemoryCache()
    });


    return (
        <ApolloProvider client={client}>
            <SnackbarProvider>
                <NotificationsEmitter maxSnack={3} hideIconVariant>
                    <Layout>
                        <Route path="/" exact component={Home} />
                        <AuthenticatedRoute path="/secure" exact component={Secure} />
                        <AuthenticatedRoute path="/types" component={Types} />
                        <AuthenticatedRoute path="/nodes" component={Nodes} />
                        <AuthenticatedRoute path="/edges" component={Edges} />
                    </Layout>
                </NotificationsEmitter>
            </SnackbarProvider>
        </ApolloProvider>
    );

}