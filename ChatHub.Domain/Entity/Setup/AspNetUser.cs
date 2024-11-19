using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHub.Domain.Entity.setup;

public class AspNetUser
{


    public string Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; }
    public string Discriminator { get; set; }

  //  public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
  //  public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
//public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
}
