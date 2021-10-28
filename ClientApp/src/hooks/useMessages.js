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
    const [unreadMessagedUsers, setUnreadMessagedUsers] = useState([]);
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
        .then(result => {
            if(result.users.length == 0 && result.unread.length == 0){
                setLabel("No conversations");
            }else{
                setMessagedUsers(result.users);
                setUnreadMessagedUsers(result.unread);
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

    const markMessagesAsRead = (senderId) => {
        var requestOptions = {
            method: 'POST',
            redirect: 'follow'
          };
          
          fetch(`${getAddress()}/api/user/messages/read/${senderId}`, requestOptions)
            .then(response => response.text())
            .then(result => setUnreadMessagedUsers(unreadMessagedUsers.filter(x=>x!=senderId)))
            .catch(error => console.log('error', error));
    }

    return [messages, messagedUsers, unreadMessagedUsers, label, fetchMessagedUsers, fetchMessagesWithUser, markMessagesAsRead];
}