import React from "react";
import { useFirstLogin, useLogin } from "../hooks/useLogin";

const Login = () => {
    useFirstLogin();//first try to login automatically if possible

    const [email, setEmail, password, setPassword, label, loginHandler] = useLogin();

    return (
        <form className="login-form">
            <h2>You need to log in:</h2>
            <div className="mb-3">
                <label htmlFor="inputmail1" className="form-label">Email address</label>
                <input type="email" className="form-control" id="inputmail1" value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            <div className="mb-3">
                <label htmlFor="inputpassword1" className="form-label">Password</label>
                <input type="password" className="form-control" id="inputpassword1" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div id="emailHelp" className="form-text">Don't have an account? Create one <a href="/register">here</a></div>
            <button type="submit" className="btn btn-primary mb-3" onClick={(e) => { loginHandler(e); }}>Login</button>
            <label>{ label }</label>
        </form>
        );
};

export default Login;