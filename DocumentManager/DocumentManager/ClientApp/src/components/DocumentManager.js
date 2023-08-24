import React, { useState, useEffect } from 'react';

function DocumentManager() {
    const [rows, setRows] = useState([]);
    const [file, setFile] = useState(null);

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleUpload = async () => {
        if (file) {
            try {
                const formData = new FormData();
                formData.append('file', file);

                const response = await fetch('/document/upload', {
                    method: 'POST',
                    body: formData,
                });

                if (response.ok) {
                    fetchData();
                } else {
                    console.error('Failed to upload the file.');
                }
            } catch (error) {
                console.error('Error uploading the file:', error);
            }
        } else {
            console.error('No file selected for upload.');
        }
    };

    const fetchData = async () => {
        try {
            const response = await fetch('/document');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setRows(data);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleDelete = async (index) => {
        const updatedRows = [...rows];
        const fileIdToDelete = updatedRows[index].path;

        try {
            const response = await fetch(`/document?path=${encodeURIComponent(fileIdToDelete)}`, {
                method: 'DELETE',
            });

            if (response.ok) {
                updatedRows.splice(index, 1);
                setRows(updatedRows);
            } else {
                console.error('Failed to delete the file.');
            }
        } catch (error) {
            console.error('Error deleting the file:', error);
        }
    };

    return (
        <div>
            <div>
                <input type="file" accept=".pdf" onChange={handleFileChange} />
                <button onClick={handleUpload}>Upload</button>
            </div>

            <table className="custom-table">
                <thead>
                    <tr>
                        <th>View</th>
                        <th>Name</th>
                        <th>Path</th>
                        <th>Category</th>
                        <th>Select</th>
                    </tr>
                </thead>
                <tbody>
                    {rows.map((row, index) => (
                        <tr key={index}>
                            <td>
                                <a href={row.path} target="_blank" rel="noopener noreferrer">
                                    View
                                </a>
                            </td>
                            <td>{row.name}</td>
                            <td>{row.path}</td>
                            <td>{row.category}</td>
                            <td>
                                <button onClick={() => handleDelete(index)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default DocumentManager;
