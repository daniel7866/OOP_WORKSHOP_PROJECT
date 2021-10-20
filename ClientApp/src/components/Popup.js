import React from "react";
import '../Styles/Popup.css';

/**
 * This is a popup component that holds another component inside of it.
 * It is used to show components in a new window on top of the application.
 */
const Popup = (props) => {
    return props.trigger ? (
        <div className="popup">
            <div className="popup-inner">
                {props.children}
            </div>
        </div>
    ): null;
};

export default Popup;