

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs
{
  public class Account
  {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int AccountId { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
  }
}