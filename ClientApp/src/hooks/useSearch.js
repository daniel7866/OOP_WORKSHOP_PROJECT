import React, { useState } from "react";
import { getAddress } from "../Services";


/**
 * This custom hook will search people and store the results
 * */
export const useSearch = () => {
    const [input, setInput] = useState('');
    const [results, setResults] = useState([]);
    const [label, setLabel] = useState('');

    const searchHandler = () => {
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
            "search": input
        });

        var requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: raw,
            redirect: 'follow'
        };

        fetch(`${getAddress()}/api/user/search`, requestOptions)
            .then(response => response.json())
            .then(result => {
                setResults(result);
                if (result.length == 0)
                    setLabel("No matches were found!");
                else
                    setLabel("");
            })
            .catch(error => console.log(error));
    }

    return [input, setInput, results, setResults, label, searchHandler];
}