namespace Zatoichi.Common.Infrastructure.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    //  only for use with Identity server. DO NOT USE AS A EMS USER OBJECT!!!!
    public interface IAuthUser
    {
        string SubjectId { get; set; }
        string ProviderName { get; set; }
        string ProviderSubjectId { get; set; }
        bool IsActive { get; set; }
        ICollection<Claim> Claims { get; set; }
        string Id { get; set; }
        string UserName { get; set; }
        string NormalizedUserName { get; set; }
        string Email { get; set; }
        string NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string PasswordHash { get; set; }
        string SecurityStamp { get; set; }
        string ConcurrencyStamp { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool TwoFactorEnabled { get; set; }
        DateTimeOffset? LockoutEnd { get; set; }
        bool LockoutEnabled { get; set; }
        int AccessFailedCount { get; set; }
    }
}