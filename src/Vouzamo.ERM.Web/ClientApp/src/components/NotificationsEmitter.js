import React, { useState, useContext } from 'react';
import gql from 'graphql-tag';
import { globalContext } from '../GlobalContext';
import { Snackbar, Slide, Typography } from '@material-ui/core';
import { Alert, AlertTitle } from '@material-ui/lab';
import ApolloClient from "apollo-client";
import { InMemoryCache } from "apollo-cache-inmemory";
import { split } from 'apollo-link';
import { useSubscription } from '@apollo/react-hooks';
import { HttpLink } from 'apollo-link-http';
import { WebSocketLink } from 'apollo-link-ws';
import { getMainDefinition } from 'apollo-utilities';

export function NotificationsEmitter() {

    const { state } = useContext(globalContext);

    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState({});

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

        var { title, message, severity } = e.subscriptionData.data.notifications;

        severity = severity.toLowerCase().replace('information', 'info');

        setMessage({ title: title, message: message, severity: severity });
        setOpen(true);
    }

    const handleClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        setOpen(false);
    };

    const { loading, error, data } = useSubscription(
        gql`
          subscription onNotification {
            notifications(recipient: "${'johnaskew'}") {
              title
              message
              severity
            }
          }
        `,
        { client: client, onSubscriptionData: handleData }
    );

    return (
        <Snackbar open={open} onClose={handleClose} TransitionComponent={Slide} autoHideDuration={6000}>
            <Alert icon={false} severity={message.severity} onClose={handleClose}>
                <AlertTitle>{message.title}</AlertTitle>
                {message.message}
            </Alert>
        </Snackbar>
    );

}