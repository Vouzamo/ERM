import React, { useState, useEffect } from 'react';
import gql from 'graphql-tag';
import { useQuery } from '@apollo/react-hooks';
import { Container, Typography, CircularProgress } from '@material-ui/core';
import { Pagination } from '@material-ui/lab';

export function Types() {

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

    if (loading) return (<CircularProgress />);
    if (error) return `Error! ${error.message}`;

    return (
        <Container>
            <Typography>Types</Typography>
            <input defaultValue={query} onBlur={handleChangeQuery} />
            <select value={scope} onChange={handleChangeScope}>
                <option value="">Any</option>
                <option value="NODES">Nodes</option>
                <option value="EDGES">Edges</option>
                <option value="BOTH">Both</option>
            </select>
            <button onClick={() => refetch()}>Search</button>

            { data &&
                <>
                    <Pagination count={calculateTotalPages(size, data.search.totalCount)} page={page} onChange={handlePagination} />
                    {data.search.results.map((result, i) => {
                        return (
                            <Typography key={i}>{result.id} {result.name}</Typography>
                        );
                    })}
                </>
            }

        </Container>
    );

}