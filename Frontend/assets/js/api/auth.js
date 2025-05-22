import { request } from "./shared.js";

/*
{
email : "",
password : ""
}
*/
export async function authUser(loginDTO) {
	return await request(`/api/Auth`, "", {
		method: "POST",
		body: loginDTO,
	});
}
