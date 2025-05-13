using AutoMapper;

namespace HospitalApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Appointment, AppointmentDTOUpdate>();
            CreateMap<AppointmentDTOUpdate, Appointment>();
            CreateMap<Appointment, AppointmentDTOGet>();
            CreateMap<AppointmentDTOGet, Appointment>();

            CreateMap<Doctor, DoctorDTOUpdate>();
            CreateMap<DoctorDTOUpdate, Doctor>();
            CreateMap<Doctor, DoctorDTOGet>();
            CreateMap<DoctorDTOGet, Doctor>();

            CreateMap<Patient, PatientDTOUpdate>();
            CreateMap<PatientDTOUpdate, Patient>();
            CreateMap<Patient, PatientDTOGet>();
            CreateMap<PatientDTOGet, Patient>();

            CreateMap<MedicalRecord, MedicalRecordDTOUpdate>();
            CreateMap<MedicalRecordDTOUpdate, MedicalRecord>();
            CreateMap<MedicalRecord, MedicalRecordDTOGet>();
            CreateMap<MedicalRecordDTOGet, MedicalRecord>();


            CreateMap<ReviewDTOGet, ReviewForHospital>();
            CreateMap<ReviewForHospital, ReviewDTOGet>();
            CreateMap<ReviewDTOUpdate, ReviewForHospital>();
            CreateMap<ReviewForHospital, ReviewDTOUpdate>();

            CreateMap<DoctorPatient, DoctorPatientDTO>();
            CreateMap<DoctorPatientDTO, DoctorPatient>();

        }
    }
}
