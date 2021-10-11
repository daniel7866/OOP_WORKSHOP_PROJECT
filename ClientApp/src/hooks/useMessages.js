import { useSelector, useDispatch } from "react-redux";
import React, { useState } from "react";
import { getAddress, getUsers } from "../Services";


/**
 * This method gets a list of jsons representing the messages this logged user is recieving or sending
 * It will extract all the users this user is interacting with through private messages
 */
const getUsersList = (messagesList, loggedUserId) => {
    let a = [];
    messagesList.forEach(element => {
        if(element.senderId != loggedUserId){
            if(a.indexOf(element.senderId)<0){
                a.push(element.senderId);
            }
        }else if(element.receiverId != loggedUserId){
            if(a.indexOf(element.receiverId)<0){
                a.push(element.receiverId);
            }
        }
    });
    return getUsers(a);
}

export const useMessages = () => {
    const [messages, setMesssages] = useState([]);
    const [usersMessaged, setUsersMessaged] = useState([]);
    const [label, setLabel] = useState('');

    const fetchAll = (loggedUserId) => {
        var myHeaders = new Headers();
    
        var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
        };
    
        fetch(`${getAddress()}/api/user/messages/`, requestOptions)
        .then(response => response.json())
        .then(messages => {
            if(messages.length == 0){
                setLabel("No messages");
            }else{
                setMesssages(messages);
                getUsersList(messages, loggedUserId).then(usersMessaged => setUsersMessaged(usersMessaged));
            }
        })
        .catch(error => console.log('error', error));
    }

    return [messages, usersMessaged, label, fetchAll];
}