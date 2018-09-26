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


        private string _dependencies;
        [NotMapped]
        public string[] Dependencies
        {
            get { 
                if(_dependencies == null)
                {
                    return new string[0];
                }

                return _dependencies.Split(','); 
            }
            set
            {
                _dependencies = string.Join($"{','}", value);
            }
        }
    }
}
