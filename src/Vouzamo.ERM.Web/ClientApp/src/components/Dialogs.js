import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { makeStyles, Backdrop, CircularProgress, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, FormControlLabel, Switch, InputLabel, TextField, Select, MenuItem, Button } from '@material-ui/core';
import gql from 'graphql-tag';
import { useMutation } from '@apollo/react-hooks';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    }
}));

export function AddTypeDialog({ open, onClose }) {

    const classes = useStyles();
    const history = useHistory();

    const [name, setName] = useState("");
    const [scope, setScope] = useState("NODES");

    const MUTATION = gql`
        mutation addType($name: String!, $scope: TypeScope!) {
            types {
                added: create(name: $name, scope: $scope) {
                    id
                }
            }
        }
    `;

    const [addType, { loading }] = useMutation(MUTATION,
        {
            onCompleted: (data) => {

                onClose();

                history.push(`/types/${data.types.added.id}`);

            },
            onError: (error) => {

                console.log(error);

            }
        }
    );

    const handleConfirm = () => {

        addType({ variables: { name, scope } });

    }

    return (
        <>
            <Backdrop className={classes.backdrop} open={loading}>
                <CircularProgress color="inherit" />
            </Backdrop>
            <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Add Type</DialogTitle>
                <DialogContent>
                    <DialogContentText>Provide a name and the desired scope for your new type.</DialogContentText>
                    <TextField
                        autoFocus
                        margin="dense"
                        id="name"
                        label="Name"
                        type="text"
                        fullWidth
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <InputLabel id="scope-select-label">Scope</InputLabel>
                    <Select margin="dense" id="scope" labelId="scope-select-label" fullWidth value={scope} onChange={(e) => setScope(e.target.value)}>
                        <MenuItem value={'NODES'}>Nodes</MenuItem>
                        <MenuItem value={'EDGES'}>Edges</MenuItem>
                        <MenuItem value={'TYPES'}>Types</MenuItem>
                    </Select>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose} color="secondary">Cancel</Button>
                    <Button onClick={handleConfirm} color="primary">Confirm</Button>
                </DialogActions>
            </Dialog>
        </>
    );

}

export function AddFieldDialog({ open, onConfirm, onClose }) {

    const [key, setKey] = useState("");
    const [name, setName] = useState("");
    const [type, setType] = useState("string");

    return (

        <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Add Field</DialogTitle>
            <DialogContent>
                <DialogContentText>Provide a unique key, display name, and type for your new field.</DialogContentText>
                <TextField
                    autoFocus
                    id="key"
                    label="Key"
                    type="text"
                    fullWidth
                    value={key}
                    onChange={(e) => setKey(e.target.value)}
                />
                <TextField
                    id="name"
                    label="Name"
                    type="text"
                    fullWidth
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
                <InputLabel id="type-select-label">Type</InputLabel>
                <Select id="type" labelId="type-select-label" fullWidth value={type} onChange={(e) => setType(e.target.value)}>
                    <MenuItem value={'string'}>String</MenuItem>
                    <MenuItem value={'integer'}>Integer</MenuItem>
                    <MenuItem value={'nested'}>Nested</MenuItem>
                </Select>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="secondary">Cancel</Button>
                <Button onClick={() => onConfirm({ key, name, type })} color="primary">Confirm</Button>
            </DialogActions>
        </Dialog>

    );

}