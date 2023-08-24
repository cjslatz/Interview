import React from 'react';

function DocumentTable({ rows, onDelete }) {
    return (
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
                            <button onClick={() => onDelete(index)}>Delete</button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

export default DocumentTable;
