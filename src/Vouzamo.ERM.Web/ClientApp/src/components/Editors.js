import React, { useState, useContext } from 'react';
import { useSnackbar } from 'notistack';
import { SortableContainer, SortableElement, SortableHandle } from 'react-sortable-hoc';
import arrayMove from 'array-move';

import { Container, Grid, makeStyles, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, ExpansionPanelActions, Divider, Card, CardHeader, TextField, Typography, Tooltip, IconButton, Button, FormControl, FormControlLabel, InputLabel, Select, MenuItem, Slider, Switch } from '@material-ui/core';
import { Add as AddIcon, Save as SaveIcon, Edit as EditIcon, Delete as DeleteIcon, ExpandMore as ExpandMoreIcon, Reorder as ReorderIcon, NewReleases as MandatoryIcon, List as EnumerableIcon, Language as LocalizableIcon } from '@material-ui/icons';

import { TypeContext } from '../routes/types/Detail';
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
    fieldEditor: {
        paddingTop: theme.spacing(3)
    },
    nameField: {
        marginBottom: theme.spacing(3)
    }
}));

export function TypeEditor() {

    const { state } = useContext(TypeContext);

    return (
        <Grid>
            <h4>{state.data.name}</h4>
            <p>{state.data.id}</p>
            <p>{state.data.scope}</p>

            {state.data.fields && <FieldsEditor />}
        </Grid>
    )

}

const DragHandle = SortableHandle(({ disabled, classes }) => <Tooltip className={classes.dragHandle} title="Drag to reorder"><IconButton disabled={disabled}><ReorderIcon /></IconButton></Tooltip>);

const SortableField = SortableElement(({ field }) => {

    const { enqueueSnackbar } = useSnackbar();
    const { state, dispatch } = useContext(TypeContext);
    const classes = useStyles();

    const [changeKeyOpen, setChangeKeyOpen] = useState(false);

    const handleChange = (change) => {

        if (field.key !== change.key && state.data.fields.find(f => f.key == change.key)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {
            let fields = state.data.fields;
            let index = fields.findIndex(f => f.key == field.key)

            fields[index] = change;

            dispatch({ type: 'UPDATE_FIELDS', fields });
            dispatch({ type: 'SET_ACTIVE_FIELD', key: change.key });

            setChangeKeyOpen(false);
        }
    }

    const handleRemove = (key) => {

        let fields = state.data.fields;

        let index = fields.findIndex(f => f.key == key);

        if (index !== -1) {

            fields.splice(index, 1);

            dispatch({ type: 'SET_ACTIVE_FIELD', key: false });
            dispatch({ type: 'UPDATE_FIELDS', fields });

        }
    }

    const handleExpand = panel => (event, isExpanded) => {
        dispatch({ type: 'SET_ACTIVE_FIELD', key: isExpanded ? panel : false })
    };

    const isExpanded = state.activeFieldKey === field.key;

    return (
        <>
            <AddFieldDialog field={field} open={changeKeyOpen} onConfirm={(f) => { handleChange(f); }} onClose={() => setChangeKeyOpen(false)} />
            <ExpansionPanel className={classes.panel} key={field.key} expanded={isExpanded} onChange={handleExpand(field.key)}>
                <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />} aria-controls={`${field.key}-content`} id={`${field.key}-header`}>
                    <DragHandle disabled={isExpanded} classes={classes} />
                    <Typography className={classes.heading}>{field.key}</Typography>
                    <Typography className={classes.secondaryHeading}>{field.type}</Typography>
                    <Tooltip title="Mandatory"><span><IconButton disabled={!field.mandatory} color="secondary"><MandatoryIcon /></IconButton></span></Tooltip>
                    <Tooltip title="Enumerable"><span><IconButton disabled={!field.enumerable} color="secondary"><EnumerableIcon /></IconButton></span></Tooltip>
                    <Tooltip title="Localizable"><span><IconButton disabled={!field.localizable} color="secondary"><LocalizableIcon /></IconButton></span></Tooltip>
                </ExpansionPanelSummary>
                <Divider />
                <ExpansionPanelDetails>
                    <FieldEditor field={field} />
                </ExpansionPanelDetails>
                <Divider />
                <ExpansionPanelActions>
                    <Button variant="outlined" size="small" color="primary" startIcon={<EditIcon />} onClick={() => setChangeKeyOpen(true)}>Change Key/Type</Button>
                    <Button variant="outlined" size="small" color="secondary" startIcon={<DeleteIcon />} onClick={() => handleRemove(field.key)}>Remove</Button>
                </ExpansionPanelActions>
            </ExpansionPanel>
        </>
    );
});

const SortableFieldset = SortableContainer(({ fields }) => {

    return (
        <Grid>
            {fields.map((field, i) => (
                <SortableField key={field.key} index={i} field={field} />
            ))}
        </Grid>
    );
});

