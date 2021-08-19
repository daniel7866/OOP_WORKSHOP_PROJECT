import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { login, logout } from "../actions";
import { useState } from "react";

const Login = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const loginHandler = (e) => {
        e.preventDefault();
        dispatch(login());
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
            <div id="emailHelp" class="form-text">Don't have an account? Create one <a href="/register">here</a></div>
            <button type="submit" class="btn btn-primary mb-3" onClick={(e) => { loginHandler(e); }}>Login</button>
        </form>
        );
};

export default Login;