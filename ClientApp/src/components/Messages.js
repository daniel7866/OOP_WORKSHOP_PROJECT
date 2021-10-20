import React, {useEffect, useState} from 'react';
import {useMessages} from "../hooks/useMessages";
import { useSelector } from 'react-redux';
import TabNav from './TabNav';
import Tab from './Tab';
import "../Styles/Images.css";
import MessagesContainer from './MessagesContainer';

/** 
 * This component is the messages page.
 * It contains tabs of all active conversation(tab for each user)
 * When clicking on a tab it will display a 'MessageContainer' component in which it will show all messages with this user
*/
const Messages = (props) => {
    const user = useSelector(state => state.user);//get logged user from redux state

    //custom hook for fetching messages
    //messagedUsers-users we have messages with
    const [messages, messagedUsers, label, fetchMessagedUsers, fetchMessagesWithUser] = useMessages();

    useEffect(()=>{
        if(user != null && user.uid != null)
            fetchMessagedUsers();
    },[user]); // fetch the users after loading redux state

    const [selected, setSelected] = useState(null); // a state for tab selection

    return (
        <div>
            <h1>{label}</h1>
            <TabNav setSelected={setSelected} tabs={messagedUsers} selected={selected}>
                {messagedUsers.map(x => <Tab key={x.id} id={x.id} isSelected={x.id === selected} >
                    {<MessagesContainer fetchMessages={()=>fetchMessagesWithUser(selected)} loggedUserId={user.uid} messages={messages} />}
                </Tab>)}
            </TabNav>
            {(messagedUsers.length>0&&selected===null)?<h4>Click on a tab to view your message history with someone!</h4>:null}
            {messagedUsers.length===0?<h4>You have not yet started a conversation. To send someone a message go to their profile and click on message.</h4>:null}
        </div>
    );
}

export default Messages;