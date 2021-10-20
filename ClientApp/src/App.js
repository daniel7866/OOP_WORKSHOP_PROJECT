import React, { Component } from 'react';
import { Route, BrowserRouter, Switch } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import Popup from "./components/Popup";
import Login from "./components/Login";
import Register from "./components/Register";
import Profile from './components/Profile';
import './custom.css'


//used to import redux state
import { useSelector } from 'react-redux';
import Search from './components/Search';
import Messages from './components/Messages';

const App = () => {
    const user = useSelector(state => state.user);//import redux global state

return (
    <Layout user={ user }>
        {/*Here is the login popup, if jwt is not null no login form will be shown*/}
        <Popup trigger={user.email == null && window.location.pathname + window.location.search!= "/register"}>
            <Login />
        </Popup>
        <Route exact path='/' component={Home} />
        <Route path='/register' component={Register} />
        <Route path='/search' component={Search} />
        <Route path='/profile' component={Profile} />
        <Route path='/messages' component={Messages} />
    </Layout>
)
};

export default App;