import React, { useState } from 'react';
import { getAddress } from '../Services';
import "../Styles/Post.css";

const AddPost = (props) => {
    const [text, setText] = useState('');
    const [file, setFile] = useState('');

    const uploadHandler = () => {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
            "description": `${text}`,
            "imagePath": ``
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/createpost`, requestOptions)
            .then(response => response.json())
            .then(result => { console.log(result); props.setRefresh(value => !value); setText(""); setFile(""); })
            .catch(error => console.log('error', error));
    }

    return (
        <div className="add-post">
            <h3>Add a new post</h3>
            <input type="file" value={file} onChange={e => setFile(e.target.value)} />
            <input type="text" placeholder="Type description here" value={text} onChange={(e) => setText(e.target.value)} />
            <button className="btn btn-primary" onClick={() => { uploadHandler(); }}>Upload</button>
        </div>
    );
};

export default AddPost;