
using HospitalApp.Helper;

namespace Hospital
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<HospitalContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IDoctorRepo,DoctorRepo>();
            builder.Services.AddScoped<IDoctorService,DoctorService>();
            builder.Services.AddScoped<IPatientRepo,PatientRepo>();
            builder.Services.AddScoped<IPatientService,PatientService>();
            builder.Services.AddScoped<IMedicalRecordRepo,MedicalRecordRepo>();
            builder.Services.AddScoped<IMedicalRecordService,MedicalRecordService>();
            builder.Services.AddScoped<IAppointmentRepo,AppointmentRepo>();
            builder.Services.AddScoped<IAppointmentService,AppointmentService>();

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
