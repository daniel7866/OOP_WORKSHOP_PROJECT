import React, { useState, useEffect } from 'react';
import { getAddress, takeLastUrlItem, getUsers } from "../Services";
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

    const uid = parseInt(takeLastUrlItem(window.location.pathname)); // user id of current profile showing based on url


    const fetchAll = () => {
        fetch(`${getAddress()}/api/user/id/${uid}`)
            .then(response => response.json())
            .then(result => {

                /*Get user's followers*/
                /*setFollowers(result.followers);*/
                getUsers(result.followers)
                    .then(response => setFollowers(response))

                /*setFollowers(people);*/
                getUsers(result.following)
                    .then(res => setFollowing(res));
                /*Get user's posts*/
                fetch(`${getAddress()}/api/post/user/id/${uid}`)
                    .then(response => response.json())
                    .then(result => setPosts(result));

                setName(result.name);
                setImagePath(result.imagePath);
            })
            .catch(error => console.log('error', error))
    }

    useEffect(() => {
        if (flag) {
            fetchAll();
        }
    }, [user.uid, uid]);

    const isLoggedUser = uid == user.uid; // this flag tells us if the profile component is the profile we are logged into or a different one

    return [following, setFollowing, followers, setFollowers, posts, name, imagePath, isLoggedUser, fetchAll];
}