import React from 'react';
import AppInner from './AppInner';

import { GlobalContextProvider } from './utils/GlobalContext';

export default function App() {

    return (
        <GlobalContextProvider>
            <AppInner />
        </GlobalContextProvider>
    );
}
