import firebase from "firebase/compat/app";
import "firebase/compat/storage";

/**
 * This file contains the firebase storage details for uploading images
 */

const firebaseConfig = {
    apiKey: "AIzaSyAgnWt531K5SGNbGwMQ-TNXU-gGBbwHRL8",
    authDomain: "oop-project-5cde7.firebaseapp.com",
    projectId: "oop-project-5cde7",
    storageBucket: "oop-project-5cde7.appspot.com",
    messagingSenderId: "395960131468",
    appId: "1:395960131468:web:c4a965761d7815af3b00a9",
    measurementId: "G-FRQ6RET28P"
};

firebase.initializeApp(firebaseConfig);
const storage = firebase.storage();

export { storage, firebase as default };