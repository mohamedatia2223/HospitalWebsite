namespace Hospital.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model);
        public Task<AuthModel> GetTokenAsync(TokenRequestModel model);


    }
}
