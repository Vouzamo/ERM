import React, { useState, useRef, useEffect } from 'react';
import { useSnackbar } from 'notistack';
import { SortableContainer, SortableElement, SortableHandle } from 'react-sortable-hoc';
import arrayMove from 'array-move';

import { Container, Grid, makeStyles, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, ExpansionPanelActions, Divider, Card, CardHeader, TextField, Typography, Tooltip, IconButton, Button, FormControl, FormControlLabel, InputLabel, Select, MenuItem, Switch } from '@material-ui/core';
import { Add as AddIcon, Save as SaveIcon, Edit as EditIcon, Delete as DeleteIcon, ExpandMore as ExpandMoreIcon, Reorder as ReorderIcon, NewReleases as MandatoryIcon, List as EnumerableIcon, Language as LocalizableIcon } from '@material-ui/icons';

import { AddFieldDialog } from './Dialogs';

const useStyles = makeStyles(theme => ({
    panel: {
        width: '100%',
    },
    dragHandle: {
        marginLeft: theme.spacing(-1.5),
    },
    heading: {
        alignSelf: 'center',
        fontSize: theme.typography.pxToRem(15),
        flexBasis: '33.33%',
        flexShrink: 0,
    },
    secondaryHeading: {
        alignSelf: 'center',
        fontSize: theme.typography.pxToRem(15),
        flexBasis: '33.33%',
        flexShrink: 0,
        color: theme.palette.text.secondary,
        '& span': {
            color: 'red'
        }
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
        <Grid>
            <h4>{source.name}</h4>
            <p>{source.id}</p>
            <p>{source.scope}</p>

            <FieldsEditor owner={source.id} fields={source.fields} onSave={handleSave} />
        </Grid>
    )

}

const DragHandle = SortableHandle(({ classes }) => <Tooltip className={classes.dragHandle} title="Drag to reorder"><IconButton><ReorderIcon /></IconButton></Tooltip>);

const SortableField = SortableElement(({ field, expanded, handleExpand, onUpdate }) => {

    const classes = useStyles();

    const isExpanded = expanded === field.key;

    return (
        <ExpansionPanel className={classes.panel} key={field.key} expanded={isExpanded} onChange={handleExpand(field.key)}>
            <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />} aria-controls={`${field.key}-content`} id={`${field.key}-header`}>
                {!isExpanded && <DragHandle classes={classes} />}
                <Typography className={classes.heading}>{field.key}</Typography>
                {!isExpanded && <>
                    <Typography className={classes.secondaryHeading}>
                        {field.type}
                    </Typography>
                    <Tooltip title="Mandatory"><IconButton disabled={!field.mandatory} color="secondary"><MandatoryIcon /></IconButton></Tooltip>
                    <Tooltip title="Enumerable"><IconButton disabled={!field.enumerable} color="secondary"><EnumerableIcon /></IconButton></Tooltip>
                    <Tooltip title="Localizable"><IconButton disabled={!field.localizable} color="secondary"><LocalizableIcon /></IconButton></Tooltip>
                </>}
            </ExpansionPanelSummary>
            <ExpansionPanelDetails>
                <FieldEditor field={field} onUpdate={onUpdate} />
            </ExpansionPanelDetails>
            <Divider />
            <ExpansionPanelActions>
                <Button variant="outlined" size="small" color="primary" startIcon={<EditIcon />}>Change Key</Button>
                <Button variant="outlined" size="small" color="secondary" startIcon={<DeleteIcon />}>Remove</Button>
            </ExpansionPanelActions>
        </ExpansionPanel>
    );
});

const SortableFieldset = SortableContainer(({ fields, expanded, handleExpand, onUpdate }) => {

    return (
        <Grid>
            {fields.map((field, i) => (
                <SortableField key={field.key} index={i} field={field} expanded={expanded} handleExpand={handleExpand} onUpdate={onUpdate} />
            ))}
        </Grid>
    );
});

export function FieldsEditor({ owner, fields, onSave }) {

    const classes = useStyles();
    const { enqueueSnackbar } = useSnackbar();

    const [addOpen, setAddOpen] = useState(false);
    const [state, setState] = useState(fields);
    const [expanded, setExpanded] = useState(false);

    const handleExpand = panel => (event, isExpanded) => {
        setExpanded(isExpanded ? panel : false);
    };

    const handleSort = ({ oldIndex, newIndex }) => {

        let sorted = arrayMove(state, oldIndex, newIndex);

        console.log(sorted);

        setState(sorted);

    };

    const handleUpdate = (field) => {

        let index = state.findIndex(f => f.key === field.key);

        if (index !== -1) {

            let updated = state;

            updated[index] = field;

            setState(updated);
            
        }
    }

    const handleAdd = (field) => {
        let fields = state;

        if (fields.find(f => f.key == field.key)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {

            field.mandatory = false;
            field.enumerable = false;
            field.localizable = false;

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

            <SortableFieldset useDragHandle fields={state} expanded={expanded} onSortEnd={handleSort} onUpdate={handleUpdate} handleExpand={handleExpand} />

            <AddFieldDialog open={addOpen} onConfirm={(field) => { handleAdd(field); }} onClose={() => setAddOpen(false)} />

            <Grid className={classes.actions}>
                <Button variant="contained" color="secondary" startIcon={<AddIcon />} onClick={() => setAddOpen(true)}>Add Field</Button>
                <Button variant="contained" color="primary" startIcon={<SaveIcon />} onClick={() => onSave(state)}>Save Changes</Button>
            </Grid>

        </Container>

    );

}

export function FieldEditor({ field, onUpdate }) {

    const [state, setState] = useState(field);

    const handleUpdate = (e) => {

        let newState = { ...state, [e.target.name]: e.target.value }

        setState(newState);

        onUpdate(newState);

    }

    const handleSwitch = (e) => {

        let currentState = state[e.target.name] || false;

        let newState = { ...state, [e.target.name]: !currentState }

        console.log(e);
        console.log(e.target.name);
        console.log(currentState);
        console.log(newState);

        setState(newState);

        onUpdate(newState);

    }

    return (

        <Grid>
            <TextField required variant="outlined" name="name" label="Name" value={state.name} onChange={(e) => handleUpdate(e)} />
            <FormControl required variant="outlined">
                <InputLabel id="type-select-label">Type</InputLabel>
                <Select name="type" labelId="type-select-label" fullWidth value={state.type} onChange={(e) => handleUpdate(e)}>
                    <MenuItem value={'string'}>String</MenuItem>
                    <MenuItem value={'integer'}>Integer</MenuItem>
                    <MenuItem value={'nested'}>Nested</MenuItem>
                </Select>
            </FormControl>
            <FormControl>
                <FormControlLabel label="Mandatory" fullwidth control={<Switch name="mandatory" checked={state.mandatory} onChange={(e) => handleSwitch(e)} value={true} />} />
            </FormControl>
            <FormControl>
                <FormControlLabel label="Enumerable" fullwidth control={<Switch name="enumerable" checked={state.enumerable} onChange={(e) => handleSwitch(e)} value={true} />} />
            </FormControl>
            <FormControl>
                <FormControlLabel label="Localizable" fullwidth control={<Switch name="localizable" checked={state.localizable} onChange={(e) => handleSwitch(e)} value={true} />} />
            </FormControl>
        </Grid>
        
    );

}