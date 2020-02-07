import React, { useContext } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { globalContext } from '../GlobalContext';
import { Auth } from 'aws-amplify';

import { AppBar, Toolbar, IconButton, Typography, Button, Link } from '@material-ui/core';
import { Menu as MenuIcon } from '@material-ui/icons';

export default function Layout(props) {

    const { state, dispatch } = useContext(globalContext);

    const logout = () => {
        Auth.signOut()
            .then(() => {
                dispatch({ type: 'SIGN_OUT' });
            });
    }

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
                    <Link color="inherit" component={RouterLink} to="/secure">Secure</Link>
                    {state.authentication.isAuthenticated
                        ? <Button color="inherit" onClick={logout}>Logout</Button>
                        : <Link color="inherit" component={RouterLink} to="/secure">Sign in / Register</Link>
                    }
                </Toolbar>
            </AppBar>
            { props.children }
        </>
    );
}