import React from "react";
import Post from "./Post";
import { useProfile } from "../hooks/useProfile";
import "../Styles/Profile.css";
import "../Styles/Images.css";

const ProfileListItem = (props) => {
    return (
        <div className="profile-list-item">
            <div className="image-cropper tiny">
                <img className="profile-image" src={props.imagePath} />
            </div>
            <a><h6>{props.name}</h6></a>
        </div>
    )
}

const Profile = () => {

    const [following, followers, posts, name, imagePath, isLoggedProfile] = useProfile();

    return (
        <div className="profile-container">
            <div className="profile-container-top">
                <div className="image-cropper small">
                    <img className="profile-image" src={imagePath} />
                </div>
                <h3>{name}</h3>
                {isLoggedProfile?null:<button className="btn btn-outline-info">Follow</button>}
            </div>
            <div className="profile-people-container">
                <div className="profile-follow-list">
                    <h6>Following:</h6>
                    {following.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                </div>
                <div className="profile-follow-list">
                    <h6>Followers:</h6>
                    {followers.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                </div>
            </div>
            <div className="profile-post-container">
                {posts.map(p => (<Post key={p.id} id={p.id} userId={p.userId} description={p.description}
                    imagePath={p.imagePath} ownedByLoggedUser={isLoggedProfile} />))}
            </div>
        </div>
    )
};

export default Profile;