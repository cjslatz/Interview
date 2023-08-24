import React, { useState } from 'react';

function FileUploadForm({ onUploadSuccess, onError }) {
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
                    onUploadSuccess();
                } else {
                    onError('Failed to upload the file.');
                }
            } catch (error) {
                onError('Error uploading the file: ' + error.message);
            }
        } else {
            onError('No file selected for upload.');
        }
    };

    return (
        <div className="file-upload-container">
            <input type="file" accept=".pdf" onChange={handleFileChange} className="file-input" />
            <button onClick={handleUpload} className="upload-button">
                Upload File
            </button>
        </div>
    );
}

export default FileUploadForm;
