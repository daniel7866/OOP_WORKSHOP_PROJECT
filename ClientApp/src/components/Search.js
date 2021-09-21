import React from 'react';
import { useSearch } from "../hooks/useSearch";
import ProfileListItem from "./ProfileListItem";
import "../Styles/Images.css";
import "../Styles/Profile.css";
import "../Styles/Search.css";

const Search = (props) => {
    const [input, setInput, results, setResults, label, searchHandler] = useSearch();

    return (
        <div className="search-container">
            <h1>Search for users:</h1>
            <div>
                <input className="from-control" type="text" value={input} onChange={(e) => { setInput(e.target.value); }} />
                <button className="btn btn-primary" onClick={searchHandler} >Search</button>
            </div>
            <div className={`search-result-container ${results.length==0?"":"full"}`}>
                <div className="profile-follow-list">
                    {results.map(r => <ProfileListItem key={r.id} id={r.id} name={r.name} imagePath={r.imagePath} />)}
                    <h3>{label}</h3>
                </div>
            </div>
        </div>
    );
};

export default Search;