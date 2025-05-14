import { request, TOKEN } from "./shared.js";

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

// console.log(
// 	await authUser({
// 		email: "admin fuck",
// 		password: "admin dude",
// 	})
// );
