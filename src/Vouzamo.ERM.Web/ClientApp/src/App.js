import React from 'react';
import { Route } from 'react-router-dom';

import { GlobalContextProvider } from './GlobalContext';

import Amplify, { Auth } from 'aws-amplify';
import AuthenticatedRoute from './components/AuthenticatedRoute';

import Layout from './components/Layout';
import { Home } from './components/Home';
import Login from './components/Login';
import { Counter } from './components/Counter';

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
                <AuthenticatedRoute path="/" exact component={Home} />
                <Route path="/counter" exact component={Counter} />
                <Route path="/login" exact component={Login} />
            </Layout>
        </GlobalContextProvider>
    );
}
