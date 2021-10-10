import React, { useState } from 'react';
import ProfileListItem from "./ProfileListItem";
import { getUsers } from "../Services";

const LikesList = (props) => {
    return (
        <div>
            <div style={{ position: "relative" }}>
                <button style={{ position: "absolute", top: 0, right: 0 }} className="btn btn-outline-danger" onClick={ ()=>props.setTrigger(false)} >X</button>
            </div>
            <div className="profile-follow-list">
                {props.comments.map(r => 
                <div>
                    <ProfileListItem key={r.userId} id={r.userId} name={r.userName} imagePath={r.userImagePath} />
                    <h1>{r.body}</h1>
                </div>)}
                <h3>{props.comments.length == 0? "No comments to this post": ""}</h3>
            </div>
        </div>
        );
}

export default LikesList;