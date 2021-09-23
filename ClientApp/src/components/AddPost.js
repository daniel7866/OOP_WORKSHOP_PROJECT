import React, { useState } from 'react';
import { getAddress } from '../Services';
import "../Styles/Post.css";
import { storage } from "../firebase/index.js";

const AddPost = (props) => {
    const [text, setText] = useState('');
    const [image, setImage] = useState(null);

    const handleChange = e => {
        if (e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    };

    const uploadHandler = () => {
        var myHeaders = new Headers();
        var imageURL;
        myHeaders.append("Content-Type", "application/json");

        const uploadTask = storage.ref(`images/${image.name}`).put(image);
        uploadTask.on(
            "state_changed",
            snapshot => { },
            error => {
                console.log(error);
            },
            () => {
                storage
                    .ref("images")
                    .child(image.name)
                    .getDownloadURL()
                    .then(url => {
                        imageURL=url;
                    });
            }
        );

        var raw = JSON.stringify({
            "description": `${text}`,
            "imagePath": `${imageURL}`
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/post/createpost`, requestOptions)
            .then(response => response.json())
            .then(result => { console.log(result); props.setRefresh(value => !value); setText(""); setImage(null); })
            .catch(error => console.log('error', error));
    }

    return (
        <div className="add-post">
            <h3>Add a new post</h3>
            <input type="file" onChange={handleChange} />
            <input type="text" placeholder="Type description here" value={text} onChange={(e) => setText(e.target.value)} />
            <button className="btn btn-primary" onClick={() => { uploadHandler(); }}>Upload</button>
        </div>
    );
};

export default AddPost;