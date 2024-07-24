import File from '../models/File';

interface Folder {
    folderId: number;
    name: string;
    subFolders: Folder[];
    files: File[];
    isOpen: boolean;
}

export default Folder;