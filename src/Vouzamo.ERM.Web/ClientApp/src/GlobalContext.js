import React, { createContext, useReducer } from 'react';

const initialState = {
    authentication: {
        isAuthenticated: false
    }
};

const globalContext = createContext(initialState);

const { Provider } = globalContext;

const reducer = (state, action) => {
    switch (action.type) {
        case 'SIGN_IN':
            var authentication = {
                isAuthenticated: true,
                token: action.token
            };
            return { ...state, authentication: authentication };
        case 'SIGN_OUT':
            var authentication = {
                isAuthenticated: false
            };
            return { ...state, authentication: authentication };
        default:
            return state;
    };
};

const GlobalContextProvider = ({ children }) => {

    const [state, dispatch] = useReducer(reducer, initialState);

    return <Provider value={{ state, dispatch }}>{children}</Provider>;
};

export { globalContext, GlobalContextProvider };