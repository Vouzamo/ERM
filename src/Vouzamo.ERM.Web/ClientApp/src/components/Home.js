import React, { useContext } from 'react';
import { globalContext } from '../GlobalContext';

import { Typography, Button } from '@material-ui/core';

export function Home() {

    const { state } = useContext(globalContext);

    const token = () => {

        if (state.authentication.isAuthenticated) {
            alert(state.authentication.token);
        } else {
            alert('No token!');
        }
    }

    return (
        <>
            <Typography>Hello World</Typography>
            <Button variant="contained" color="primary" onClick={token}>Output Session</Button>
        </>
    );

}