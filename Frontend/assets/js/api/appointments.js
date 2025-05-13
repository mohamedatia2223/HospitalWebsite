import { request, TOKEN } from "./shared.js";

async function getAllAppointments(TOKEN) {
	return await request(`/api/Appointment`, TOKEN);
}

async function getAppointmentById(id, TOKEN) {
	return await request(`/api/Appointment/${id}`, TOKEN);
}

async function getAppointmentsToday(TOKEN) {
	return await request(`/api/Appointment/Today`, TOKEN);
}
async function addAppointment(TOKEN, appointment) {
	return await request(`/api/Appointment`, TOKEN, {
		method: "POST",
		body: appointment,
	});
}
async function updateAppointmentById(id, TOKEN, appointment) {
	return await request(`/api/Appointment/${id}`, TOKEN, {
		method: "PUT",
		body: appointment,
	});
}
async function rescheduleAppointmentById(id, TOKEN, appointment, queryParams) {
	const query = URLSearchParams(queryParams).toString();
	return await request(`/api/Appointment/${id}/Reschedule?${query}`, TOKEN, {
		method: "PUT",
		body: appointment,
	});
}
async function deleteAppointmentById(id, TOKEN) {
	return await request(`/api/Appointment`, TOKEN, {
		method: "DELETE",
	});
}

// a394466c-3fc2-4f70-826e-904b071db3a5   PATIENT
// c455035b-a0c4-4a18-8505-01f930dd8065   DOCTOR