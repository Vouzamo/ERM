import React, { useState, useEffect } from 'react';
import { useSnackbar } from 'notistack';

import { Container, Grid, makeStyles, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, Card, CardHeader, CardContent, Typography, Button } from '@material-ui/core';
import { Add as AddIcon, Save as SaveIcon, ExpandMore as ExpandMoreIcon } from '@material-ui/icons';

import { AddFieldDialog } from './Dialogs';

const useStyles = makeStyles(theme => ({
    panel: {
        width: '100%',
    },
    heading: {
        fontSize: theme.typography.pxToRem(15),
        flexBasis: '33.33%',
        flexShrink: 0,
    },
    secondaryHeading: {
        fontSize: theme.typography.pxToRem(15),
        color: theme.palette.text.secondary,
    },
    actions: {
        display: 'flex',
        justifyContent: 'space-between'
    }
}));

export function TypeEditor({ source, onSave }) {

    const [state, setState] = useState(source);

    const handleSave = (fields) => {

        let type = state;

        type.fields = fields;

        onSave(type);

    }

    console.log(state);

    return (
        <Container>
            <h4>{source.name}</h4>
            <p>{source.id}</p>
            <p>{source.scope}</p>

            <FieldsEditor owner={source.id} fields={source.fields} onSave={handleSave} />
        </Container>
    )

}

export function FieldsEditor({ owner, fields, onSave }) {

    const classes = useStyles();
    const { enqueueSnackbar } = useSnackbar();

    const [expanded, setExpanded] = React.useState(false);
    const [addOpen, setAddOpen] = useState(false);
    const [state, setState] = useState(fields);

    const handleChange = panel => (event, isExpanded) => {
        setExpanded(isExpanded ? panel : false);
    };

    const addField = (field) => {
        let fields = state;

        if (fields.find(f => f.key == field.key)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {
            fields.push(field);

            setExpanded(field.key);
            setState(fields);
            setAddOpen(false);
        }
    }

    console.log(state);

    return (

        <Container>

            {state.length === 0 &&
                <Card>
                    <CardHeader title="Empty" subheader="Add a field to get started..." />
                </Card>
            }

            {state.map((field) => {
                return (
                    <ExpansionPanel className={classes.panel} key={field.key} expanded={expanded === field.key} onChange={handleChange(field.key)}>
                        <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />} aria-controls={`${field.key}-content`} id={`${field.key}-header`}>
                            <Typography className={classes.heading}>{field.name}</Typography>
                            <Typography className={classes.secondaryHeading}>{field.type}{field.enumerable ? '[]' : ''}{field.mandatory ? '*' : ''}</Typography>
                        </ExpansionPanelSummary>
                        <ExpansionPanelDetails>
                            <FieldEditor owner={owner} field={field} />
                        </ExpansionPanelDetails>
                    </ExpansionPanel>
                );
            })}

            <AddFieldDialog open={addOpen} onConfirm={(field) => { addField(field); }} onClose={() => setAddOpen(false)} />

            <Grid className={classes.actions}>
                <Button variant="contained" color="secondary" startIcon={<AddIcon />} onClick={() => setAddOpen(true)}>Add Field</Button>
                <Button variant="contained" color="primary" startIcon={<SaveIcon />} onClick={() => onSave(state)}>Save Changes</Button>
            </Grid>

        </Container>

    );

}

export function FieldEditor({ owner, field }) {

    const [state, setState] = useState(field);

    console.log(state);

    return (

        <Typography>Form goes here</Typography>
        
    );

}