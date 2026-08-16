using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A1Template.Models
{
    public class Sign
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
