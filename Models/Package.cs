using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cs
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }
        public string Name {get; set;}

        public Org Org {get; set;}
        public string SourceRef {get; set;}

        public ICollection<SemVer> Dependencies { get; set; }
        public ICollection<Artifact> Artifacts { get; set; }
    }
}
