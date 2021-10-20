import React, {useState} from "react";
import ProgressBar from "./ProgressBar";
import { storage } from "../firebase";
import { getAddress } from "../Services";
import "../Styles/Popup.css";

/**
 * This component is responsible for changing user's account details
 * It will display a form with details to choose from:
 * Profile picture
 * Name
 * Password
 */

const formStyle = { margin: "1rem" ,
                    padding: "1rem",
                    display: "flex",
                    flexDirection: "column",
                    backgroundColor: "background-color: rgba(229, 229, 229, 0.8)",
                    borderRadius: "1rem",
                    boxShadow: "#282c34 0 0 4px 0",
                    padding: "1px",
                    alignItems: "center"
                    };

 const EditProfileDetails = (props) => {
    const [image, setImage] = useState(null);
    const [name, setName] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [repeatNewPassword, setRepeatNewPassword] = useState('');
    const [progress, setProgress] = useState(0);
    const [label, setLabel] = useState('');

    var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'PATCH',
        headers: myHeaders,
        body: null,
        redirect: 'follow'
        };

    const verifyMatchingPasswords = () => {
        if(repeatNewPassword===newPassword){
            setLabel("");
            return true;
        }
        //else
        setLabel("Passwords do not match!");
        return false;
    }

    //change password
    const editProfilePasswordHandler = ()=>{
        if(!verifyMatchingPasswords(newPassword,repeatNewPassword))//don't do anything if passwords do not match
            return;

        var raw = JSON.stringify({
        "oldPassword": oldPassword,
        "password": newPassword
        });
        requestOptions.body = raw;

        fetch(`${getAddress()}/api/user/update`, requestOptions)
        .then(response => response.json())
        .then(result => setLabel(result.message))
        .catch(error => console.log('error', error));
    }//


    //change name
    const editProfileNameHandler = ()=>{
        var raw = JSON.stringify({
        "name": name
        });
        requestOptions.body = raw;

        fetch(`${getAddress()}/api/user/update`, requestOptions)
        .then(response => response.json())
        .then(result => setLabel(result.message))
        .catch(error => console.log('error', error));
    }//


    const handleFileChange = (e) => {
        if (e.target.files[0]) {
            setImage(e.target.files[0]);
        }
    }

    //change profile picture
    const editProfilePictureHandler = () => {
        if(image==null){
            alert("You must select an image");
            return;
        }

        let hashed = `${image.name} + ${Date.now()}`;

        const uploadTask = storage.ref(`images/${hashed}`).put(image);
        uploadTask.on(
            "state_changed",
            snapshot => {
                const prog = Math.round(
                    (snapshot.bytesTransferred / snapshot.totalBytes) * 100);
                setProgress(prog);
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
                        console.log(url);

                        var raw = JSON.stringify({
                            "imagePath": `${url}`
                        });
                        requestOptions.body = raw;

                        fetch(`${getAddress()}/api/user/update`, requestOptions)
                        .then(response => response.json())
                        .then(result => setLabel(result.message))
                            .catch(error => console.log('error', error));
                    });
            }
        );
    }//

    return (
        <div>
            <button className="btn btn-outline-danger" onClick={()=>props.setEditPopup(false)} >Close Window</button>
            <br />
            <label>Edit profile details:</label>
            <div>
                <div style={formStyle}
                 >

                    <label>Change profile image:</label>
                    <input type="file" className="form-control-file" style={{margin: "auto", width: "min-content"}} placeholder="New profile image" onChange={handleFileChange} />
                    <button className="btn btn-primary" onClick={editProfilePictureHandler}>Change profile image</button>
                    <ProgressBar bgcolor={"#00695c"} completed={progress}/>
                </div>

                <div style={formStyle} >

                    <label>Change profile name:</label>
                    <input type="text" className="form-control" placeholder="New name" value={name} onChange={(e)=>setName(e.target.value)} />
                    <button className="btn btn-primary" onClick={editProfileNameHandler}>Change profile name</button>
                </div>

                <div style={formStyle} >

                    <label>Change password:</label>
                    <input type="password" className="form-control" placeholder="Old password" value={oldPassword} onChange={(e)=>setOldPassword(e.target.value)} />
                    <input type="password" className="form-control" placeholder="New password" value={newPassword} onChange={(e)=>setNewPassword(e.target.value)}/>
                    <input type="password" className="form-control" placeholder="Repeat new password" value={repeatNewPassword} onKeyUp={verifyMatchingPasswords} onChange={(e)=>setRepeatNewPassword(e.target.value)}/>
                    <button className="btn btn-primary" onClick={editProfilePasswordHandler}>Change password</button>
                </div>
                
                <h6>{label}</h6>
            </div>
        </div>
    )
}

export default EditProfileDetails;