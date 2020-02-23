import React, { useState } from 'react';
import { Typography, IconButton } from '@material-ui/core';
import { Edit as EditIcon, Clear as ClearIcon } from '@material-ui/icons';

import { SearchTypeDialog } from './Dialogs';

const TypeSelector = ({ name, value, onChange }) => {

    const [searchOpen, setSearchOpen] = useState(false);

    const handleSubmit = (selected) => {

        var event = {
            target: {
                name: name,
                value: selected
            }
        }

        onChange(event);

    }

    return (
        <>
            <SearchTypeDialog open={searchOpen} onClose={() => setSearchOpen(false)} scope="TYPES" onSubmit={(selected) => handleSubmit(selected)} />

            {(value !== null && value !== undefined) &&
                <Typography>{value}</Typography>
            }
            <IconButton onClick={() => handleSubmit(null)}><ClearIcon /></IconButton>
            <IconButton onClick={() => setSearchOpen(true)}><EditIcon /></IconButton>
        </>
    );

}

export { TypeSelector }