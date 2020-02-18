import React, { createContext, useReducer } from 'react';
import { useParams } from 'react-router-dom';
import gql from 'graphql-tag';
import { useQuery } from '@apollo/react-hooks';
import { Backdrop, CircularProgress, Button, makeStyles } from '@material-ui/core';

import { TypeEditor } from '../../components/Editors';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    }
}));

const initialState = {
    
};

const reducer = (state, action) => {
    switch (action.type) {
        case 'SET_TYPE':
            return action.source;
        case 'UPDATE_FIELD':

            let index = state.fields.findIndex(f => f.key === action.field.key);

            if (index !== -1) {

                let fields = state.fields;

                fields[index] = action.field;

                return {
                    ...state, fields: fields
                };

            }

            return state;

        case 'UPDATE_FIELDS':
            return {
                ...state, fields: action.fields
            };
        default:
            return state;
    };
};

const TypeContext = createContext(initialState);

const Detail = () => {

    const [state, dispatch] = useReducer(reducer, initialState);
    const classes = useStyles();
    const { id } = useParams();

    const QUERY = gql`
        query queryType($id: ID!) {
            source: type(id: $id) {
                id
                name
                scope
                fields {
                    type
                    key
                    name
                    mandatory
                    enumerable
                    localizable
                    ... on StringField {
                        minLength
                        maxLength
                    }
                    ... on IntegerField {
                        minValue
                        maxValue
                    }
                    ... on NestedField {
                        typeId
                    }
                }
            }
        }
    `;

    const { loading, error, refetch } = useQuery(QUERY,
        {
            variables: { id },
            onCompleted: (data) => {
                dispatch({ type: 'SET_TYPE', source: data.source });
            }
        }
    );

    if (error) {

        console.log(error);

        return (
            <Button onClick={refetch}>Retry</Button>
        );

    }

    return (
        <TypeContext.Provider value={{ state, dispatch }}>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>
            <TypeEditor />
        </TypeContext.Provider>
    );

}

export { Detail, TypeContext }