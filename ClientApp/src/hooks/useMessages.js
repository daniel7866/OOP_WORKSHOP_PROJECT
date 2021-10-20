import { useSelector, useDispatch } from "react-redux";
import React, { useState } from "react";
import { getAddress, getUsers } from "../Services";

/**
 * This custom hook will fetch messages with users.
 * It has two methods:
 *      fetchMessagedUsers() - get users with whom we have messages with
 *      fetchMessagesWithUser() - get messages we have with a particular user
 * 
 * This is so we don't load all messages at once from all users.
 * We just load the users we are conversating with, and load only messages with the user we want to talk to
 */
export const useMessages = () => {
    const [messages, setMessages] = useState([]);
    const [messagedUsers, setMessagedUsers] = useState([]);
    const [label, setLabel] = useState('');

    const fetchMessagedUsers = () => {
        var myHeaders = new Headers();
    
        var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        };
    
        fetch(`${getAddress()}/api/user/messages/users`, requestOptions)
        .then(response => response.json())
        .then(users => {
            if(users.length == 0){
                setLabel("No conversations");
            }else{
                setMessagedUsers(users);
            }
        })
        .catch(error => console.log('error', error));
    }

    const fetchMessagesWithUser = (userId) =>{
        var myHeaders = new Headers();
    
        var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        };
    
        fetch(`${getAddress()}/api/user/messages/user/${userId}`, requestOptions)
        .then(response => response.json())
        .then(messages => {
            if(messages.length == 0){
                setLabel("No messages");
            }else{
                setMessages(messages);
            }
        })
        .catch(error => console.log('error', error));
    }

    return [messages, messagedUsers, label, fetchMessagedUsers, fetchMessagesWithUser];
}