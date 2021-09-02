import React from "react";
import "../Styles/Post.css";

const Post = (props) => {

    return (
        <div className="post-container">
            <h5><a href={`/profile/${props.userId}`}>{props.userId}</a></h5>
            <div className="post-image-container">
                <img className="post-image" src={props.imagePath} width="300" height="300" />
            </div>
            <p>{props.description}</p>
            <div className="post-bottom-container">
                {props.ownedByLoggedUser ? <button className="btn btn-outline-danger"><span style={{ fontSize: "xx-small"}}>Remove</span></button> : null}
            </div>
        </div>
    );
};

export default Post;