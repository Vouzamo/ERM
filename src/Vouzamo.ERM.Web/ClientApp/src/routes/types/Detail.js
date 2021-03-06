﻿import React, { createContext, useReducer } from 'react';
import { useSnackbar } from 'notistack';
import { useParams } from 'react-router-dom';
import gql from 'graphql-tag';
import { useQuery, useMutation } from '@apollo/react-hooks';
import { Backdrop, CircularProgress, Button, makeStyles } from '@material-ui/core';
import { Save as SaveIcon } from '@material-ui/icons';

import { TypeEditor } from '../../components/Editors';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    }
}));

const initialState = {
    data: {},
    activeFieldKey: false,
    addDialogOpen: false,
    changeDialogOpen: false
};

const reducer = (state, action) => {
    switch (action.type) {
        case 'SET_TYPE':
            return {
                ...state, data: action.source
            };
        case 'SET_ADD_DIALOG':
            return {
                ...state, addDialogOpen: action.open
            }
        case 'SET_CHANGE_DIALOG':
            return {
                ...state, changeDialogOpen: action.open
            }
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
                ...state, activeFieldKey: action.key, changeDialogOpen: false, addDialogOpen: false
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

    const FIELDS_MUTATION = gql`
      mutation Update($id: ID! $fields: [Json]) {
        types {
            updateFields(id: $id, fields: $fields) {
                id
            }
        }
      }
    `;

    const TYPE_MUTATION = gql`
      mutation Update($type: Json!) {
        types {
            updateType(type: $type) {
                id
            }
        }
      }
    `;

    const { enqueueSnackbar } = useSnackbar();
    const [updateFields, { fieldsData }] = useMutation(FIELDS_MUTATION)
    const [updateType, { typeData }] = useMutation(TYPE_MUTATION);

    const { loading, error, refetch } = useQuery(QUERY,
        {
            variables: { id },
            onCompleted: (data) => {
                dispatch({ type: 'SET_TYPE', source: data.source });
            }
        }
    );

    const handleSaveFields = () => {

        let id = state.data.id;
        let fields = state.data.fields;

        updateFields({ variables: { id, fields } })
            .then(() => {
                enqueueSnackbar('Changes saved successfully', { variant: 'success' });
            })
            .catch(error => enqueueSnackbar(error.message, { variant: 'error' }));
    }

    const handleSave = () => {

        let type = state.data;

        updateType({ variables: { type: type } })
            .catch(error => console.log(error));

    }

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
            <Button variant="contained" color="primary" startIcon={<SaveIcon />} onClick={handleSaveFields}>Save Changes</Button>
        </TypeContext.Provider>
    );

}

export { Detail, TypeContext }