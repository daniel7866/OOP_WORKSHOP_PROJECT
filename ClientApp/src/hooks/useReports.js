import React, {useState} from "react";
import { getAddress } from "../Services";

export const useReports = () => {
    const [commentReports, setCommentReports] = useState([]);
    const [postReports, setPostReports] = useState([]);

    //fetch all the reports
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

    //close a report on a post
    //remove - true or false (if we wish to remove the post as well)
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
        .then(response => setPostReports(prevstate=>prevstate.filter(report => report.post.id !== postId)))
        .catch(error => console.log('error', error));
    }

    //close a report on a comment
    //remove - true or false (if we wish to remove the comment as well)
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
        .then(response => setCommentReports(prevstate=>prevstate.filter(comment => comment.comment.id !== commentId)))
        .catch(error => console.log('error', error));
    }

    return [postReports, commentReports, closePostReport, closeCommentReport, fetchAll];
}