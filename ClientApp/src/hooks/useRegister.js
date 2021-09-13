import React, { useState } from 'react';
import { getAddress } from "../Services";

export const useRegister = () => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [file, setFile] = useState('');

    const [label, setLabel] = useState('');

    const registerHandler = (e) => {
        e.preventDefault(); // prevent page from reloading

        //need to make a script to upload image to aws

        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
            "Email": email,
            "Name": name,
            "Password": password
            //here we add image path after uploaded to aws
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/user/register`, requestOptions)
            .then(response => {
                if (response.ok) {
                    setLabel("Registered successfully");
                }
                else {
                    response.text().then(text => setLabel(text));
                }
            })
            .catch(error => console.log(error));
    }

    return [name, setName, email, setEmail, password, setPassword, file, setFile, label, registerHandler];
};