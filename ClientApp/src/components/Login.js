import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { login, logout } from "../actions";
import { useState } from "react";

const SERVER = "18.219.92.190";

const Login = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const [label, setLabel] = useState('');

    const ADDRESS = window.location.hostname == "localhost" ? "https://localhost:44306" : `http://${SERVER}`

    /**
     * This code will try to get the user from a token saved on cookie, if succeded - user can surf the site, else - needs to login with the form below
     * */
    const [flag, setFlag] = useState(true);
    if (flag) {
        var myHeaders = new Headers();

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            redirect: 'follow'
        };

        fetch(`${ADDRESS}/api/user/getUser`, requestOptions)
            .then(response => response.json())
            .then(result => { console.log(result); dispatch(login(result)); })
            .catch(error => console.log('error', error));
        setFlag(false);
    }

    /**
     * END OF CODE BLOCK
     * */

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

        fetch(`${ADDRESS}/api/user/login`, requestOptions)
            .then(response => response.json())
            .then(result => { console.log(result); dispatch(login(result)); setLabel(result.message); })
            .catch(error => null);
        
    }

    return (
        <form className="login-form">
            <h2>You need to log in:</h2>
            <div className="mb-3">
                <label for="inputmail1" className="form-label">Email address</label>
                <input type="email" className="form-control" id="inputmail1" value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            <div className="mb-3">
                <label for="inputpassword1" className="form-label">Password</label>
                <input type="password" className="form-control" id="inputpassword1" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div id="emailHelp" className="form-text">Don't have an account? Create one <a href="/register">here</a></div>
            <button type="submit" className="btn btn-primary mb-3" onClick={(e) => { loginHandler(e); }}>Login</button>
            <label>{ label }</label>
        </form>
        );
};

export default Login;