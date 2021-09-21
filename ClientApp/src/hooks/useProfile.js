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

    const people = [{ id: 0, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 1, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 2, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 3, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 4, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 5, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 6, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 7, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 8, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    { id: 9, name: "jesus", imagePath: "https://media.istockphoto.com/photos/smiling-man-in-the-living-room-is-taking-a-selfie-picture-id813935198?k=6&m=813935198&s=612x612&w=0&h=1DSgYH-eugIriZB1d5CKg5yhUpdwpeU-cl4iXqOfVQg=" },
    ];

    const postsList = [
        { id: 0, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" },
        { id: 1, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" },
        { id: 2, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" },
        { id: 3, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" },
        { id: 4, userId: 50, description: "This is a post", imagePath: "https://www.birdlife.org/sites/default/files/styles/full_1140x550/public/news/shutterstock_1451653292_1_1.jpg?itok=BWagqmnZ" }
    ];

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
        console.log("FETCHED");
    }

    useEffect(() => {
        if (flag) {
            fetchAll();
        }
    }, [user.uid, uid]);

    const isLoggedUser = uid == user.uid; // this flag tells us if the profile component is the profile we are logged into or a different one

    return [following, setFollowing, followers, setFollowers, posts, name, imagePath, isLoggedUser, fetchAll];
}