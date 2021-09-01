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