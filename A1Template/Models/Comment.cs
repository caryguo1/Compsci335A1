using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A1Template.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserComment { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string IP { get; set; }
    }
}
