import React, { useEffect, useState } from 'react';
import { useHome } from "../hooks/useHome";
import Post from "./Post";
import AddPost from "./AddPost";
import { useSelector } from 'react-redux';
import "../Styles/Profile.css";
import "../Styles/Images.css";

/** 
 * This component represents the home page that holds the posts' feed of the logged in user
*/
const Home = () => {
    const [feed, label, fetchAll] = useHome(); // custom hook for fetching posts
    const [refresh, setRefresh] = useState(false); // a flag for updating components

    const user = useSelector(state => state.user); // use redux to get logged user id

    useEffect(() => { fetchAll() }, [refresh, user]); // fetch feed everytime there is a change of state

    return (
        <div className="profile-post-container">
            <AddPost refresh={refresh} setRefresh={setRefresh} />
            {feed.map(p => (<Post key={p.id} id={p.id} user={p.user} description={p.description} datePosted={p.datePosted} likes={p.likes}
                imagePath={p.imagePath} comments={p.comments} ownedByLoggedUser={user.uid === p.user.id} setRefresh={setRefresh} />))}
            <h1>{ label }</h1>
        </div>
    );
};

export default Home;