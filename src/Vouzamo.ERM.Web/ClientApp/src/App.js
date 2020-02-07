import React from 'react';
import { Route } from 'react-router-dom';

import { GlobalContextProvider } from './GlobalContext';

import Amplify from 'aws-amplify';
import AuthenticatedRoute from './components/AuthenticatedRoute';

import Layout from './components/Layout';
import { Home } from './components/Home';
import { Secure } from './components/Secure';

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
            <Layout>
                <Route path="/" exact component={Home} />
                <AuthenticatedRoute path="/secure" exact component={Secure} />
            </Layout>
        </GlobalContextProvider>
    );
}
