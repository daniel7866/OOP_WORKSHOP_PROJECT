import React, {useEffect, useState} from 'react';
import {useMessages} from "../hooks/useMessages";
import { useSelector } from 'react-redux';
import TabNav from './TabNav';
import Tab from './Tab';
import "../Styles/Images.css";

const Messages = (props) => {
    const user = useSelector(state => state.user);
    const [messages, usersMessaged, label, fetchAll] = useMessages();

    useEffect(()=>{
        if(user != null && user.uid != null)
            fetchAll(user.uid);
    },[user]);
    const [selected, setSelected] = useState(null);

    return (
        <div>
            <h1>{label}</h1>
            <TabNav setSelected={setSelected} tabs={usersMessaged} selected={selected}>
                {usersMessaged.map(x => <Tab key={x.id} id={x.id} isSelected={x.id === selected} >
                    {<h1>this is the user's message window</h1>}
                </Tab>)}
            </TabNav>
        </div>
    );
}

export default Messages;