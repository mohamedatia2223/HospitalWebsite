using AutoMapper;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppointmentRepo, Appointment>();
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId));
            CreateMap<AppointmentDTO, Appointment>();
            CreateMap<Doctor, DoctorDTO>(); 
            CreateMap<DoctorDTO, Doctor>();
            CreateMap<PatientDTO, Patient>();
            CreateMap<Patient, PatientDTO>();
            CreateMap<MedicalRecord, MedicalRecordDTO>();
            CreateMap<MedicalRecordDTO, MedicalRecord>();
        }
    }
}