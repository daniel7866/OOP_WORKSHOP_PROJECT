import React from 'react';

const Tab = (props) => {
    return (
        <div>
            {props.isSelected?props.children:null}
        </div>
    );
}

export default Tab;