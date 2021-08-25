import React from "react";
import "../Styles/Post.css";

const Post = (props) => {

    return (
        <div className="post-container">
            <h3><a>map from uid={props.userId} to username</a></h3>
            <img className="post-image" src={props.imagePath} width="300" height="300" />
            <p>{props.description}</p>
        </div>
    );
};

export default Post;