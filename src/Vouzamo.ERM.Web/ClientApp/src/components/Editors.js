import React, { useState } from 'react';
import { useSnackbar } from 'notistack';

import { Container, Grid, makeStyles, Card, CardHeader, CardContent, Typography, Button } from '@material-ui/core';
import { Add as AddIcon, Save as SaveIcon } from '@material-ui/icons';

import { AddFieldDialog } from './Dialogs';

const useStyles = makeStyles(theme => ({
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

    const [addOpen, setAddOpen] = useState(false);
    const [state, setState] = useState(fields);

    const addField = (field) => {
        let fields = state;

        if (fields.find(f => f.key == field.key)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {
            fields.push(field);

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
                return <FieldEditor key={field.key} owner={owner} field={field} />
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

        <Card>
            <CardHeader title={field.name} subheader={field.key} />
            <CardContent>
                <Typography variant="body2" color="textSecondary" component="p">
                    You should be able to edit all aspects of a field here
                </Typography>
            </CardContent>
        </Card>
        
    );

}