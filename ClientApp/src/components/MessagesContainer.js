import React, {useEffect, useState} from "react";
import { getAddress } from "../Services";
import "../Styles/Message.css";

// a simple message component which displays the message content with css styles:
// messages from logged user are displayed on the right in green
// messages from other users are displayed on the left in gray
const Message = (props) => {
    return (
        <div className={`message ${(props.loggedUserId === props.message.senderId)?"right":"left"}`}>
            <p className="message-time">{props.message.dateSent}</p>
            <h6>{props.message.messageContent}</h6>
        </div>
    )
}

/**
 * This component displays all the messages with a particular user
 */
const MessagesContainer = (props) => {
    const [text, setText] = useState(''); // state for sending a new message
    
    const sendMessageHandler = ()=>{
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
        "receiverId": props.messages[0].senderId!==props.loggedUserId?props.messages[0].senderId:props.messages[0].receiverId,
        "messageContent": text
        });

        var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        setText('');

        fetch(`${getAddress()}/api/user/message`, requestOptions)
        .then(response => response.json())
        .then(result => props.fetchMessages()) // after sending a message refresh messages
        .catch(error => console.log('error', error));
    }

    useEffect(()=>{
        props.fetchMessages();
    },[]); // fetch messages when component mounts

    // scroll to the bottom automatically to view the most recent message
    useEffect(()=>{
        let messageContainerDiv = document.getElementById("messages-container1");
        messageContainerDiv.scrollTop = messageContainerDiv.scrollHeight;
    },[props.messages]);

    return (
        <div className="messages-container" id="messages-container1">
            {props.messages.map(message => <Message loggedUserId={props.loggedUserId} key={message.id} message={message} />)}
            <span style={{display: "flex"}}>
                <input type="text" className="form-control" placeholder="Type your message here" value={text} onChange={(e)=>setText(e.target.value)} />
                <button disabled={text.length==0} className="btn btn-outline-info" onClick={sendMessageHandler}>Send</button>
                <button className="btn btn-primary" onClick={props.fetchMessages} title="refresh" >↻</button>
            </span>
        </div>
    )
}

export default MessagesContainer;