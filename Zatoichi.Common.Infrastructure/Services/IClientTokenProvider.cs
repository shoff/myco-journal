namespace Zatoichi.Common.Infrastructure.Services
{
    using System.Threading.Tasks;

    public interface IClientTokenProvider
    {
        Task<string> GetTokenAsync();
    }
}