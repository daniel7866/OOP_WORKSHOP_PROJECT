import { useSelector, useDispatch } from "react-redux";
import React, { useState } from "react";
import { getAddress, getUsers } from "../Services";

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