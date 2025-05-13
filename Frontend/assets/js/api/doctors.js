import { request, TOKEN } from "./shared.js";
async function getAllDoctors(TOKEN) {
	return await request(`/api/Doctor`, TOKEN);
}
async function getDoctorById(id, TOKEN) {
	return await request(`/api/Doctor/${id}`, TOKEN);
}
async function getDoctorPatients(id, TOKEN) {
	return await request(`/api/Doctor/${id}/patients`, TOKEN);
}
async function filterDoctors(queryOptions, TOKEN) {
	const query = new URLSearchParams(queryOptions).toString();
	return await request(`/api/Doctor?${query}`, TOKEN);
}
async function getDoctorAppointments(id, TOKEN) {
	return await request(`/api/Doctor/${id}/appointments`, TOKEN);
}
async function getDoctorUpcomingAppointments(id, TOKEN) {
	return await request(`/api/Doctor/${id}/upcomingAppointments`, TOKEN);
}
async function getDoctorAverageRating(id, TOKEN) {
	return await request(`/api/Doctor/${id}/rating`, TOKEN);
}
async function getDoctorRatingByPatient(id, patId, TOKEN) {
	return await request(`/api/Doctor/${id}/rating/${patId}`, TOKEN);
}
async function getDoctorProfitByDate(id, TOKEN, date) {
	return await request(`/api/Doctor/${id}/profit/${date}`, TOKEN);
}

async function addDoctor(TOKEN, doctor) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "POST",
		body: doctor,
	});
}
async function updateDoctorById(id,doctor,TOKEN) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "PUT",
		body: doctor,
	});
}
async function deleteDoctorById(id,TOKEN) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "DELETE",
	});
}

