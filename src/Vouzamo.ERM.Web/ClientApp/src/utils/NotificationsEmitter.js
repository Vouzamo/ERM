import React, { useContext } from 'react';
import gql from 'graphql-tag';
import { useSnackbar } from 'notistack';
import ApolloClient from "apollo-client";
import { InMemoryCache } from "apollo-cache-inmemory";
import { split } from 'apollo-link';
import { useSubscription } from '@apollo/react-hooks';
import { HttpLink } from 'apollo-link-http';
import { WebSocketLink } from 'apollo-link-ws';
import { getMainDefinition } from 'apollo-utilities';

import { globalContext } from '../utils/GlobalContext';

export default function NotificationsEmitter(props) {

    const { state } = useContext(globalContext);
    const { enqueueSnackbar } = useSnackbar();

    const httpLink = new HttpLink({
        uri: 'https://localhost:56432/graphql'
    });

    const wsLink = new WebSocketLink({
        uri: 'ws://localhost:56432/graphql',
        options: {
            reconnect: true
        },
        connectionParams: {
            authToken: state.authentication.token,
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

    const handleData = (e) => {
        
        console.log(e.subscriptionData);

        var { message, severity } = e.subscriptionData.data.notifications;

        let variant = severity.toLowerCase();

        enqueueSnackbar(message, { variant });
    }

    const { loading, error, data } = useSubscription(
        gql`
          subscription onNotification {
            notifications(recipient: "${'state.sessionId'}") {
              title
              message
              severity
            }
          }
        `,
        { client: client, onSubscriptionData: handleData }
    );

    return (
        <>{ props.children }</>
    );

}