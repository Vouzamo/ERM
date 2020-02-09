import React from 'react';
import { Route } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import Amplify from 'aws-amplify';

import Layout from './Layout';

import { GlobalContextProvider } from './utils/GlobalContext';
import NotificationsEmitter from './utils/NotificationsEmitter';

import AuthenticatedRoute from './components/AuthenticatedRoute';

import { Home } from './routes/Home';
import { Secure } from './routes/Secure';
import { Types } from './routes/Types';
import { Nodes } from './routes/Nodes';
import { Edges } from './routes/Edges';


Amplify.configure({
    Auth: {
        region: 'us-east-1',
        userPoolId: 'us-east-1_6u0RWKWaV',
        userPoolWebClientId: '3gqq1t3c01f55dd02srt13le9l'
    }
});

export default function App() {

    return (
        <GlobalContextProvider>
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
        </GlobalContextProvider>
    );
}
