import React, { useState,useEffect } from "react";
import Post from "./Post";
import ProfileListItem from "./ProfileListItem";
import { useProfile } from "../hooks/useProfile";
import AddPost from "./AddPost";
import { takeLastUrlItem, getAddress, findIdInUserList, getUsers } from "../Services";
import Popup from "./Popup";
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

/**
 * 
 * This button will be revealed when visiting a different profile, when pressing it will show a popup of an input field to send a message to that particular user
 */
const ProfileMessageButton = ()=>{
    const uid = parseInt(takeLastUrlItem(window.location.pathname)); // user id of current profile showing based on url
    const [text, setText] = useState('');
    const [messagePopupTrigger, setMessagePopupTrigger] = useState(false);

    const sendMessageHandler = ()=>{
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
        "receiverId": uid,
        "messageContent": text
        });

        var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        setText('');

        fetch(`${getAddress()}/api/user/message`, requestOptions)
        .then(response => setMessagePopupTrigger(false))
        .catch(error => console.log('error', error));
    }

    return (
        <>
            <button className="btn btn-outline-success" onClick={()=>setMessagePopupTrigger(true)}>Message</button>
            <Popup trigger={messagePopupTrigger} >
                <>
                    <button className="btn btn-danger" onClick={()=>setMessagePopupTrigger(false)}>X</button>
                    <div>
                        <input type="text" className="form-control" placeholder="Type your message here" setTrigger={setMessagePopupTrigger} value={text} onChange={(e)=>setText(e.target.value)} />
                        <button className="btn btn-success" disabled={text.length==0} onClick={()=>{ sendMessageHandler();}}>Send</button>
                    </div>
                </>
                </Popup>
        </>
    )
}

const EditProfileDetails = (props) => {
    const [picture, setPicture] = useState(null);
    const [name, setName] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [repeatNewPassword, setRepeatNewPassword] = useState('');

    return (
        <div>
            <button className="btn btn-outline-danger" onClick={()=>props.setEditPopup(false)} >Close Window</button>
            <br />
            <label>Edit profile details:</label>
            <div>
                <div style={{margin: "1rem" , padding: "1rem", display: "flex", flexDirection: "column",  backgroundColor: "background-color: rgba(229, 229, 229, 0.8)", borderRadius: "1rem", boxShadow: "#282c34 0 0 4px 0", padding: "1px", alignItems: "center"}} >
                    <label>Change profile picture:</label>
                    <input type="file" className="form-control-file" style={{margin: "auto", width: "min-content"}} placeholder="New profile picture" value={picture} onChange={(e)=>setPicture(e.target.value)} />
                    <button className="btn btn-primary">Change profile picture</button>
                </div>
                <div style={{margin: "1rem" , padding: "1rem", display: "flex", flexDirection: "column",  backgroundColor: "background-color: rgba(229, 229, 229, 0.8)", borderRadius: "1rem", boxShadow: "#282c34 0 0 4px 0", padding: "1px", alignItems: "center"}} >
                    <label>Change profile name:</label>
                    <input type="text" className="form-control" placeholder="New name" value={name} onChange={(e)=>setName(e.target.value)} />
                    <button className="btn btn-primary">Change profile name</button>
                </div>
                <div style={{margin: "1rem" , padding: "1rem", display: "flex", flexDirection: "column",  backgroundColor: "background-color: rgba(229, 229, 229, 0.8)", borderRadius: "1rem", boxShadow: "#282c34 0 0 4px 0", padding: "1px", alignItems: "center"}} >
                    <label>Change password:</label>
                    <input type="password" className="form-control" placeholder="Old password" value={oldPassword} onChange={(e)=>setOldPassword(e.target.value)} />
                    <input type="password" className="form-control" placeholder="New password" value={newPassword} onChange={(e)=>setNewPassword(e.target.value)}/>
                    <input type="password" className="form-control" placeholder="Repeat new password" value={repeatNewPassword} onChange={(e)=>setRepeatNewPassword(e.target.value)}/>
                    <button className="btn btn-primary">Change password</button>
                </div>
            </div>
        </div>
    )
}

const EditProfileButton = () => {
    const [editPopup, setEditPopup] = useState(false);

    return (
        <>
            <button className="btn btn-danger" onClick={()=>setEditPopup(true)}>Edit Profile Details</button>
            <Popup trigger={editPopup} >
                <EditProfileDetails setEditPopup={setEditPopup} />
            </Popup>
        </>
    );
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
                <div style={isLoggedProfile?null:{backgroundColor: "rgba(229, 229, 229, 0.8)",borderRadius: "1rem",boxShadow: "#282c34 0 0 4px 0", padding: "1rem"}}>
                    {isLoggedProfile ? null : <ProfileFollowButton followers={followers} setFollowers={setFollowers} />}
                    {isLoggedProfile ? null : <ProfileMessageButton />}
                    {isLoggedProfile ? <EditProfileButton /> : null}
                </div>
            </div>
            <div className="profile-people-container">
                <div className="profile-follow-list">
                    <h6>Following:</h6>
                    {following.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                        {following.length===0?<h6>Not following anyone</h6>:null}
                </div>
                <div className="profile-follow-list">
                    <h6>Followers:</h6>
                    {followers.map(f =>
                        (<ProfileListItem key={f.id} id={f.id} name={f.name} imagePath={f.imagePath} />))}
                        {followers.length===0?<h6>No followers</h6>:null}
                </div>
            </div>
            <div className="profile-post-container">
                <>
                    {isLoggedProfile ? <AddPost refresh={refresh} setRefresh={setRefresh} />: null}
                </>
                {posts.map(p => (<Post key={p.id} id={p.id} user={p.user} description={p.description} datePosted={p.datePosted} likes={p.likes}
                    comments={p.comments} imagePath={p.imagePath} ownedByLoggedUser={isLoggedProfile} setRefresh={setRefresh} />))}
                    {posts.length===0?<h1>You have not yet uploaded any post</h1>:null}
            </div>
        </div>
    )
};

export default Profile;