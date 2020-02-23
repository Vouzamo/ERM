import React from 'react';
import AuthenticatedRoute from '../../components/AuthenticatedRoute';

import { SearchRoute } from './Search';
import { Detail } from './Detail';

export default function Default() {

    return (
        <>
            <AuthenticatedRoute path="/types" exact component={SearchRoute} />
            <AuthenticatedRoute path="/types/:id" exact component={Detail} />
        </>
    );

}