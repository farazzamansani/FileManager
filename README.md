# FileManager

<h1>How to run</h1>
1. Create the database,  database is EF Code first, you can create by CDing to the directory FarazTechTest\FileDatabase and running "dotnet ef database update"
2. Run the api, open FarazTechTest.sln in Visual Studio, You can then simply run the solution from the top (https)
3. Run the react app, cd to WebApp\file-manager-app and run "npm install" followed by "npm start".

Everything should now be running, there is no default data in the the db, but you are right to start creating folders and then from there files/subfolders.

<h1>Database Schema</h1>
Super simple, 2 tables
<h3>folders </h3>
folderId, name, parentFolder<br>
folder must have an PK id and a name,
optionally has a parent folder (root folders won't have a parent).
<h3>files</h3>
fileId, name, folderId, fileData, fileType, uploadedDateTime <br>
File must have a PK id,
must have fileData (we are storing the file as bytes)
we have name and filetype so we can get this metadata without having to process the raw file data.
and uploadedDateTime to just stamp the upload date on.

<h2>Better design</h2>
Became clear it would have been better having the fileType its own table of options and having a key to it from the file, this would be more normalised and allow the api to make decisions about allowed file types based on that table instead of using the current hardcoded list. Didn't persue this due to time constraints.

<h1>Limitations</h1>
Supports infinite nesting of the folder structure, design involves just handling the entire directory to the UI to handle and present, not going to perform well if it were subject to massive folder structures.<br>
No renaming functionality.<br>
No file previewing functionality (must download the files and handle them to see them).<br>
I opted to simply have the 1 component acting as both the hierarchical view and the file display instead of splitting them up exactly the same as the wireframe view.

<h1>Future Development plans</h1>
Define actual requirements/Nice to haves. <br>
Consider a more lazy loaded approach to retrieving/displaying/managing the folder structure.<br>
Consider storing files somewhere more appropriate such as Azure Blobs etc instead of directly in DB.<br>
More polish including bringing more values out into configs.

