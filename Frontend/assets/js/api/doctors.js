import { request } from "./shared.js";
export async function getAllDoctors(TOKEN) {
	return await request(`/api/Doctor`, TOKEN);
}
export async function getDoctorById(id, TOKEN) {
	return await request(`/api/Doctor/${id}`, TOKEN);
}
export async function getDoctorPatients(id, TOKEN) {
	return await request(`/api/Doctor/${id}/patients`, TOKEN);
}
export async function filterDoctors(queryOptions, TOKEN) {
	const query = new URLSearchParams(queryOptions).toString();
	return await request(`/api/Doctor?${query}`, TOKEN);
}
export async function getDoctorAppointments(id, TOKEN) {
	return await request(`/api/Doctor/${id}/appointments`, TOKEN);
}
export async function getDoctorUpcomingAppointments(id, TOKEN) {
	return await request(`/api/Doctor/${id}/upcomingAppointments`, TOKEN);
}
export async function getDoctorAverageRating(id, TOKEN) {
	return await request(`/api/Doctor/${id}/rating`, TOKEN);
}
export async function getDoctorRatingByPatient(id, patId, TOKEN) {
	return await request(`/api/Doctor/${id}/rating/${patId}`, TOKEN);
}
export async function getDoctorProfitByDate(id, TOKEN, date) {
	return await request(`/api/Doctor/${id}/profit/${date}`, TOKEN);
}

export async function addDoctor(TOKEN, doctor) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "POST",
		body: doctor,
	});
}
export async function updateDoctorById(id,doctor,TOKEN) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "PUT",
		body: doctor,
	});
}
export async function deleteDoctorById(id,TOKEN) {
	return await request(`/api/Doctor`, TOKEN, {
		method: "DELETE",
	});
}

