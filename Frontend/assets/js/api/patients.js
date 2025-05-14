import { request} from "./shared.js";

export async function getAllPatients(TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "GET",
	});
}

export async function getPatientById(id, TOKEN) {
	return await request(`/api/Patient/${id}`, TOKEN);
}

export async function getPatientRecords(id, TOKEN) {
	return await request(`/api/Patient/${id}/records`, TOKEN);
}
export async function getPatientAppointments(id, TOKEN) {
	return await request(`/api/Patient/${id}/appointments`, TOKEN);
}
export async function getPatientUpcommingAppointments(id, TOKEN) {
	return await request(`/api/Patient/${id}/upcomingAppointments`, TOKEN);
}
export async function filterPatientsByName(name, TOKEN) {
	return await request(`/api/Patient/filter?name=${name}`, TOKEN);
}

export async function getPatientDoctors(id,TOKEN) {
	return await request(`/api/Patient/${id}/doctors`,TOKEN)
}
export async function assignDoctorToPatient(patientId, doctorId, TOKEN) {
	return await request(
		`/api/Patient/${patientId}/assignDoctor/${doctorId}`,
		TOKEN,
		{
			method: "POST",
		}
	);
}
export async function rateDoctor(patientId, doctorId, rating, TOKEN) {
	return await request(
		`/api/Patient/${patientId}/rateDoctor/${doctorId}?rating=${rating}`,
		TOKEN,
		{
			method: "POST",
		}
	);
}
export async function addPatient(patient) {
	return await request(`/api/Patient`, "", {
		method: "POST",
		body: patient,
	});
}

export async function updatePatientById(id, patient, TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "PUT",
		body: patient,
	});
}
export async function deletePatientById(id, TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "DELETE",
	});
}
