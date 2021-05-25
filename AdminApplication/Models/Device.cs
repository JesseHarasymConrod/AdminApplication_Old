using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApplication.Models
{
    [Table("device")]
    public class Device
    {
        public int Id { get; private set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string SoftwareRevision { get; set; }
        public int ClientId { get; set; }
    }
}
