using Microsoft.AspNetCore.Identity;
using Speurzoekers.Data.Entities.User;
using System;

namespace Speurzoekers.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual UserInfoEntity UserInfo { get; set; }        
    }
}
