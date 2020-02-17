import React from 'react';
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

export function Detail() {

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

    const { loading, error, data, refetch } = useQuery(QUERY,
        {
            variables: { id }
        }
    );

    const handleSave = (newSource) => {
        alert('Not yet implemented...');
        console.log(newSource);

        // mutation provides success feedback
        // then refetch
        // error notification
    }

    if (error) {

        console.log(error);

        return <Button onClick={refetch}>Retry</Button>;

    }

    return (
        <>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>

            {data &&
                <TypeEditor source={data.source} onSave={handleSave} />
            }
        </>
    );

}