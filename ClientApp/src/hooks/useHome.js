import React, { useState } from "react";
import { getAddress } from "../Services";


/**
 * This custom hook will load all the posts in the user feed and store the result in a state
 */
export const useHome = () => {
    const [feed, setFeed] = useState([]);
    const [label, setLabel] = useState('');

    const fetchAll = () => { //fetch all the posts in the user's feed
        fetch(`${getAddress()}/api/post/feed`)
            .then(response => response.json())
            .then(result => {
                if (result.length == 0) {
                    setLabel("Nothing to show. Upload a post or follow people to view their posts here!");
                }
                else if(result.length>0){
                    setLabel("");
                    setFeed(result);
                }
            })
            .catch(error => console.log('error', error));
    }

    return [feed, label, fetchAll];
}