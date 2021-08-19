import React from "react";
import '../Styles/Popup.css';

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