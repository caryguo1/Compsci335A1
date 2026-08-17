//using System;
//using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

namespace A1Template.Dtos
{
    public class CommentInputDto
    {
        [Required]
        public string UserComment { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
