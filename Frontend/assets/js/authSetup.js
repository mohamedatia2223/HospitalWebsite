import {
	cleanToken,
	getToken,
	parseJwt,
	isTokenValid,
	logOut,
} from "./utils/jwt.js";

export const setupUser = () => {
    const rawToken = getToken();

    if (!rawToken) return null; 

    const parsedToken = parseJwt(rawToken);

    if (!parsedToken || !isTokenValid(parsedToken)) {
        logOut(); 
        return null; 
    }

    const token = cleanToken(parsedToken);
    window.User = token;
    return token; 
};