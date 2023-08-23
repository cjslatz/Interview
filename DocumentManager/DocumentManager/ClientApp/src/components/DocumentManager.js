import React, { useState, useEffect } from 'react';

function DocumentManager() {
    const [rows, setRows] = useState([]);

    useEffect(() => {
        async function fetchData() {
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
        }
        fetchData();
    }, []);

    const handleCheckboxChange = (event, index) => {
        // Update the selected row's checkbox status
        const updatedRows = [...rows];
        updatedRows[index].selected = event.target.checked;
        setRows(updatedRows);
    };

    return (
        <div>
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
                                <input
                                    type="checkbox"
                                    checked={row.selected || false}
                                    onChange={(event) => handleCheckboxChange(event, index)}
                                />
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default DocumentManager;
