﻿import React, { useState, useRef, useEffect } from 'react';
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
    },
    nameField: {
        marginBottom: theme.spacing(3)
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

const SortableField = SortableElement(({ field, expanded, handleExpand, onUpdate, onChangeKey, onRemove }) => {

    const classes = useStyles();
    const [changeKeyOpen, setChangeKeyOpen] = useState(false);

    const isExpanded = expanded === field.key;

    return (
        <>
            <AddFieldDialog field={field} open={changeKeyOpen} onConfirm={(f) => { onChangeKey(field.key, f.key); }} onClose={() => setChangeKeyOpen(false)} />
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
                    <Button variant="outlined" size="small" color="primary" startIcon={<EditIcon />} onClick={() => setChangeKeyOpen(true)}>Change Key</Button>
                    <Button variant="outlined" size="small" color="secondary" startIcon={<DeleteIcon />} onClick={() => onRemove(field.key)}>Remove</Button>
                </ExpansionPanelActions>
            </ExpansionPanel>
        </>
    );
});

const SortableFieldset = SortableContainer(({ fields, expanded, handleExpand, onUpdate, onChangeKey, onRemove }) => {

    return (
        <Grid>
            {fields.map((field, i) => (
                <SortableField key={field.key} index={i} field={field} expanded={expanded} handleExpand={handleExpand} onChangeKey={onChangeKey} onUpdate={onUpdate} onRemove={onRemove} />
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

    const defaultField = {
        key: "",
        name: "",
        type: "string",
        mandatory: false,
        enumerable: false,
        localizable: false
    };

    const handleExpand = panel => (event, isExpanded) => {
        setExpanded(isExpanded ? panel : false);
    };

    const handleSort = ({ oldIndex, newIndex }) => {

        let sorted = arrayMove(state, oldIndex, newIndex);

        console.log(sorted);

        setState(sorted);

    };

    const handleChangeKey = (oldKey, newKey) => {

        let fields = state;

        if (fields.find(f => f.key == newKey)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {

            let index = fields.findIndex(f => f.key == oldKey)
            let field = fields[index];

            field.key = newKey;
            fields[index] = field;

            setExpanded(newKey);
            setState(fields);
            //setChangeKeyOpen(false);

        }

    }

    const handleRemove = (fieldKey) => {

        console.log('remove: ' + fieldKey);

        let index = state.findIndex(f => f.key == fieldKey);

        if (index !== -1) {

            let updated = state;

            updated.splice(index, 1);

            setExpanded(false);
            setState(updated);

        }

    }

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

            <SortableFieldset useDragHandle fields={state} expanded={expanded} onSortEnd={handleSort} onUpdate={handleUpdate} onChangeKey={handleChangeKey} onRemove={handleRemove} handleExpand={handleExpand} />

            <AddFieldDialog field={defaultField} open={addOpen} onConfirm={(field) => { handleAdd(field); }} onClose={() => setAddOpen(false)} />

            <Grid className={classes.actions}>
                <Button variant="contained" color="secondary" startIcon={<AddIcon />} onClick={() => setAddOpen(true)}>Add Field</Button>
                <Button variant="contained" color="primary" startIcon={<SaveIcon />} onClick={() => onSave(state)}>Save Changes</Button>
            </Grid>

        </Container>

    );

}

export function FieldEditor({ field, onUpdate }) {

    const classes = useStyles();
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

        <Grid container spacing={3}>
            <Grid item xs={12} sm={6}>
                <Grid container>
                    <Grid item xs={12} className={classes.nameField}>
                        <TextField required name="name" label="Name" value={state.name} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                    <Grid item xs={12}>
                        <FormControl required>
                            <InputLabel id="type-select-label">Type</InputLabel>
                            <Select name="type" labelId="type-select-label" value={state.type} onChange={(e) => handleUpdate(e)}>
                                <MenuItem value={'string'}>String</MenuItem>
                                <MenuItem value={'integer'}>Integer</MenuItem>
                                <MenuItem value={'nested'}>Nested</MenuItem>
                            </Select>
                        </FormControl>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12} sm={6}>
                <Grid container>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Mandatory" control={<Switch name="mandatory" checked={state.mandatory} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Enumerable" control={<Switch name="enumerable" checked={state.enumerable} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Localizable" control={<Switch name="localizable" checked={state.localizable} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12}>
            {state.type === 'string' &&
                <Grid container>
                    <Grid item xs={12}>
                        <TextField name="minLength" label="Min Length" value={state.minLength} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField name="maxLength" label="Max Length" value={state.maxLength} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                </Grid>
            }
            {state.type === 'integer' &&
                <Grid container>
                    <Grid item xs={12}>
                        <TextField name="minValue" label="Min Value" value={state.minValue} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField name="maxValue" label="Max Value" value={state.maxValue} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                </Grid>
            }
            {state.type === 'nested' &&
                <Grid container>
                    <Grid item xs={12}>
                        <TextField required name="typeId" label="TypeId" value={state.typeId} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                </Grid>
            }
             </Grid>   
        </Grid>
        
    );

}