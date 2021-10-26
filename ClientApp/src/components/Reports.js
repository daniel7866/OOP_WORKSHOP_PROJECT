import React, {useState} from "react";
import { useReports } from "../hooks/useReports";
import PostReport from "./PostReport";

const Reports = () => {
    const [postReports, commentReports, closePostReport, closeCommentReport, fetchAll] = useReports();
    const [refresh, setRefresh] = useState(false);

    fetchAll();

    return (
        <div className="reports-container">
            <h1>Posts:</h1>
            {postReports.map(report => <PostReport key={report.id} count={report.count}
                                        id={report.id} postId={report.post.id}
                                        imagePath={report.post.imagePath}
                                        description={report.post.description}
                                        datePosted={report.post.datePosted}
                                        user={report.post.user}
                                        closePostReport={closePostReport} />)}
        </div>
    );
}

export default Reports;