/**
 * This file contains the actions we have to manipulate the redux state.
 */

export const login = (payload) => {
    return {
        type: 'LOGIN',
        payload: payload
    }
}

export const logout = () => {
    return {
        type: 'LOGOUT'
    }
}