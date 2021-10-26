import React, {useState} from "react";
import { useReports } from "../hooks/useReports";
import CommentReport from "./CommentReport";
import PostReport from "./PostReport";

const Reports = () => {
    const [postReports, commentReports, closePostReport, closeCommentReport, fetchAll] = useReports();
    const [refresh, setRefresh] = useState(false);

    fetchAll();

    return (
        <div className="reports-container">
            <h5>Posts:</h5>
            {postReports.length==0?<h5>No post reports to show</h5>:null}
            {postReports.map(report => <PostReport key={report.id} count={report.count}
                                        id={report.id} postId={report.post.id}
                                        imagePath={report.post.imagePath}
                                        description={report.post.description}
                                        datePosted={report.post.datePosted}
                                        user={report.post.user}
                                        closePostReport={closePostReport} />)}
            
            <h5>Comments:</h5>
            {commentReports.length==0?<h5>No comment reports to show</h5>:null}
            {commentReports.map(report => <CommentReport key={report.id} count={report.count}
                                        id={report.id} commentId={report.comment.id}
                                        body={report.comment.body}
                                        datePosted={report.comment.datePosted}
                                        userId={report.comment.userId}
                                        name={report.comment.userName}
                                        imagePath={report.comment.userImagePath}
                                        closeCommentReport={closeCommentReport} />)}
        </div>
    );
}

export default Reports;