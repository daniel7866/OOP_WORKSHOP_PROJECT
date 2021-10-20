import React from "react";
import { useState } from "react";
import { useRegister } from '../hooks/useRegister';


/**
 * This component is the register page for signing up new users to the system.
 * There is a form for filling personal information.
 * The information is updated using a custom hook 'useRegister'
 */
const Register = () => {
    const [name, setName, email, setEmail, password, setPassword, file, setFile, label, registerHandler] = useRegister();
    const [matchPassword, setMatchPassword] = useState('');
    const [matchPasswordLabel, setMatchPasswordLabel] = useState('');

    const verifyMatchingPasswords = () => {
        if(matchPassword===password){
            setMatchPasswordLabel("");
            return true;
        }
        //else
        setMatchPasswordLabel("Passwords do not match!");
        return false;
    }

    return (
        <form className="register-form">
            <h2>Register:</h2>
            <h6>A password must be at least 4 characters long and contain letters and numbers</h6>

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
                <label for="inputpassword2" className="form-label">Verify password</label>
                <input type="password" className="form-control" id="inputpassword2" value={matchPassword} onKeyUp={verifyMatchingPasswords} onChange={(e) => setMatchPassword(e.target.value)} />
            </div>

            <button type="submit" class="btn btn-primary mb-3" onClick={(e) => { e.preventDefault(); if(verifyMatchingPasswords())registerHandler(e); }}>Register</button>
            <label>{label}</label>
            <br />
            <label>{matchPasswordLabel}</label>
            <br />
        </form>
        );
};

export default Register;