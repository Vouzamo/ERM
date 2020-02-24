import React, { useState } from 'react';
import gql from 'graphql-tag';
import { useQuery } from '@apollo/react-hooks';
import { useSnackbar } from 'notistack';
import { Grid, Typography, Fab, Backdrop, CircularProgress, Button, makeStyles } from '@material-ui/core';
import { Add as AddIcon } from '@material-ui/icons';
import { Pagination } from '@material-ui/lab';

import { TypeCard } from '../../components/Cards';
import { AddTypeDialog } from '../../components/Dialogs';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    },
    card: {
        margin: theme.spacing(1)
    },
    fab: {
        position: 'absolute',
        bottom: theme.spacing(2),
        right: theme.spacing(2),
    }
}));

export function SearchRoute() {

    const classes = useStyles();
    const [addOpen, setAddOpen] = useState(false);

    return (
        <>
            <Typography>Types</Typography>

            <Search>
                {(results) => results.map((result) => <TypeCard key={result.id} source={result} className={classes.card} />)}
            </Search>

            <AddTypeDialog open={addOpen} onClose={() => setAddOpen(false)} />
            <Fab className={classes.fab} color="primary" aria-label="add" onClick={() => setAddOpen(true)}>
                <AddIcon />
            </Fab>
        </>
    );

}


export function Search({ children, scope, size }) {

    const { enqueueSnackbar } = useSnackbar();
    const classes = useStyles();

    const [query, setQuery] = useState('');
    const [scopeState, setScope] = useState(scope);
    const [page, setPage] = useState(1);
    const [sizeState, setSize] = useState(size ?? 10);

    const QUERY = gql`
        query queryTypes($query: String!, $scopeState: TypeScope, $sizeState: Int!, $page: Int) {
            search: typeSearch(query: $query, scope: $scopeState, size: $sizeState, page: $page) {
                totalCount
                page
                size
                hasPrevious
                hasNext
                results {
                  id
                  name
                  scope
                }
            }
        }
    `;

    const handlePagination = (event, value) => setPage(value);

    const handleChangeQuery = (e) => {
        setQuery(e.target.value)
        setPage(1);
    }

    const handleChangeScope = (e) => {

        console.log(e.target.value);

        e.target.value === "" ? setScope() : setScope(e.target.value);
        setPage(1);
    }

    const calculateTotalPages = (size, total) => {

        let totalPages = (total / size);

        totalPages = (total % size > 0) ? Math.ceil(totalPages) : Math.floor(totalPages);

        return totalPages;
    }

    const { loading, error, data, refetch } = useQuery(QUERY,
        {
            variables: { query, scopeState, sizeState, page },
        }
    );

    if (error) {

        enqueueSnackbar(error.message, { variant: 'error' });

        return null;

    }

    return (
        <>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>

            <input defaultValue={query} onBlur={handleChangeQuery} />
            <select value={scopeState} onChange={handleChangeScope}>
                <option value="">Any</option>
                <option value="NODES">Nodes</option>
                <option value="EDGES">Edges</option>
                <option value="TYPES">Types</option>
            </select>
            <button onClick={() => refetch()}>Search</button>

            {data &&
                <>
                    <Pagination count={calculateTotalPages(sizeState, data.search.totalCount)} page={page} onChange={handlePagination} />
                    <Grid>{children(data.search.results)}</Grid>
                </>
            }

        </>
    );

}