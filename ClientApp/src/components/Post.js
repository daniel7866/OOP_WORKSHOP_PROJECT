﻿import React from "react";
import "../Styles/Post.css";
import "../Styles/Images.css"
import { Link } from 'react-router-dom';
import { getAddress } from "../Services";

const Post = (props) => {
    const deleteHandler = () => {
        if (!window.confirm("Are you sure you want to delete this post?")) {
            return;
        }
        var requestOptions = {
            method: 'DELETE',
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/deletepost/${props.id}`, requestOptions)
            .then(response => props.setRefresh(x=>!x))
            .catch(error => console.log('error', error));
    }
    return (
        <div className="post-container">
            <div className="post-top" style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                <div className="image-cropper tiny">
                    <img className="profile-image" src={props.user.imagePath} />
                </div>
                    <h5><Link to={`/profile/${props.user.id}`}>{props.user.name}</Link></h5>
            </div>
            <p>{ props.datePosted }</p>
            <div className="post-image-container">
                <img className="post-image" src={props.imagePath} width="300" height="300" />
            </div>
            <p>{props.description}</p>
            <div className="post-bottom-container">
                {props.ownedByLoggedUser ? <button className="btn btn-outline-danger" onClick={deleteHandler} ><span style={{ fontSize: "xx-small" }}>Remove</span></button> : null}
            </div>
        </div>
    );
};

export default Post;