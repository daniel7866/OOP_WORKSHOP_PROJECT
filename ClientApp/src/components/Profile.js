import React, { useState,useEffect } from "react";
import Post from "./Post";
import ProfileListItem from "./ProfileListItem";
import { useProfile } from "../hooks/useProfile";
import AddPost from "./AddPost";
import { takeLastUrlItem, getAddress } from "../Services";
import "../Styles/Profile.css";
import "../Styles/Images.css";

import { useSelector, useDispatch } from "react-redux";

const followUser = (id) => {
    var requestOptions = {
        method: 'POST',
        redirect: 'follow'
    };

    fetch(`${getAddress()}/api/user/follow/${id}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}

const unfollowUser = (id) => {
    var requestOptions = {
        method: 'DELETE',
        redirect: 'follow'
    };

    fetch(`${getAddress()}/api/user/unfollow/${id}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}

const ProfileFollowButton = (props) => {
    const uid = parseInt(takeLastUrlItem(window.location.pathname)); // user id of current profile showing based on url
    const user = useSelector(state => state.user);
    const [following, setFollowing] = useState([]);
    const [flag, setFlag] = useState(false);

    useEffect(() => {
        if (user.uid !== null) {
            fetch(`${getAddress()}/api/user/id/${user.uid}`)
                .then(response => response.json())
                .then(result => { setFollowing(result.following); setFlag(true); })
                .catch(error => console.log('error', error));
        }
    }, [user, props.refresh]);

    if (flag) { // flag is set when all the asynchronus tasks finish
        if (following.indexOf(uid) < 0) {
            return (
                <button className="btn btn-outline-info" onClick={() => { followUser(uid); props.setRefresh(value => !value); }}> Follow</button >
            );
        }
        else {
            return (
                <button className="btn btn-outline-info" onClick={() => { unfollowUser(uid); props.setRefresh(value => !value); }}>Unfollow</button>
            );
        }
    }

    return null;
}

const Profile = () => {
    const [following, followers, posts, name, imagePath, isLoggedProfile, fetchAll] = useProfile();

    const [refresh, setRefresh] = useState(false);

    useEffect(() => {
        fetchAll();
    }
        ,[refresh]);

    return (
        <div className="profile-container">
            <div className="profile-container-top">
                <div className="image-cropper small">
                    <img className="profile-image" src={imagePath} />
                </div>
                <h3>{name}</h3>
                {isLoggedProfile ? null : <ProfileFollowButton setRefresh={setRefresh} />}
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
                <>
                    {isLoggedProfile ? <AddPost refresh={refresh} setRefresh={setRefresh} />: null}
                </>
                {posts.map(p => (<Post key={p.id} id={p.id} user={p.user} description={p.description} datePosted={p.datePosted} likes={p.likes}
                    imagePath={p.imagePath} ownedByLoggedUser={isLoggedProfile} setRefresh={setRefresh} />))}
            </div>
        </div>
    )
};

export default Profile;