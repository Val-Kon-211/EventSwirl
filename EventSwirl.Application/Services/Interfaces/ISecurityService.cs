namespace EventSwirl.Application.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<(string, string)> Encoder(string password);

        Task<(string, string)> Encoder(string password, string str_salt);
    }
}
