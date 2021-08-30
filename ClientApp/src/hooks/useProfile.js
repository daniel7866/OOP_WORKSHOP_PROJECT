import React, { useState, useEffect } from 'react';
import { getAddress } from "../Services";
import { useSelector, useDispatch } from "react-redux";


export const useProfile = () => {
    const user = useSelector(state => state.user);

    const [following, setFollowing] = useState([]);
    const [followers, setFollowers] = useState([]);
    const [posts, setPosts] = useState([]);
    const [name, setName] = useState('');
    const [imagePath, setImagePath] = useState('');

    const [flag, setFlag] = useState(false);
    if (!flag && user.uid != null) {
        setFlag(true);
    }

    useEffect(() => {
        if (flag) {
            fetch(`${getAddress()}/api/user/id/${user.uid}`)
                .then(response => response.json())
                .then(result => {
                    setFollowing(result.following);
                    setFollowers(result.followers);
                    setPosts([{ id: 0, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" }]);//need to get user's post
                    setName(result.name);
                    setImagePath(result.imagePath);
                })
                .catch(error => console.log('error', error))
        }
    }, [user.uid]);

    return [following, followers, posts, name, imagePath];
}