using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs
{
    [NotMapped]
    public class SemVer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SemVerId { get; set; }
        public string Version {get; set;}
        public Package Package {get; set;}
    }
}
