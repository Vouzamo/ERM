import React from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { makeStyles, Card, CardHeader, Avatar, CardContent, CardActions, Typography, IconButton } from '@material-ui/core';
import { Edit as EditIcon, Delete as DeleteIcon, BubbleChart as NodeIcon, LinearScale as EdgeIcon, Extension as TypeIcon } from '@material-ui/icons';
import { pink } from '@material-ui/core/colors';

const useStyles = makeStyles(theme => ({
    root: {
        display: 'flex',
    },
    body: {
        flexGrow: 1
    },
    avatar: {
        backgroundColor: pink[500]
    }
}));

export function TypeCard(props) {

    const classes = useStyles();

    const { source } = props;

    const ScopeIcon = (scope) => {
        switch (scope) {
            case 'NODES':
                return <NodeIcon />;
            case 'EDGES':
                return <EdgeIcon />;
            case 'TYPES':
                return <TypeIcon />;
            default:
                return null;
        }
    }

    const avatar = (scope) => (
        <Avatar aria-label="recipe" className={classes.avatar}>
            {ScopeIcon(scope)}
        </Avatar>
    );

    return (
        <Card className={props.className + ' ' + classes.root}>
            <CardHeader avatar={avatar(source.scope)} title={source.name} subheader={source.id} />
            <CardContent className={classes.body}>
                <Typography variant="body2" color="textSecondary" component="p">
                    This is an example card description. I'm not sure how useful description is yet.
                </Typography>
            </CardContent>
            <CardActions disableSpacing>
                <IconButton component={RouterLink} aria-label="edit" to={`/types/${source.id}`}>
                    <EditIcon />
                </IconButton>
                <IconButton aria-label="delete">
                    <DeleteIcon />
                </IconButton>
            </CardActions>
        </Card>
    );

}