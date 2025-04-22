namespace Hospital.Utils
{
	public class Mapper
	{
		public static DoctorDTO MapDoctorDTO(Doctor doctor)
		{
			return new DoctorDTO()
			{
				DoctorId = doctor.DoctorId,
				DoctorName = doctor.DoctorName,
				Email = doctor.Email,
				Specialty = doctor.Specialty,
				ContactInfo = doctor.ContactInfo,
				YearsOfExperience = doctor.YearsOfExperience,
				HourlyPay = doctor.HourlyPay,
			};
		}
		public static List<DoctorDTO> MapDoctorDTOs(List<Doctor> doctors)
		{
			var output = new List<DoctorDTO>();

			foreach (var doctor in doctors) {
				output.Add(
					new DoctorDTO()
					{
						DoctorId = doctor.DoctorId,
						DoctorName = doctor.DoctorName,
						Email = doctor.Email,
						Specialty = doctor.Specialty,
						ContactInfo = doctor.ContactInfo,
						YearsOfExperience = doctor.YearsOfExperience,
						HourlyPay = doctor.HourlyPay,
					});
			}

			return output;
		}
		public static PatientDTO MapPatientDTO(Patient patient)
		{
			return new PatientDTO()
			{
				PatientId = patient.PatientId,
				PatientName = patient.PatientName,
				Email = patient.Email,
				EmergencyContact = patient.EmergencyContact,
				ContactInfo = patient.ContactInfo,
				Address = patient.Address,
				InsuranceDetails = patient.InsuranceDetails,
			};
		}
		public static List<PatientDTO> MapPatientDTOs(List<Patient> patients)
		{
			var output = new List<PatientDTO>();

			foreach (var patient in patients)
			{
				output.Add(
					new PatientDTO()
					{
						PatientId = patient.PatientId,
						PatientName = patient.PatientName,
						Email = patient.Email,
						EmergencyContact = patient.EmergencyContact,
						ContactInfo = patient.ContactInfo,
						Address = patient.Address,
						InsuranceDetails = patient.InsuranceDetails,
					});
			}

			return output;
		}
		public static AppointmentDTO MapAppointmentDTO(Appointment appointment)
		{
			return new AppointmentDTO()
			{
				AppointmentId = appointment.AppointmentId,
				AppointmentDate = appointment.AppointmentDate,
				ReasonForVisit = appointment.ReasonForVisit,
				PatientId = appointment.PatientId,
				DoctorId = appointment.DoctorId,
			};
		}
		public static List<AppointmentDTO> MapAppointmentDTOs(List<Appointment> appointments)
		{
			var output = new List<AppointmentDTO>();

			foreach (Appointment appointment in appointments)
			{
				output.Add(
					new AppointmentDTO()
					{
						AppointmentId = appointment.AppointmentId,
						AppointmentDate = appointment.AppointmentDate,
						ReasonForVisit = appointment.ReasonForVisit,
						PatientId = appointment.PatientId,
						DoctorId = appointment.DoctorId,
					});
			}

			return output;
		}
		public static MedicalRecordDTO MapMedicalRecordDTO(MedicalRecord medicalRecord)
		{
			return new MedicalRecordDTO()
			{
				MedicalRecordId = medicalRecord.MedicalRecordId,
				RecordDate = medicalRecord.RecordDate,
				Notes = medicalRecord.Notes,
				Diagnosis = medicalRecord.Diagnosis,
				Treatment = medicalRecord.Treatment,
				PatientId= medicalRecord.PatientId,
			};
		}
		public static List<MedicalRecordDTO> MapMedicalRecordDTOs(List<MedicalRecord> medicalRecords)
		{
			var output = new List<MedicalRecordDTO>();

			foreach (var medicalRecord in medicalRecords)
			{
				output.Add(
					new MedicalRecordDTO()
					{
						MedicalRecordId = medicalRecord.MedicalRecordId,
						RecordDate = medicalRecord.RecordDate,
						Notes = medicalRecord.Notes,
						Diagnosis = medicalRecord.Diagnosis,
						Treatment = medicalRecord.Treatment,
						PatientId = medicalRecord.PatientId,
					});
			}

			return output;
		}
	}
}
