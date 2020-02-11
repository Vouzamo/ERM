import React from 'react';
import AuthenticatedRoute from '../../components/AuthenticatedRoute';

import { Search } from './Search';
import { Detail } from './Detail';

export default function Default() {

    return (
        <>
            <AuthenticatedRoute path="/types" exact component={Search} />
            <AuthenticatedRoute path="/types/:id" exact component={Detail} />
        </>
    );

}