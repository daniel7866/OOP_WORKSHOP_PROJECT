import React, { useEffect, useState } from 'react';
import { useHome } from "../hooks/useHome";
import Post from "./Post";
import AddPost from "./AddPost";
import { useSelector } from 'react-redux';
import "../Styles/Profile.css";
import "../Styles/Images.css";

const Home = () => {
    const [feed, label, fetchAll] = useHome();
    const [refresh, setRefresh] = useState(false);
    const user = useSelector(state => state.user);

    useEffect(() => { fetchAll() }, [refresh]);

    return (
        <div className="profile-post-container">
            <AddPost refresh={refresh} setRefresh={setRefresh} />
            {feed.map(p => (<Post key={p.id} id={p.id} user={p.user} description={p.description} datePosted={p.datePosted} likes={p.likes}
                imagePath={p.imagePath} ownedByLoggedUser={user.uid === p.id} setRefresh={setRefresh} />))}
            <h1>{ label }</h1>
        </div>
    );
};

export default Home;