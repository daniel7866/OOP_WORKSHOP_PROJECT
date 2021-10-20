import React, { useState } from "react";
import { login } from "../actions";
import { useSelector, useDispatch } from "react-redux";
import { getAddress } from "../Services";


    /**
     * This code will try to get the user from a token saved on cookie, if succeded - user can surf the site, else - needs to login with the form below.
     * The login will update the user in the global state in Redux
     * */
export const useFirstLogin = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();
    const [flag, setFlag] = useState(true);
    if (flag) { // make sure we run only once
        fetch(`${getAddress()}/api/user/token`)
            .then(response => response.json())
            .then(result => { dispatch(login(result)); })
            .catch(error => console.log('error', error));
        setFlag(false);
    }
}

/**
 * This custom hook will store the information the user is typing for login and send the request.
 * After a successfull login - the user state will be stored in Redux gloal state
 */
export const useLogin = () => {
    //redux state management
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const [label, setLabel] = useState(''); // label for error messages

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
            .then(result => { setLabel(result.message); dispatch(login(result)); })
            .catch(error => null);

    }

    return [email, setEmail, password, setPassword, label, loginHandler];
}