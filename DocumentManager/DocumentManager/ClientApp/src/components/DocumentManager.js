import React, { useState, useEffect } from 'react';
import FileUploadForm from './FileUploadForm';
import DocumentTable from './DocumentTable';

function DocumentManager() {
    const [rows, setRows] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchData = async () => {
        setIsLoading(true);
        try {
            const response = await fetch('/document');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setRows(data);
        } catch (error) {
            setError(error);
        } finally {
            setIsLoading(false);
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
                setError('Failed to delete the file.');
            }
        } catch (error) {
            setError('Error deleting the file: ' + error.message);
        }
    };

    return (
        <div>
            <FileUploadForm
                onUploadSuccess={() => fetchData()}
                onError={(error) => setError(error)}
            />
            {isLoading ? (
                <p>Loading...</p>
            ) : error ? (
                <p>Error: {error.message}</p>
            ) : (
                <DocumentTable rows={rows} onDelete={handleDelete} />
            )}
        </div>
    );
}

export default DocumentManager;
