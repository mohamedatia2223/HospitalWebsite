export const BASE_URL = "http://localhost:5251";
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
