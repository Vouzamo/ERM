import React, { useContext } from "react";
import { Route, Redirect } from "react-router-dom";
import { authenticationContext } from '../Authentication';

export default function AuthenticatedRoute({ component: Component, ...rest }) {

    const authState = useContext(authenticationContext);
    const { state, dispatch } = authState;

    return (
        <Route {...rest} render={(props) => (state.isAuthenticated === true ? <Component {...props} /> : <Redirect to='/login' />)} />
    );

}