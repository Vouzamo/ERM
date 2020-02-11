import React, { useState, useContext } from 'react';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { Auth } from 'aws-amplify';
import { makeStyles, CssBaseline, TextField, Breadcrumbs, Link, Drawer, Container, AppBar, Toolbar, IconButton, Typography, Menu, MenuItem, List, ListItem, ListItemText, Divider } from '@material-ui/core';
import { Menu as MenuIcon, AccountCircle as AccountCircleIcon } from '@material-ui/icons';

import { globalContext } from './utils/GlobalContext';

const useStyles = makeStyles(theme => ({
    root: {
        flexGrow: 1,
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    title: {
        flexGrow: 1,
    },
    input: {
        color: 'white',
        borderColor: 'white'
    },
    list: {
        width: 250,
    }
}));

export default function Layout(props) {

    const { pathname } = useLocation();

    const generateCrumbs = () => {

        let parts = pathname.split('/');

        let crumbs = [{ text: 'HOME', link: '/', isLast: false }];

        for (var i = 1; i < parts.length; i++) {

            var part = parts[i];

            var text = part.toUpperCase();
            var link = parts.slice(0, i + 1).join('/');

            crumbs.push({ text, link, isLast: (i == parts.length - 1) });
        }

        return crumbs;

    }

    const classes = useStyles();

    const [drawer, setDrawer] = useState(false);

    const [anchorEl, setAnchorEl] = useState(null);
    const open = Boolean(anchorEl);

    const { state, dispatch } = useContext(globalContext);

    const handleMenu = event => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const logout = () => {
        Auth.signOut()
            .then(() => {
                dispatch({ type: 'SIGN_OUT' });
            });
    }

    const toggleDrawer = (open) => event => {
        if (event.type === 'keydown' && (event.key === 'Tab' || event.key === 'Shift')) {
            return;
        }

        setDrawer(open);
    };

    return (
        <div className={classes.root}>
            <AppBar position="static">
                <Toolbar>
                    <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu" onClick={toggleDrawer(!drawer)}>
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6" className={classes.title}>
                        Vouzamo: ERM
                    </Typography>
                    <TextField InputProps={{className: classes.input}} defaultValue={state.server} onBlur={(e) => dispatch({ type: 'SET_SERVER', server: e.target.value })} />
                    {state.authentication.isAuthenticated && (
                        <div>
                            <IconButton aria-label="account of current user" aria-controls="menu-appbar" aria-haspopup="true" onClick={handleMenu} color="inherit">
                                <AccountCircleIcon />
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorEl}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                open={open}
                                onClose={handleClose}
                            >
                                <MenuItem>Profile</MenuItem>
                                <MenuItem onClick={logout}>Sign Out</MenuItem>
                            </Menu>
                        </div>
                    )}
                </Toolbar>
            </AppBar>
            <Drawer open={drawer} onClose={toggleDrawer(false)}>
                <div className={classes.list} role="presentation" onClick={toggleDrawer(false)} onKeyDown={toggleDrawer(false)}>
                    <List>
                        <ListItem button component={RouterLink} to="/">
                            <ListItemText primary="Home" />
                        </ListItem>
                        <Divider />
                        <ListItem button component={RouterLink} to="/types">
                            <ListItemText primary="Types" />
                        </ListItem>
                        <ListItem button component={RouterLink} to="/nodes">
                            <ListItemText primary="Nodes" />
                        </ListItem>
                        <ListItem button component={RouterLink} to="/edges">
                            <ListItemText primary="Edges" />
                        </ListItem>
                        <Divider />
                        <ListItem button component={RouterLink} to="/secure">
                            <ListItemText primary="Secure" />
                        </ListItem>
                    </List>
                </div>
            </Drawer>
            <Container component="main">
                <CssBaseline />
                <Breadcrumbs aria-label="breadcrumb">
                    {generateCrumbs().map((crumb, i) => {
                        return crumb.isLast ? (
                            <Typography key={i} color="textPrimary">{crumb.text}</Typography>
                        ) : (
                            <Link key={i} component={RouterLink} color="inherit" to={crumb.link}>{crumb.text}</Link>
                        );
                    })}
                </Breadcrumbs>
                {props.children}
            </Container>
        </div>
    );
}