import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { makeStyles, Backdrop, CircularProgress, Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, InputLabel, TextField, Select, MenuItem, Button } from '@material-ui/core';
import gql from 'graphql-tag';
import { useMutation } from '@apollo/react-hooks';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
    }
}));

export function AddTypeDialog(props) {

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

                props.onClose();

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
            <Dialog open={props.open} onClose={props.onClose} aria-labelledby="form-dialog-title">
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
                    <Button onClick={props.onClose} color="secondary">Cancel</Button>
                    <Button onClick={handleConfirm} color="primary">Confirm</Button>
                </DialogActions>
            </Dialog>
        </>
    );

}