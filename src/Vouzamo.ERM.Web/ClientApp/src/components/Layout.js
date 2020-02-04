import React, { useContext } from 'react';
import { authenticationContext } from '../Authentication';

import { AppBar, Toolbar, IconButton, Typography, Button, Link } from '@material-ui/core';
import { Menu as MenuIcon } from '@material-ui/icons';

export default function Layout(props) {

    const authState = useContext(authenticationContext);
    const { state, dispatch } = authState;

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
                    {state.isAuthenticated
                        ? <Button color="inherit" onClick={() => dispatch({ type: 'logout' })}>Logout</Button>
                        : <>
                            <Link color="inherit" href="/register">
                                Register
                            </Link>
                            <Link color="inherit" href="/login">
                                Sign in
                            </Link>
                        </>
                    }
                </Toolbar>
            </AppBar>
            { props.children }
        </>
    );
}