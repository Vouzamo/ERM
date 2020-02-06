import React, { useState, useEffect } from 'react';
import { Link as RouterLink, useHistory } from 'react-router-dom';
import { Auth } from 'aws-amplify';

import Login from './Login';

import { Container, CircularProgress } from '@material-ui/core';
import { AppBar, Toolbar, IconButton, Typography, Button, Link } from '@material-ui/core';
import { Menu as MenuIcon } from '@material-ui/icons';

export default function Layout(props) {

    const history = useHistory();

    const [isLoading, setLoadingState] = useState(true);
    const [isAuthenticated, setAuthenticatedState] = useState(false);

    const logout = () => {
        Auth.signOut().then(() => history.push('/'));
    }

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
        <>
            <AppBar position="static">
                <Toolbar>
                    <IconButton edge="start" color="inherit" aria-label="menu">
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6">
                        Vouzamo : (E)ntity (R)esource (M)anagement
                    </Typography>
                    <Link color="inherit" component={RouterLink} to="/">Home</Link>
                    <Link color="inherit" component={RouterLink} to="/counter">Counter</Link>
                    {isAuthenticated
                        ? <Button color="inherit" onClick={logout}>Logout</Button>
                        : <>
                            <Link color="inherit" component={RouterLink} to="/register">
                                Register
                            </Link>
                            <Link color="inherit" component={RouterLink} to="/login">
                                Sign in
                            </Link>
                        </>
                    }
                </Toolbar>
            </AppBar>
            { isLoading ? loading : props.children }
        </>
    );
}