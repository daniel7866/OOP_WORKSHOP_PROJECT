import React from "react";
import "../Styles/Post.css";

const Post = (props) => {

    return (
        <div className="post-container">
            <h5><a href={`/profile/${props.userId}`}>map from uid={props.userId} to username</a></h5>
            <img className="post-image" src={props.imagePath} width="300" height="300" />
            <p>{props.description}</p>
            {props.ownedByLoggedUser ? <button className="btn btn-outline-danger">Remove</button> : null}
        </div>
    );
};

export default Post;