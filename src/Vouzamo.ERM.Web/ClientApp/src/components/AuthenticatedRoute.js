import React, { useContext } from "react";
import { globalContext } from '../utils/GlobalContext';
import { Route, Redirect } from "react-router-dom";
import Unauthorized from '../routes/Unauthorized';


export default function AuthenticatedRoute({ component: Component, ...rest }) {

    const { state } = useContext(globalContext)

    return (
        <Route {...rest} render={(props) => (state.authentication.isAuthenticated ? <Component {...props} /> : <Unauthorized />)} />
    );

}