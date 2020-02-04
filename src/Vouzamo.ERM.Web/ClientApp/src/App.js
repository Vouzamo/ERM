import React from 'react';
import { Route } from 'react-router-dom';

import { AuthenticationProvider } from './Authentication';
import AuthenticatedRoute from './components/AuthenticatedRoute';

import Layout from './components/Layout';
import { Home } from './components/Home';
import Login from './components/Login';

export default function App() {

    return (
        <AuthenticationProvider>
            <Layout>
                <AuthenticatedRoute path="/" exact component={Home} />
                <Route path="/login" exact component={Login} />
            </Layout>
        </AuthenticationProvider>
    );
}
