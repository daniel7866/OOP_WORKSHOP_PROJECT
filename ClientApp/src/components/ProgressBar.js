﻿import React from "react";


/**
 * This component is a progress bar for uploading new images
 */
const ProgressBar = (props) => {
    const { bgcolor, completed } = props;

    const containerStyles = {
        height: 20,
        width: '33%',
        backgroundColor: "#e0e0de",
        borderRadius: 50,
        margin: 2
    }

    const fillerStyles = {
        height: '100%',
        width: `${completed}%`,
        backgroundColor: bgcolor,
        borderRadius: 'inherit',
        textAlign: 'right',
        transition: 'width 1s ease-in-out',
    }

    const labelStyles = {
        padding: 5,
        color: 'white',
        fontWeight: 'bold'
    }

    return (
        <div style={containerStyles}>
            <div style={fillerStyles}>
                <span style={labelStyles} > {`${completed}%`}</span>
            </div>
        </div>
    );
};

export default ProgressBar;