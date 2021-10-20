import React, { useState } from 'react';
import { getAddress } from '../Services';
import "../Styles/Post.css";
import { storage } from "../firebase/index.js";
import ProgressBar from "./ProgressBar"


/** 
 * This component is a form that adds a new post to the application.
 * A post includes an image and a text description.
 * It includes a progress bar for the image upload.
*/
const AddPost = (props) => {
    const [text, setText] = useState('');
    const [image, setImage] = useState(null);
    const [progress, setProgress] = useState(0);

    const handleChange = e => { //file change handler
        if (e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    };

    const uploadHandler = () => { //upload the image to firebase
        if(image==null){
            alert("You must select an image");
            return;
        }
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        let hashed = `${image.name} + ${Date.now()}`;

        const uploadTask = storage.ref(`images/${hashed}`).put(image);
        uploadTask.on(
            "state_changed",
            snapshot => {
                const progress = Math.round(
                    (snapshot.bytesTransferred / snapshot.totalBytes) * 100);
                setProgress(progress);
            },
            error => {
                console.log(error);
            },
            () => {
                storage
                    .ref("images")
                    .child(hashed)
                    .getDownloadURL()
                    .then(url => {

                        var raw = JSON.stringify({
                            "description": `${text}`,
                            "imagePath": `${url}`
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
                    });
            }
        );
    }

    return (
        <div className="add-post">
            <ProgressBar bgcolor={"#00695c"} completed={progress}/>
            <h3>Add a new post</h3>
            <input type="file" onChange={handleChange} />
            <input type="text" placeholder="Type description here" value={text} onChange={(e) => setText(e.target.value)} />
            <button className="btn btn-primary" onClick={() => { uploadHandler(); }}>Upload</button>
        </div>
    );
};

export default AddPost;