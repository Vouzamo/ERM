import React from 'react';
import gql from 'graphql-tag';
import { useSnackbar } from 'notistack';

import { useSubscription } from '@apollo/react-hooks';

export default function NotificationsEmitter(props) {

    const { enqueueSnackbar } = useSnackbar();

    useSubscription(
        gql`
          subscription onNotification {
            notifications(recipient: "${'state.sessionId'}") {
              title
              message
              severity
            }
          }
        `,
        {
            onSubscriptionData: (e) => {

                console.log(e.subscriptionData);

                var { message, severity } = e.subscriptionData.data.notifications;

                let variant = severity.toLowerCase();

                enqueueSnackbar(message, { variant });
            }
        }
    );

    return (
        <>{ props.children }</>
    );

}