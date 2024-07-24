// src/App.tsx

import React from 'react';
import FileList from './components/Directory'; // Adjust the path as per your file structure
import './App.css'; // Example CSS import
import { library } from '@fortawesome/fontawesome-svg-core';
import { faFolder, faFile } from '@fortawesome/free-solid-svg-icons';

library.add(faFolder, faFile);

const App: React.FC = () => {
    return (
        <div className="App">
            <h1>WELCOME TO FOLDER EXPLORER</h1>
            <main>
                <FileList />
            </main>
        </div>
    );
};

export default App;
