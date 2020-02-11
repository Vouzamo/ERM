import React, { useState } from 'react';
import gql from 'graphql-tag';
import { useQuery } from '@apollo/react-hooks';
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

export function Search() {

    const classes = useStyles();

    const [addOpen, setAddOpen] = useState(false);

    const [query, setQuery] = useState("");
    const [scope, setScope] = useState();
    const [page, setPage] = useState(1);
    const [size, setSize] = useState(3);

    const QUERY = gql`
        query queryTypes($query: String!, $scope: TypeScope, $size: Int!, $page: Int) {
            search: typeSearch(query: $query, scope: $scope, size: $size, page: $page) {
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

    const { loading, error, data, refetch } = useQuery(QUERY, {
        variables: { query, scope, size, page }
    });

    if (error) {

        // enqueueAlert

        return <Button onClick={refetch}>Retry</Button>;

    }

    return (
        <>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>

            <Typography>Types</Typography>

            <input defaultValue={query} onBlur={handleChangeQuery} />
            <select value={scope} onChange={handleChangeScope}>
                <option value="">Any</option>
                <option value="NODES">Nodes</option>
                <option value="EDGES">Edges</option>
                <option value="BOTH">Both</option>
            </select>
            <button onClick={() => refetch()}>Search</button>

            {data &&
                <>
                    <Pagination count={calculateTotalPages(size, data.search.totalCount)} page={page} onChange={handlePagination} />
                    <Grid>
                        {data.search.results.map((result) => {
                            return (
                                <TypeCard key={result.id} source={result} className={classes.card} />
                            );
                        })}
                    </Grid>
                </>
            }

            <AddTypeDialog open={addOpen} onClose={() => setAddOpen(false)} />

            <Fab className={classes.fab} color="primary" aria-label="add" onClick={() => setAddOpen(true)}>
                <AddIcon />
            </Fab>

        </>
    );

}