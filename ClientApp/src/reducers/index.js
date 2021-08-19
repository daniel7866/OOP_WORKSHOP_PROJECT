import userReducer from './user';
import { combineReducers } from 'redux';


/**
 * Combine reducers in case we have multiple global states.
 * */

const allReducers = combineReducers({
    user: userReducer
});

export default allReducers;