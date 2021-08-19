import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { login, logout } from "../actions";


const Login = () => {
    const user = useSelector(state => state.user);
    const dispatch = useDispatch();

    const loginHandler = (e) => {
        e.preventDefault();
        dispatch(login());
    }

    return (
        <form className="login-form">
            <h2>Login</h2>
            <label >Email:</label>
            <input type="text" placeholder="Enter Email Address" />
            <br/>
            <label >Password:</label>
            <input type="password" placeholder="Password" />
            <br />
            <button type="submit" class="btn btn-primary mb-3" onClick={(e) => { loginHandler(e); }}>Login</button>
        </form>
        );
};

export default Login;