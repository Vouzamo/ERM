import React, { useContext } from 'react';
import { useSnackbar } from 'notistack';
import { Typography, Button } from '@material-ui/core';

import { globalContext } from '../utils/GlobalContext';

export function Secure() {

    const { state } = useContext(globalContext);
    const { enqueueSnackbar } = useSnackbar();

    const token = () => {

        if (state.authentication.isAuthenticated) {
            enqueueSnackbar(state.authentication.token);
            console.log(state.authentication.token);
        } else {
            enqueueSnackbar('No token!', { variant: 'error' });
        }
    }

    return (
        <>
            <Typography>Hello World</Typography>
            <Button variant="contained" color="primary" onClick={token}>Output Session</Button>
        </>
    );

}