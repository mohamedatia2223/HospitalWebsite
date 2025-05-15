export const getToken = () => {
	return localStorage.getItem("jwt") || sessionStorage.getItem("jwt");
};
export const logIn = (rawToken, rememberMe) => {
	if (!rawToken) {
		return false;
	}
	rememberMe
		? localStorage.setItem("jwt", rawToken)
		: sessionStorage.setItem("jwt", rawToken);
	return true;
};
export const logOut = () => {
	localStorage.removeItem("jwt");
	sessionStorage.removeItem("jwt");
};

export const parseJwt = (token) => {
	try {
		return JSON.parse(atob(token.split(".")[1]));
	} catch (e) {
		return null;
	}
};

export const cleanToken = (parsedToken) => {
	const id =
		parsedToken[
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
		];
	const name =
		parsedToken[
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
		];
	const email =
		parsedToken[
			"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
		];
	const role =
		parsedToken[
			"http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
		];
	const exp = parsedToken["exp"];

	return { id, name, email, role, exp };
};
export const isTokenValid = (parsedToken) => {
	return Date.now() / 1000 < parsedToken["exp"];
};
