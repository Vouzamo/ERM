import React from 'react';
import AppInner from './AppInner';

import Amplify from 'aws-amplify';

import { GlobalContextProvider } from './utils/GlobalContext';

Amplify.configure({
    Auth: {
        region: 'us-east-1',
        userPoolId: 'us-east-1_6u0RWKWaV',
        userPoolWebClientId: '3gqq1t3c01f55dd02srt13le9l'
    }
});

export default function App() {

    return (
        <GlobalContextProvider>
            <AppInner />
        </GlobalContextProvider>
    );
}
