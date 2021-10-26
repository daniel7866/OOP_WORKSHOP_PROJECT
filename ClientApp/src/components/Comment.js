import React from 'react';
import ProfileListItem from './ProfileListItem';
import "../Styles/Comments.css";
import { getAddress } from '../Services';

/** 
 * This component is a comment on a post.
 * A comment is simply text.
*/
const Comment = (props) => {
    
    const removeComment = () =>{
        var myHeaders = new Headers();

        var requestOptions = {
        method: 'DELETE',
        headers: myHeaders,
        redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/comment/${props.id}`, requestOptions)
        .then(response => props.getComments())
        .catch(error => console.log('error', error));
            }

    const reportComment = () => {
        if(!window.confirm("You are about to report this comment as inappropriate, do you wish to continue?"))
            return;
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        var raw = JSON.stringify({
        "commentId": props.id,
        "postId": props.postId
        });

        var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/report/comment`, requestOptions)
        .then(response => alert("Thank you for your report, our team will review it."))
        .catch(error => console.log('error', error));
    }

    return (
        <div className="comment-container" style={{textAlign: "center"}}>
            <ProfileListItem id={props.userId} name={props.name} imagePath={props.imagePath} />
            <p>{props.body}</p>
            {props.ownedByUser?<button className="btn btn-outline-danger" title="Remove comment" onClick={removeComment}><span >🗑</span></button>:null}
            {props.ownedByUser?null:<button className="btn btn-outline-danger" title="Report comment" onClick={reportComment}><span >❕</span></button>}
        </div>
    );
}

export default Comment;