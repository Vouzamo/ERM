import React, { Component } from 'react';
import { Typography, Button } from '@material-ui/core';
import { Auth } from 'aws-amplify';

export function Home() {

    const token = () => {
        return Auth.currentSession().then(session => alert(session.idToken.jwtToken)).catch(error => alert(error.message));
    }

    return (
        <>
            <Typography>Hello World</Typography>
            <Button variant="contained" color="primary" onClick={token}>Output Session</Button>
        </>
    );

}