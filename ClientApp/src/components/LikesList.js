import React, { useState } from 'react';
import ProfileListItem from "./ProfileListItem";
import { getUsers } from "../Services";

const LikesList = (props) => {
    const [likesUsers, setLikesUsers] = useState([]);

    if(props.likes.length>0)
        getUsers(props.likes).then(list => setLikesUsers(list));//turn the likes list into a users list

    return (
        <div>
            <div style={{ position: "relative" }}>
                <button style={{ position: "absolute", top: 0, right: 0 }} className="btn btn-outline-danger" onClick={ ()=>props.setTrigger(false)} >X</button>
            </div>
            <div className="profile-follow-list">
                <h3>Users who likes this post:</h3>
                {likesUsers.map(r => <ProfileListItem key={r.id} id={r.id} name={r.name} imagePath={r.imagePath} />)}
                <h3>{props.likes.length == 0? "No likes to this post": ""}</h3>
            </div>
        </div>
        );
}

export default LikesList;