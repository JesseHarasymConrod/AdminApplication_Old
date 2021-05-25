using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApplication.Models
{
    [Table("client")]
    public class Client
    {
        public int Id { get; private set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
    }
}
