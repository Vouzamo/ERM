import React, { useContext } from 'react';
import { Route } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { ApolloProvider } from 'react-apollo';
import ApolloClient from "apollo-client";
import { InMemoryCache, IntrospectionFragmentMatcher } from 'apollo-cache-inmemory';
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
import Types from './routes/types/Default';
import { Nodes } from './routes/Nodes';
import { Edges } from './routes/Edges';

import Amplify, { Auth } from 'aws-amplify';

Amplify.configure({
    Auth: {
        region: 'us-east-1',
        userPoolId: 'us-east-1_6u0RWKWaV',
        userPoolWebClientId: '3gqq1t3c01f55dd02srt13le9l'
    }
});

export default function AppInner() {

    const { state, dispatch } = useContext(globalContext);

    //Auth.currentSession()
    //    .then(session => dispatch({ type: 'SIGN_IN', token: session.idToken.jwtToken }))
    //    .error(error => console.log(error));

    // HTTPS localhost:44328
    const httpLink = new HttpLink({
        uri: `https://${state.server}:44328/graphql`,
        headers: {
            Authorization: `Bearer ${state.authentication.token ?? ''}`
        }
    });

    // HTTP localhost:56432
    const wsLink = new WebSocketLink({
        uri: `ws://${state.server}:56432/graphql`,
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

    const fragmentMatcher = new IntrospectionFragmentMatcher({
        introspectionQueryResultData: {
            __schema: {
                types: [
                    {
                        kind: "UNION",
                        name: "Field",
                        possibleTypes: [
                            { name: "IntegerField" },
                            { name: "StringField" },
                            { name: "NestedField" }
                        ],
                    },
                ],
            },
        }
    });

    const client = new ApolloClient({
        link: link,
        cache: new InMemoryCache({ fragmentMatcher })
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