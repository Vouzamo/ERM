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
    data: {},
    activeFieldKey: false
};

const reducer = (state, action) => {
    switch (action.type) {
        case 'SET_TYPE':
            return {
                ...state, data: action.source
            };
        case 'UPDATE_FIELD':

            let update_field = state.data;
            let index = update_field.fields.findIndex(f => f.key === action.field.key);

            if (index !== -1) {

                update_field.fields[index] = action.field;

                return {
                    ...state, update_field
                };

            }

            return state;

        case 'UPDATE_FIELDS':

            let update_fields = state.data;

            update_fields.fields = action.fields;

            return {
                ...state, data: update_fields
            };

        case 'SET_ACTIVE_FIELD':

            return {
                ...state, activeFieldKey: action.key
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