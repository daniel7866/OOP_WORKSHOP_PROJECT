import React from "react";

const Login = () => {
    return (
        <form className="login-form">
            <h2>Login</h2>
            <label >Email:</label>
            <input type="text" placeholder="Enter Email Address" />
            <br/>
            <label >Password:</label>
            <input type="password" placeholder="Password" />
            <br />
            <button type="submit" class="btn btn-primary mb-3" onClick={(e) => { e.preventDefault(); }}>Login</button>
        </form>
        );
};

export default Login;