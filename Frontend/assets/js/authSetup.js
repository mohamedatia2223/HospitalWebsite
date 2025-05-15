import {
	cleanToken,
	getToken,
	parseJwt,
	isTokenValid,
	logOut,
} from "./utils/jwt.js";

export const setupUser = () => {
	const rawToken = getToken();

	if (!rawToken) return redirectToLogin();

	const parsedToken = parseJwt(rawToken);

	if (!parsedToken || !isTokenValid(parsedToken)) {
		logOut();
		redirectToLogin();
	}

	const token = cleanToken(parsedToken);

	window.User = token;
};

const redirectToLogin = () => {
	window.location.href = "http://127.0.0.1:5500/Frontend/Guests/log-in.html";
};
