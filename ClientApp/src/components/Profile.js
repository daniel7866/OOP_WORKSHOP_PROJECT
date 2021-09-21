import React, { useState,useEffect } from "react";
import Post from "./Post";
import ProfileListItem from "./ProfileListItem";
import { useProfile } from "../hooks/useProfile";
import AddPost from "./AddPost";
import { takeLastUrlItem, getAddress, findIdInUserList, getUsers } from "../Services";
import "../Styles/Profile.css";
import "../Styles/Images.css";

import { useSelector, useDispatch } from "react-redux";

const followUser = (id) => {
    var requestOptions = {
        method: 'POST',
        redirect: 'follow'
    };

    return fetch(`${getAddress()}/api/user/follow/${id}`, requestOptions)
        .then(response => response.text())
        .catch(error => console.log('error', error));
}

const unfollowUser = (id) => {
    var requestOptions = {
        method: 'DELETE',
        redirect: 'follow'
    };

    return fetch(`${getAddress()}/api/user/unfollow/${id}`, requestOptions)
        .then(response => response.text())
        .catch(error => console.log('error', error));
}

const ProfileFollowButton = (props) => {
    const uid = parseInt(takeLastUrlItem(window.location.pathname)); // user id of current profile showing based on url
    const user = useSelector(state => state.user);
    const [flag, setFlag] = useState(false); //flag is true if i'm already follow this user

    useEffect(() => {
        if (findIdInUserList(props.followers, user.uid) < 0) // if you're not following him => flag is false
            setFlag(false);
        else
            setFlag(true);
            }, [props.followers]);//check the flag each time the followers changed
    if (!flag) {//if not following - show a follow button
        return (
            <button className="btn btn-outline-info" onClick={() => {
                followUser(uid)
                    .then(res => {
                        fetch(`${getAddress()}/api/user/id/${uid}`)
                            .then(response => response.json())
                            .then(result => { getUsers(result.followers).then(res=>props.setFollowers(res)); })
                });
            }}> Follow</button >
        );
    }
    else {//if following - show an unfollow button
        return (
            <button className="btn btn-outline-info" onClick={() => {
                unfollowUser(uid)
                    .then(res => {
                        fetch(`${getAddress()}/api/user/id/${uid}`)
                            .then(response => response.json())
                            .then(result => { getUsers(result.followers).then(res => props.setFollowers(res)); })
                    });
            }}> Unfollow</button >
        );
    }

    return null;
}

const Profile = () => {
    const [following, setFollowing, followers, setFollowers, posts, name, imagePath, isLoggedProfile, fetchAll] = useProfile();

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
                {isLoggedProfile ? null : <ProfileFollowButton followers={followers} setFollowers={setFollowers} />}
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