import React, { createContext, useReducer } from 'react';
import Amplify, { Auth } from 'aws-amplify';

Amplify.configure({
    Auth: {
        region: 'us-east-1',
        userPoolId: 'us-east-1_6u0RWKWaV',
        userPoolWebClientId: '3gqq1t3c01f55dd02srt13le9l'
    }
});

const initialState = { isAuthenticated: false };

const authenticationContext = createContext(initialState);

const { Provider } = authenticationContext;

function reducer(state, action) {

    try {
        switch (action.type) {
            case 'login':
                Auth.signIn(action.email, action.password);
                return { ...state, isAuthenticated: true };
            case 'logout':
                Auth.signOut();
                return { ...state, isAuthenticated: false };
            default:
                return state;
        }
    } catch {
        return state;
    }
}

const AuthenticationProvider = ({ children }) => {

    const [state, dispatch] = useReducer(reducer, initialState);

    return <Provider value={{ state, dispatch }}>{children}</Provider>;

};

export { authenticationContext, AuthenticationProvider }