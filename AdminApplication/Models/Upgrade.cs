using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AdminApplication.Models
{
    [Table("upgrade")]
    public class Upgrade
    {
        public int Id { get; private set; }
        public DateTime CreationDate { get; set; }

        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d+.\d+.\d+.\d+$")]
        public string Version { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        [NotMapped]
        public IFormFile ZipFile { get; set; }
    }
}
