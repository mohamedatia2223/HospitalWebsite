export const BASE_URL = "http://localhost:5251";
export const TOKEN =
	"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjlmYjIxMWRlLWVjZmUtNGQ1Zi04MTk5LWJlOTFjZTQzYmJmNyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhZG1pbiBtYW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbiBmdWNrIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NDcxNjAyMDIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTI1MSIsImF1ZCI6InNoaXQifQ.9KUAcs6fLyAvmFvWOTUfDv6BlnB-5AndD7s18re_s2U";

export const request = async (url, TOKEN = "", options = {}) => {
	try {
		const response = await fetch(`${BASE_URL}${url}`, {
			method: options.method ? options.method : "GET",
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${TOKEN}`,
			},
			body: JSON.stringify(options.body),
		});

		if (!response.ok) throw new Error(await response.text());
		if (response.status == 204) return null;

		return await response.json();
	} catch (err) {
		return {
			msg: err.message,
		};
	}
};
