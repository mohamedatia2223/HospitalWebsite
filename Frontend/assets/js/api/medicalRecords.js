import { request, TOKEN } from "./shared.js";

async function getAllMedicalRecords(TOKEN) {
	return await request(`/api/MedicalRecord`, TOKEN);
}
async function getMedicalRecordById(id, TOKEN) {
	return await request(`/api/MedicalRecord/${id}`, TOKEN);
}
async function getMedicalRecordsByDateRange(TOKEN, queryParams) {
	const query = new URLSearchParams(queryParams).toString();
	return await request(`/api/MedicalRecord/by-date-range?${query}`, TOKEN);
}

async function searchRecords(TOKEN, keyword) {
	return await request(`/api/MedicalRecord/SearchRecords/${keyword}`, TOKEN);
}
async function addMedicalRecord(TOKEN, MD) {
	return await request(`/api/MedicalRecord`, TOKEN, {
		method: "POST",
		body: MD,
	});
}
async function updateMedicalRecordById(id, TOKEN, MD) {
	return await request(`/api/MedicalRecord/${id}`, TOKEN, {
		method: "PUT",
		body: MD,
	});
}
async function deleteMedicalRecordById(id, TOKEN) {
	return await request(`/api/MedicalRecord/${id}`, TOKEN, {
		method: "DELETE",
	});
}
