export const BASE_URL = "https://localhost:7227";

export const request = async (url, TOKEN = "", options = {}) => {
	try {
		const { method = "GET", body } = options;

		const fetchOptions = {
			method,
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${TOKEN}`,
			},
		};

		// Only include body if method is not GET and body is defined
		if (method !== "GET" && body !== undefined) {
			fetchOptions.body = JSON.stringify(body);
		}

		const response = await fetch(`${BASE_URL}${url}`, fetchOptions);

		if (!response.ok) {
			let errorText;
			try {
				errorText = await response.text(); // or response.json() if your backend sends JSON errors
			} catch (e) {
				errorText = response.statusText;
			}
			throw new Error(errorText);
		}

		if (response.status === 204) return null; // No Content

		return await response.json();
	} catch (err) {
		return {
			msg: err.message || "Unknown error",
		};
	}
};
