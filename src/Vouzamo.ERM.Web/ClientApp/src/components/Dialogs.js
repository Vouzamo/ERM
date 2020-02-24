import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { makeStyles, Typography, Backdrop, CircularProgress, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, FormControl, InputLabel, TextField, Select, MenuItem, Button } from '@material-ui/core';
import gql from 'graphql-tag';
import { useMutation } from '@apollo/react-hooks';

import { Search } from '../routes/types/Search';

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

export function AddFieldDialog({ field, open, onConfirm, onClose }) {

    const [state, setState] = useState(field);

    const handleInput = (e) => {

        let newState = { ...state, [e.target.name]: e.target.value };

        setState(newState);

    }

    return (

        <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Add Field</DialogTitle>
            <DialogContent>
                <DialogContentText>Provide a unique key and type for field.</DialogContentText>
                <TextField
                    autoFocus
                    required
                    name="key"
                    label="Key"
                    type="text"
                    fullWidth
                    value={state.key}
                    onChange={(e) => handleInput(e)}
                />
                <FormControl required>
                    <InputLabel id="type-select-label">Type</InputLabel>
                    <Select name="type" labelId="type-select-label" value={state.type} onChange={(e) => handleInput(e)}>
                        <MenuItem value={'string'}>String</MenuItem>
                        <MenuItem value={'integer'}>Integer</MenuItem>
                        <MenuItem value={'nested'}>Nested</MenuItem>
                    </Select>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="secondary">Cancel</Button>
                <Button onClick={() => onConfirm(state)} color="primary">Confirm</Button>
            </DialogActions>
        </Dialog>

    );

}

export function SearchTypeDialog({ open, onClose, scope, onSubmit }) {

    const [value, setValue] = useState('');

    const handleSubmit = () => {

        // search for value

        onSubmit('GUID');

    }

    return (
        <Dialog open={open} onClose={onClose} aria-labelledby="form-dialog-title">
            <DialogTitle id="form-dialog-title">Search Types</DialogTitle>
            <DialogContent>
                <DialogContentText>Search for type by name:</DialogContentText>
                <Search>
                    {(results) => results.map((result) => <Typography key={result.id}>{result.name}</Typography>)}
                </Search>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="secondary">Cancel</Button>
                <Button onClick={handleSubmit} color="primary">Confirm</Button>
            </DialogActions>
        </Dialog>
    );

}