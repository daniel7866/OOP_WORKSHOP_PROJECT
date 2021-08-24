/*
 * User reducer will represent the user state:
 * The state contains jwt, user_id and user_email.
 * The default state sets all the fields to null
 * */

const userReducer = (state = { uid: null, email: null, name: null }, action) => {
    switch (action.type) {
        case 'LOGIN':
            return { uid: action.payload.id, email: action.payload.email, name: action.payload.name };
        case 'LOGOUT':
            return { uid: null, email: null, name: null };
        default:
            return state;
    }
}

export default userReducer;