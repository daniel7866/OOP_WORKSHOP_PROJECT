export const getAddress = () => {
    return window.location.origin;
};


/**
 * 
 * This function gets a string and trim the string till the last slash - returns everything after it:
 * trimTillLastSlash('/profile/19') -> '19'
 */
export const takeLastUrlItem = (str) => {
    let index = 0;
    while (index != -1) {
        index = str.indexOf('/');
        if (index == str.length - 1) {
            return str.substring(0, str.length-1);
        }
        str = str.substring(index + 1);
    }
    return str;
}


export const getUsers = (uidList) => {
    let promises = [];
    for (let i = 0;i < uidList.length; i++) {
        promises.push(getUser(uidList[i]));
    }

    return Promise.all(promises);
}

export const getUser = (uid) =>
    fetch(`${getAddress()}/api/user/id/${uid}`)
        .then(response => response.json())
        .then(result => result)
        .catch(error => console.log('error', error));

export const findIdInUserList = (userList, id) => {
    for (let i = 0; i < userList.length; i++) {
        if (userList[i].id == id) {
            return i;
        }
    }
    return -1;
}