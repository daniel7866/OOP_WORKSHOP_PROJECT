import React from "react";
import { Link } from 'react-router-dom';
import "../Styles/Post.css";
import "../Styles/Images.css"
import "../Styles/Report.css"

const PostReport = (props) => {
    
    return (
        <div className="post-container">
            <div style={{display: "flex", flexDirection: "row", padding: "2rem"}}>
                <h5>Number of reports: {props.count}</h5>
                <button className="btn btn-danger" onClick={()=>props.closePostReport(props.postId,true)}>Remove Post</button>
                <button className="btn btn-success" onClick={()=>props.closePostReport(props.postId,false)}>Keep Post</button>
            </div>
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
        </div>
    );
}

export default PostReport;