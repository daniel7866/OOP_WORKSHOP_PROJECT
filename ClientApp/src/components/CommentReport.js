import React from "react";
import ProfileListItem from "./ProfileListItem";

import "../Styles/Report.css";

const CommentReport = (props) => {

    return (
        <div className="comment-container report" style={{textAlign: "center"}}>
            <div style={{display: "flex", flexDirection: "row", padding: "2rem"}}>
                <h5>Number of reports: {props.count}</h5>
                <button className="btn btn-danger" onClick={()=>props.closeCommentReport(props.commentId,true)}>Remove Comment</button>
                <button className="btn btn-success" onClick={()=>props.closeCommentReport(props.commentId,false)}>Keep Comment</button>
            </div>
            <ProfileListItem id={props.userId} name={props.name} imagePath={props.imagePath} />
            <p>{props.body}</p>
        </div>
    );
}

export default CommentReport;