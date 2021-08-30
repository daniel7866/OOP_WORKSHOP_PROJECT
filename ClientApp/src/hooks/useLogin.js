import React, { useState } from "react";
import { login } from "../actions";
import { useSelector, useDispatch } from "react-redux";
import { getAddress } from "../Services";


    /**
     * This code will try to get the user from a token saved on cookie, if succeded - user can surf the site, else - needs to login with the form below
     * */
export const useFirstLogin = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();
    const [flag, setFlag] = useState(true);
    if (flag) {
        var myHeaders = new Headers();

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/user/getUser`, requestOptions)
            .then(response => response.json())
            .then(result => { dispatch(login(result)); })
            .catch(error => console.log('error', error));
        setFlag(false);
    }
}

export const useLogin = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const [label, setLabel] = useState('');

    const loginHandler = (e) => {
        e.preventDefault();
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
            "Email": email,
            "Password": password
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/user/login`, requestOptions)
            .then(response => response.json())
            .then(result => { console.log(result); setLabel(result.message); dispatch(login(result)); })
            .catch(error => null);

    }

    return [email, setEmail, password, setPassword, label, loginHandler];
}