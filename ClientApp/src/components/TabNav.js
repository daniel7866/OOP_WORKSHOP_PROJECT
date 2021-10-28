import React, {useState} from 'react';

const TabItem = (props) => {
    return (
        <span>
            <div className="image-cropper tiny">
                <img className="profile-image" src={props.imagePath} />
            </div>
                <h5>{props.name}</h5>
                {props.unread?<h5 style={{color: "red"}}>*</h5>:null}
            </span>
    );
}

const TabNav = (props) => {
    return (
        <div>
            <ul className="nav nav-tabs">
                {
                    props.tabs.map(tab => {
                        const active = tab.id === props.selected?'active':'';
                        return (
                            <li className="nav-item" key={ tab.id }>
                                <a className={`nav-link + ${active}`} onClick={()=>props.setSelected(tab.id)}>
                                    <TabItem active={active} name={tab.name} imagePath={tab.imagePath} unread={props.unread.indexOf(tab.id)>=0} />
                                </a>
                            </li>
                        );
                    })
                }
            </ul>
            {props.children}
        </div>
    );
}

export default TabNav;