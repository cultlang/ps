

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs
{
  enum RBACRole {
    OrgOwner = 1 << 0,
    OrgRead = 1 << 1,
    OrgWrite = 1 << 2,
    PackageOwner = 1 << 3,
    PackageRead = 1 << 4,
    PackageWrite = 1 << 5,
  };
}