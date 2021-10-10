import React from 'react';
import ProfileListItem from './ProfileListItem';
import "../Styles/Comments.css";
import { getAddress } from '../Services';

const Comment = (props) => {
    
    const removeComment = () =>{
        console.log(`commentId=${props.id}`)
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

    return (
        <div className="comment-container">
            <ProfileListItem id={props.userId} name={props.name} imagePath={props.imagePath} />
            <p>{props.body}</p>
            {props.ownedByUser?<button className="btn btn-danger" onClick={removeComment}>X</button>:null}
        </div>
    );
}

export default Comment;