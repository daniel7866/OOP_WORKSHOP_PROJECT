import userReducer from './user';
import { combineReducers } from 'redux';


/**
 * Combine reducers in case we could have multiple global states.
 * */

const allReducers = combineReducers({
    user: userReducer
});

export default allReducers;