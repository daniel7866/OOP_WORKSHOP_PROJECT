import React from "react";
import { useState } from "react";
import { useRegister } from '../hooks/useRegister';

const Register = () => {
    const [name, setName, email, setEmail, password, setPassword, file, setFile, label, registerHandler] = useRegister();

    return (
        <form className="register-form">
            <h2>Register:</h2>
            <div className="mb-3">
                <label for="inputname1" className="form-label">Name</label>
                <input type="text" className="form-control" id="inputname1" value={name} onChange={(e) => setName(e.target.value)} />
            </div>
            <div className="mb-3">
                <label for="inputmail1" className="form-label">Email address</label>
                <input type="email" className="form-control" id="inputmail1" value={email} onChange={(e) => setEmail(e.target.value)} />
            </div>
            <div className="mb-3">
                <label for="inputpassword1" className="form-label">Password</label>
                <input type="password" className="form-control" id="inputpassword1" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div id="emailHelp" class="form-text">Don't have an account? Create one <a href="/register">here</a></div>
            {/*<div class="input-group mb-3">
                <label for="inputGroupFile02" className="form-label">Profile Picture</label>
                <input type="file" class="form-control" id="inputGroupFile02" onChange={(e) => { setFile(e.target.files[0].href) }} />
                <label class="input-group-text" for="inputGroupFile02">Upload</label>
    </div>*/}
            <button type="submit" class="btn btn-primary mb-3" onClick={(e) => { registerHandler(e); }}>Register</button>
            <label>{label}</label>
        </form>
        );
};

export default Register;