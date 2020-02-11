import React from 'react';
import { useParams } from 'react-router-dom';

export function Detail() {

    const { id } = useParams();

    return (
        <>
            <h4>Detail</h4>
            <p>{id}</p>
        </>
    );

}