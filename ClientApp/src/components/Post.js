import React, { useState } from "react";
import "../Styles/Post.css";
import "../Styles/Images.css"
import { Link } from 'react-router-dom';
import { getAddress } from "../Services";
import { useSelector } from "react-redux";
import Popup from "./Popup";
import LikesList from "./LikesList";
import CommentsList from "./CommentsList";

//like post - logic
const likePost = (postId,setLikes) => {
    var requestOptions = {
        method: 'POST',
        redirect: 'follow'
    };

    fetch(`${getAddress()}/api/post/like/id/${postId}`, requestOptions)
        .then(result => {
            fetch(`${getAddress()}/api/post/id/${postId}`)
                .then(response => response.json())
                .then(result => setLikes(result.likes))
        })
        .catch(error => console.log('error', error));
}

//unlike post - logic
const unlikePost = (postId, setLikes) => {
    var requestOptions = {
        method: 'DELETE',
        redirect: 'follow'
    };

    fetch(`${getAddress()}/api/post/unlike/id/${postId}`, requestOptions)
        .then(result => {
            fetch(`${getAddress()}/api/post/id/${postId}`)
                .then(response => response.json())
                .then(result => setLikes(result.likes))
        })
        .catch(error => console.log('error', error));
}

// like/unlike post - button
const PostLikeButton = (props) => {
    const user = useSelector(state => state.user);

    // if user does not like the post - display a like button
    // otherwise - display an unlike button
    if (props.likes.indexOf(user.uid) < 0) {
        return (
            <button className="btn btn-outline-info" title="like" onClick={() => { likePost(props.postId, props.setLikes) }}>👍🏻</button >
        );
    }
    else {
        return (
            <button className="btn btn-outline-info" title="unlike" onClick={() => { unlikePost(props.postId, props.setLikes) }}>👎🏻</button>
        );
    }

    return null;
}

/**
 * Post component:
 * A post will have:
 *  Post owner (user name, link and profile picture)
 *  Time of post
 *  Image
 *  Text
 * Few buttons including:
 *  Delete, like and comment
 */
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

    const reportPost = () => {
        if(!window.confirm("You are about to report this post as inappropriate, do you wish to continue?"))
            return;
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        var raw = JSON.stringify({
        "postId": props.id
        });

        var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/report/post`, requestOptions)
        .then(response => alert("Thank you for your report, our team will review it."))
        .catch(error => console.log('error', error));
    }

    //likes
    const [likes, setLikes] = useState(props.likes);
    const [likesPopupTrigger, setLikesPopupTrigger] = useState(false);

    //comments
    const [comments, setComments] = useState(props.comments);
    const [commentsPopupTrigger, setCommentsPopupTrigger] = useState(false);

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
                {props.ownedByLoggedUser ? <button className="btn btn-outline-danger" title="Remove post" onClick={deleteHandler} ><span >🗑</span></button> : null}
                {props.ownedByLoggedUser?null:<button className="btn btn-outline-danger" title="Report post" onClick={reportPost}><span >❕</span></button>}
                <PostLikeButton postId={props.id} likes={likes} setLikes={setLikes} />
                
                <button className="btn btn-outline-primary" onClick={()=>setLikesPopupTrigger(true)} >{likes.length == 1 ? likes.length + " like" : likes.length + " likes"}</button>
                <Popup trigger={likesPopupTrigger} >
                    <LikesList likes={likes} setTrigger={setLikesPopupTrigger} />
                </Popup>
                
                <button className="btn btn-outline-primary" onClick={()=>setCommentsPopupTrigger(true)} >{comments.length == 1 ? comments.length + " comment" : comments.length + " comments"}</button>
                <Popup trigger={commentsPopupTrigger} >
                    <CommentsList postId={props.id} comments={comments} setComments={setComments} setTrigger={setCommentsPopupTrigger} />
                </Popup>
            </div>
        </div>
    );
};

export default Post;