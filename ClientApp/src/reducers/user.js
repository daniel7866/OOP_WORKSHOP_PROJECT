/*
 * User reducer will represent the user state:
 * The state contains jwt, user_id and user_email.
 * The default state sets all the fields to null
 * */

const userReducer = (state = { jwt: null, uid: null, email: null }, action) => {
    switch (action.type) {
        case 'LOGIN':
            return { jwt: 'JWT', uid: 'UID', email: 'EMAIL' };
        case 'LOGOUT':
            return { jwt: null, uid: null, email: null };
        default:
            return state;
    }
}

export default userReducer;