import { request } from "./shared.js";
export async function getAllReviews(TOKEN) {
	return await request(`/api/Review/`, TOKEN);
}
export async function getAverageRating(TOKEN) {
	return await request(`/api/Review/GetAverageofRating`, TOKEN);
}
export async function getReviewCount(TOKEN) {
	return await request(`/api/Review/GetCountOfAllReviews`, TOKEN);
}
export async function getAllReviewsForPatient(id, TOKEN) {
	return await request(`/api/Review/GetAllReviewsForPatient/${id}`, TOKEN);
}
export async function addReview(review, TOKEN) {
	return await request(`/api/Review/AddReviewByPatientId`, TOKEN, {
		method: "POST",
		body: review,
	});
}
export async function updateReview(id, review, TOKEN) {
	return await request(`/api/Review/EditReview/${id}`, TOKEN, {
		method: "PUT",
		body: review,
	});
}
export async function deleteReview(id, TOKEN) {
	return await request(`/api/Review/DeleteReviewByReviewId/${id}`, TOKEN, {
		method: "DELETE",
	});
}
export async function filterReviews(
	TOKEN,
	keyword = "",
	sortBy = "",
	rating = 0,
	page = 1 ,
	pageSize = 10
) {
	const query = new URLSearchParams({
		keyword,
		sortBy,
		rating,
		page,
		pageSize,
	});
	return await request(`/api/Review/filter?${query}`, TOKEN);
}
