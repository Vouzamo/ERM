import React, { createContext, useReducer } from 'react';

const uuid = () => {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

const initialState = {
    authentication: {
        isAuthenticated: false,
        token: ''
    },
    server: 'localhost',
    sessionId: uuid()
};

const globalContext = createContext(initialState);

const { Provider } = globalContext;

const reducer = (state, action) => {
    switch (action.type) {
        case 'SIGN_IN':
            return {
                ...state, authentication: {
                    isAuthenticated: true,
                    token: action.token
                } };
        case 'SIGN_OUT':
            return {
                ...state, authentication: {
                    isAuthenticated: false,
                    token: ''
                } };
        case 'SET_SERVER':
            return { ...state, server: action.server }
        default:
            return state;
    };
};

const GlobalContextProvider = ({ children }) => {

    const [state, dispatch] = useReducer(reducer, initialState);

    return <Provider value={{ state, dispatch }}>{children}</Provider>;
};

export { globalContext, GlobalContextProvider };