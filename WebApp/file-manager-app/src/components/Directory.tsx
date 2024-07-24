import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Folder from '../models/Folder';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFile, faFolder } from '@fortawesome/free-solid-svg-icons';
import API_BASE_URL from '../config';

const Directory: React.FC = () => {
    const [folders, setFolders] = useState<Folder[]>([]);
    const [selectedFolderId, setSelectedFolderId] = useState<number | null>(null);
    const [newFolderName, setNewFolderName] = useState<string>('');

    useEffect(() => {
        const fetchFolders = async () => {
            try {
                const response = await axios.get<Folder[]>(`${API_BASE_URL}/Folders`);
                setFolders(response.data);
            } catch (error) {
                console.error('Error fetching folders:', error);
            }
        };
        fetchFolders(); // Initial fetch
    }, []);

    const toggleFolder = (folderId: number) => {
        const updatedFolders = toggleFolderInFolders([...folders], folderId);
        setFolders(updatedFolders);
    };

    const toggleFolderInFolders = (foldersToUpdate: Folder[], folderId: number): Folder[] => {
        return foldersToUpdate.map(folder => {
            if (folder.folderId === folderId) {
                return { ...folder, isOpen: !folder.isOpen };
            } else if (folder.subFolders.length > 0) {
                return {
                    ...folder,
                    subFolders: toggleFolderInFolders(folder.subFolders, folderId)
                };
            } else {
                return folder;
            }
        });
    };


    const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>, parentId: number) => {
        const file = event.target.files?.[0];
        if (!file) return;

        try {
            const formData = new FormData();
            formData.append('ParentFolderId', parentId.toString());
            formData.append('file', file);

            await axios.post(`${API_BASE_URL}/Files`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });

            // After successful upload, refetch folders
            fetchFolders();

            // Clear file input
            event.target.value = '';
        } catch (error) {
            console.error('Error uploading file:', error);
        }
    };


    const fetchFolders = async () => {
        try {
            const response = await axios.get<Folder[]>(`${API_BASE_URL}/Folders`);
            
            //recursively merge folders
            const mergeFolders = (newFolders: Folder[], existingFolders: Folder[]): Folder[] => {
                return newFolders.map(newFolder => {
                    const existingFolder = existingFolders.find(folder => folder.folderId === newFolder.folderId);
                    
                    if (existingFolder) {
                        // If existing folder is found, merge and preserve isOpen state
                        return {
                            ...newFolder,
                            isOpen: existingFolder.isOpen,
                            subFolders: mergeFolders(newFolder.subFolders, existingFolder.subFolders)
                        };
                    } else {
                        // Otherwise, return the new folder with default isOpen state
                        return {
                            ...newFolder,
                            isOpen: false, // or newFolder.isOpen if server-side isOpen is available
                            subFolders: newFolder.subFolders
                        };
                    }
                });
            };
    
            const updatedFolders = mergeFolders(response.data, folders);
            setFolders(updatedFolders);
        } catch (error) {
            console.error('Error fetching folders:', error);
        }
    };

    const findFolderById = (foldersToSearch: Folder[], folderId: number): Folder | null => {
        for (let folder of foldersToSearch) {
            if (folder.folderId === folderId) {
                return folder;
            }
            if (folder.subFolders.length > 0) {
                const foundInSubFolder = findFolderById(folder.subFolders, folderId);
                if (foundInSubFolder) {
                    return foundInSubFolder;
                }
            }
        }
        return null;
    };

    const handleFileDownload = async (fileId: number) => {
        try {
            const response = await axios.get(`${API_BASE_URL}/files/${fileId}`, {
                responseType: 'blob',
                withCredentials: true,
            });

            // Extract filename from Content-Disposition header
            console.log(response)
            console.log(response.headers['content-disposition'])
            const contentDisposition = response.headers['content-disposition'];
            let fileName = 'file';
            if (contentDisposition) {
                const fileNameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                const matches = fileNameRegex.exec(contentDisposition);
                if (matches != null && matches[1]) {
                    fileName = matches[1].replace(/['"]/g, '');
                }
            }

            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            console.log(response)
            link.setAttribute('download',fileName); // Set the file name for download
            document.body.appendChild(link);
            link.click();
            link.remove();

        } catch (error) {
            console.error('Error downloading file:', error);
        }
    };

    const createFolder = async () => {
        try {
            await axios.post(`${API_BASE_URL}/Folders`, {
                ParentFolderId:selectedFolderId ? selectedFolderId.toString() : null,
                FolderName: newFolderName
            } );

            // After successful creation, refetch folders and clear the folder creation name
            fetchFolders();
            setNewFolderName('');
        } catch (error) {
            console.error('Error creating folder:', error);
        }
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setNewFolderName(event.target.value);
    };

    const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            createFolder();
        }
    };
 
    const renderFolders = (folders: Folder[]) => {
        return (
            <ul>
                {folders.map(folder => (
                    <ul key={folder.folderId}>
                        <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                            <div onClick={() => setSelectedFolderId(folder.folderId)}  className={`folder-item ${selectedFolderId === folder.folderId ? 'selected-folder' : ''}`}>
                                <FontAwesomeIcon icon={faFolder} style={{ marginRight: '5px' }} />
                                {folder.name}
                            </div>
                            <div onClick={() => toggleFolder(folder.folderId)}  className={`folder-item ${selectedFolderId === folder.folderId ? 'selected-folder' : ''}`}>
                                {folder.isOpen ? '▼ ' : '▶ '}
                            </div>
                        </div>
                        {folder.isOpen && (
                            <>
                                {folder.files.length > 0 && (
                                    <ul>
                                        {folder.files.map(file => (
                                         <li key={file.fileId}>
                                             <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                                             <FontAwesomeIcon icon={faFile} style={{ marginRight: '5px' }} />
                                             {file.fileName}
                                             <button onClick={() => handleFileDownload(file.fileId)}>Download</button>
                                             </div>
                                         </li>
                                        ))}
                                    </ul>
                                )}
                                {folder.subFolders.length > 0 && renderFolders(folder.subFolders)}
                                
                            </>
                        )}
                    </ul>
                ))}
            </ul>
        );
    };

    return (
        <div>
            <h2>Directory</h2>
            <input type="file" onChange={(e)=>handleFileUpload(e,selectedFolderId ?? -1)} />
            <button onClick={createFolder}>Create Folder</button>
            <input
                    type="text"
                    placeholder="Enter folder name"
                    value={newFolderName}
                    onChange={handleChange}
                    onKeyPress={handleKeyPress}
                />
            {renderFolders(folders)}
        </div>
    );
};

export default Directory;
