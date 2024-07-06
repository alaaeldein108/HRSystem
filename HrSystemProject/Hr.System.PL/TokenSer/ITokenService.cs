using HrSystem.DAL.Entities;

namespace Hr.System.PL.TokenSer
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser appUser);

    }
}
