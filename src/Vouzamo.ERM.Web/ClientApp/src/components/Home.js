import React, { Component } from 'react';
import { Typography } from '@material-ui/core';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <Typography>Hello World</Typography>
        );
    }
}