export function FieldsEditor() {

    const { state, dispatch } = useContext(TypeContext);
    const classes = useStyles();
    const { enqueueSnackbar } = useSnackbar();

    const defaultField = {
        key: "",
        name: "",
        type: "string",
        mandatory: false,
        enumerable: false,
        localizable: false,
        minValue: 0,
        maxValue: 512,
        minLength: 0,
        maxLength: 512
    };

    const handleSort = ({ oldIndex, newIndex }) => {

        let fields = arrayMove(state.data.fields, oldIndex, newIndex);

        dispatch({ type: 'UPDATE_FIELDS', fields });

    };

    const handleAdd = (field) => {
        let fields = state.data.fields;

        if (fields.find(f => f.key == field.key)) {
            enqueueSnackbar('Each field key must be unique within type', { variant: 'error' });
        } else {

            field.mandatory = false;
            field.enumerable = false;
            field.localizable = false;

            fields.push(field);

            dispatch({ type: 'SET_ACTIVE_FIELD', key: field.key });
            dispatch({ type: 'UPDATE_FIELDS', fields });
            dispatch({ type: 'CLOSE_ADD_DIALOG' })
        }
    }

    return (

        <Container>

            {state.data.fields.length === 0 &&
                <Card>
                    <CardHeader title="Empty" subheader="Add a field to get started..." />
                </Card>
            }

            <SortableFieldset lockAxis="y" useDragHandle fields={state.data.fields} onSortEnd={handleSort} />

            <AddFieldDialog field={defaultField} open={state.addDialogOpen} onConfirm={(field) => { handleAdd(field); }} onClose={() => dispatch({ type: 'CLOSE_ADD_DIALOG' })} />

            <Grid className={classes.actions}>
                <Button variant="contained" color="secondary" startIcon={<AddIcon />} onClick={() => dispatch({ type: 'OPEN_ADD_DIALOG' })}>Add Field</Button>
                <Button variant="contained" color="primary" startIcon={<SaveIcon />} onClick={() => alert(state)}>Save Changes</Button>
            </Grid>

        </Container>

    );

}

export function FieldEditor({ field }) {

    const { dispatch } = useContext(TypeContext);
    const classes = useStyles();

    const handleUpdate = (e) => {

        let newState = { ...field, [e.target.name]: e.target.value }

        dispatch({ type: 'UPDATE_FIELD', field: newState });

    }

    const handleSlide = (e, newValue, target) => {

        let min = newValue[0];
        let max = newValue[1];

        let newState = { ...field, [`min${target}`]: min, [`max${target}`]: max }

        dispatch({ type: 'UPDATE_FIELD', field: newState });

    };

    const handleSwitch = (e) => {

        let currentState = field[e.target.name] || false;

        let newState = { ...field, [e.target.name]: !currentState }

        dispatch({ type: 'UPDATE_FIELD', field: newState });

    }

    return (

        <Grid className={classes.fieldEditor} container spacing={3}>
            <Grid item xs={12} sm={6}>
                <Grid container>
                    <Grid item xs={12} className={classes.nameField}>
                        <TextField required name="name" label="Name" value={field.name} onChange={(e) => handleUpdate(e)} />
                    </Grid>
                </Grid>
            </Grid>
            <Grid item xs={12} sm={6}>
                <Grid container>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Mandatory" control={<Switch name="mandatory" checked={field.mandatory} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Enumerable" control={<Switch name="enumerable" checked={field.enumerable} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                    <Grid item xs={12}>
                        <FormControl>
                            <FormControlLabel label="Localizable" control={<Switch name="localizable" checked={field.localizable} onChange={(e) => handleSwitch(e)} value={true} />} />
                        </FormControl>
                    </Grid>
                </Grid>
            </Grid>
            {(field.type === 'string' || field.type === 'nested') &&
                <Grid item xs={12}>
                    <Divider className={classes.nameField} />
                    {field.type === 'string' &&
                        <Grid container>
                            <Grid item xs={12}>
                                <Typography id="slider-length" gutterBottom>Min/Max Length</Typography>
                                <Slider
                                    min={0}
                                    max={512}
                                    value={[field.minLength, field.maxLength]}
                                    onChange={(e, newValue) => handleSlide(e, newValue, 'Length')}
                                    valueLabelDisplay="auto"
                                    aria-labelledby="slider-length"
                                    getAriaValueText={(value) => value}
                                />
                            </Grid>
                        </Grid>
                    }
                    {field.type === 'nested' &&
                        <Grid container>
                            <Grid item xs={12}>
                                <TextField required name="typeId" label="TypeId" value={field.typeId} onChange={(e) => handleUpdate(e)} />
                            </Grid>
                        </Grid>
                    }
                </Grid>
            }
        </Grid>
        
    );

}