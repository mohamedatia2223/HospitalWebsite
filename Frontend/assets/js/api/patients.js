import { request, TOKEN } from "./shared.js";

async function getAllPatients(TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "GET",
	});
}

async function getPatientById(id, TOKEN) {
	return await request(`/api/Patient/${id}`, TOKEN);
}

async function getPatientRecords(id, TOKEN) {
	return await request(`/api/Patient/${id}/records`, TOKEN);
}
async function getPatientAppointments(id, TOKEN) {
	return await request(`/api/Patient/${id}/appointments`, TOKEN);
}
async function getPatientUpcommingAppointments(id, TOKEN) {
	return await request(`/api/Patient/${id}/upcomingAppointments`, TOKEN);
}
async function filterPatientsByName(name, TOKEN) {
	return await request(`/api/Patient/filter?name=${name}`, TOKEN);
}

async function getPatientDoctors(id,TOKEN) {
	return await request(`/api/Patient/${id}/doctors`,TOKEN)
}
async function assignDoctorToPatient(patientId, doctorId, TOKEN) {
	return await request(
		`/api/Patient/${patientId}/assignDoctor/${doctorId}`,
		TOKEN,
		{
			method: "POST",
		}
	);
}
async function rateDoctor(patientId, doctorId, rating, TOKEN) {
	return await request(
		`/api/Patient/${patientId}/rateDoctor/${doctorId}?rating=${rating}`,
		TOKEN,
		{
			method: "POST",
		}
	);
}
async function addPatient(patient) {
	return await request(`/api/Patient`, "", {
		method: "POST",
		body: patient,
	});
}

async function updatePatientById(id, patient, TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "PUT",
		body: patient,
	});
}
async function deletePatientById(id, TOKEN) {
	return await request(`/api/Patient`, TOKEN, {
		method: "DELETE",
	});
}

console.log(await getPatientDoctors("a394466c-3fc2-4f70-826e-904b071db3a5",TOKEN));
