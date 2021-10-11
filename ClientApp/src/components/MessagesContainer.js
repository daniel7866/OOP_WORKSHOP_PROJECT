import React, {useState} from "react";
import { getAddress } from "../Services";
import "../Styles/Message.css";

const Message = (props) => {
    return (
        <div className={`message ${(props.loggedUserId === props.message.senderId)?"right":"left"}`}>
            <p>{props.message.messageContent}</p>
        </div>
    )
}

const MessagesContainer = (props) => {
    const [text, setText] = useState('');
    
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
        .then(result => props.fetchAll())
        .catch(error => console.log('error', error));
    }

    return (
        <div className="messages-container" id="messages-container1">
            {props.messages.map(message => <Message loggedUserId={props.loggedUserId} key={message.id} message={message} />)}
            <span style={{display: "flex"}}>
                <input type="text" className="form-control" placeholder="Type your message here" value={text} onChange={(e)=>setText(e.target.value)} />
                <button disabled={text.length==0} className="btn btn-outline-info" onClick={sendMessageHandler}>Send</button>
            </span>
        </div>
    )
}

export default MessagesContainer;