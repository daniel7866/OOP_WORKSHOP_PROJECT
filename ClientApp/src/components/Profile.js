import React from "react";
import Post from "./Post";
import "../Styles/Profile.css";

const ProfileListItem = (props) => {
    return (
        <div className="profile-list-item">
            <div className="image-cropper tiny">
                <img className="profile-image" src={props.imagePath} />
            </div>
            <a><h5>{props.name}</h5></a>
        </div>
    )
}

const Profile = (props) => {


    return (
        <div className="profile-container">
            <div className="profile-container-top">
                <div className="image-cropper small">
                    <img className="profile-image" src={props.imagePath} />
                </div>
                <h3>{props.name}</h3>
            </div>
            <div className="profile-people-container">
                <div className="profile-follow-list">
                    <h3>Following:</h3>
                    {props.followers.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                </div>
                <div className="profile-follow-list">
                    <h3>Followers:</h3>
                    {props.followers.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                </div>
            </div>
            <div className="profile-post-container">
                {props.posts.map(p => (<Post key={p.id} id={p.id} userId={p.userId} description={p.description}
                    imagePath={p.imagePath} />))}
            </div>
        </div>
    )
};

export default Profile;