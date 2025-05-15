namespace Hospital.Services
{
    public class AuthService
    {
        private readonly HospitalContext _context;
        private readonly IConfiguration _config;
        public AuthService(HospitalContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<object?> Authenticate(string email, string password)
        {
            var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.Email == email && d.Password == password);
            var patient = await _context.Patients.SingleOrDefaultAsync(d => d.Email == email && d.Password == password);
            var admin = await _context.Admins.SingleOrDefaultAsync(d => d.Email == email && d.Password == password);

            var claims = new List<Claim>();
            if (doctor != null)
            {
                return doctor;
            }
            else if (patient != null)
            {
                return patient;
            }
            else if (admin != null)
            {
                return admin;
            }
            return null;

        }
        public object GenerateToken<T>(T user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }


            var id = new Claim(ClaimTypes.NameIdentifier, GetUserId<T>(user).ToString());
            var email = new Claim(ClaimTypes.Email, GetUserEmail<T>(user));
            var name = new Claim(ClaimTypes.Name, GetUserName(user) );
            var role = new Claim(ClaimTypes.Role, user.GetType().Name.ToString());
            var claims = new List<Claim>
            {
                id,name,email,role
        
            };

            var key = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var creds = new SigningCredentials(key,
                        SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
            );

            var output = new {
                token = new JwtSecurityTokenHandler().WriteToken(token) ,
                expires = DateTime.Now.AddHours(6) 
            };
                    
            return output;


        }

        private Guid GetUserId<T>(T user)
        {
            var s = $"{user.GetType().Name}Id";

            return (Guid)user.GetType().GetProperty(s)?.GetValue(user);
        }
        private string GetUserName<T>(T user)
        {
            var s = $"{user.GetType().Name}Name";

            return (string)user.GetType().GetProperty(s)?.GetValue(user);
        }
        private string GetUserEmail<T>(T user)
        {

            return (string)user.GetType().GetProperty("Email")?.GetValue(user);
        }
    }

}
