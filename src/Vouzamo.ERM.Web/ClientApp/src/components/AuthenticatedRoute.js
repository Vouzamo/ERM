import React, { useState, useEffect } from "react";
import { Route, Redirect } from "react-router-dom";
import { Container, CircularProgress } from '@material-ui/core';
import { Auth } from 'aws-amplify';

export default function AuthenticatedRoute({ component: Component, ...rest }) {

    const [isLoading, setLoadingState] = useState(true);
    const [isAuthenticated, setAuthenticatedState] = useState(false);

    useEffect(() => {
        Auth.currentSession()
            .then(() => setAuthenticatedState(true))
            .catch(() => setAuthenticatedState(false))
            .finally(() => setLoadingState(false))
    });

    const loading = (
        <Container>
            <CircularProgress />
        </Container>
    );

    return (
        isLoading ? loading : 
        <Route {...rest} render={(props) => (isAuthenticated ? <Component {...props} /> : <Redirect to='/login' />)} />
    );

}