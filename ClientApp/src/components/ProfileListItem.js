import React from 'react';
import { Link } from 'react-router-dom';
import "../Styles/Profile.css";
import "../Styles/Images.css";


/**
 * This component shows a profile in a list
 * We have the profile picture, and next to it we have the name with a link to the profile
 */
const ProfileListItem = (props) => {
    return (
        <div className="profile-list-item">
            <div className="image-cropper tiny">
                <img className="profile-image" src={props.imagePath} />
            </div>
            <Link to={`/profile/${props.id}`}>{props.name}</Link>
        </div>
    )
}

export default ProfileListItem;