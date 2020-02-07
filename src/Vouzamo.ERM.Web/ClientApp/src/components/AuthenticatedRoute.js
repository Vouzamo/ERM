import React, { useContext } from "react";
import { globalContext } from '../GlobalContext';
import { Route, Redirect } from "react-router-dom";


export default function AuthenticatedRoute({ component: Component, ...rest }) {

    const { state } = useContext(globalContext)

    return (
        <Route {...rest} render={(props) => (state.authentication.isAuthenticated ? <Component {...props} /> : <Redirect to='/login' />)} />
    );

}