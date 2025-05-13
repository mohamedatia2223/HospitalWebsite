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

            builder.Services.AddScoped<AuthService>();
            
            builder.Services.AddAutoMapper(typeof(MappingProfiles));


            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>  {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true , 
                    ValidateAudience = false , 
                    ValidateIssuerSigningKey = true ,
                    ValidateLifetime = true ,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
            };
            });

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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
