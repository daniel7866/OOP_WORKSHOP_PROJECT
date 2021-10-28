import React, {useState} from "react";
import { getAddress } from "../Services";

export const useReports = () => {
    const [commentReports, setCommentReports] = useState([]);
    const [postReports, setPostReports] = useState([]);

    const fetchAll = () => {
          fetch(`${getAddress()}/api/root/reports`)
            .then(response => response.json())
            .then(result => {
                if(result.posts != null && result.comments !== null){
                    setPostReports(result.posts);
                    setCommentReports(result.comments);
                }
            })
            .catch(error => console.log('error', error));
    }

    const closePostReport = (postId, remove) => {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
        "postId": postId,
        "remove": remove
        });

        var requestOptions = {
        method: 'DELETE',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        fetch(`${getAddress()}/api/root/report/post`, requestOptions)
        .then(response => setPostReports(postReports.filter(report => report.postId !== postId)))
        .catch(error => console.log('error', error));
    }

    const closeCommentReport = (commentId, remove) => {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
        "commentId": commentId,
        "remove": remove
        });

        var requestOptions = {
        method: 'DELETE',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
        };

        fetch(`${getAddress()}/api/root/report/comment`, requestOptions)
        .then(response => setCommentReports(commentReports.filter(report => report.commentId !== commentId)))
        .catch(error => console.log('error', error));
    }

    return [postReports, commentReports, closePostReport, closeCommentReport, fetchAll];
}