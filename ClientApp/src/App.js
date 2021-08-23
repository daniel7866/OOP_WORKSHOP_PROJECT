import React, { Component } from 'react';
import { Route, BrowserRouter, Switch } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Dashboard } from './components/Dashboard';
import Popup from "./components/Popup";
import Login from "./components/Login";
import Register from "./components/Register";

import './custom.css'


//used to import redux state
import { useSelector } from 'react-redux';

const App = () => {
    const user = useSelector(state => state.user);//import redux global state

return (
    <Layout>
        {/*Here is the login popup, if jwt is not null no login form will be shown*/}
        <Popup trigger={user.jwt == null && window.location.pathname + window.location.search!= "/register"}>
            <Login />
        </Popup>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/dashboard' component={Dashboard} />
        <Route path='/register' component={Register} />
    </Layout>
)
};

export default App;