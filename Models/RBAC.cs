

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs
{
  enum RBACRole {
    OrgAdmin = 1 << 1,
    OrgRead = 1 << 2,
    OrgWrite = 1 << 3,
    PackageAdmin = 1 << 4,
    PackageRead = 1 << 5,
    PackageWrite = 1 << 6,
  };

  class OrgGrant {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int GrantId { get; set; } 
     public Org Org {get; set;} 
     public RBACRole Grant {get; set;}
     public Account Granted {get; set;} 
  }

  class PackageGrant {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int GrantId { get; set; } 
     public Org Org {get; set;} 
     public RBACRole Grant {get; set;}
     public Account Granted {get; set;} 
  }
}