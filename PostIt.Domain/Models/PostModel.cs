using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PostIt.Domain.Models
{
    public class Post
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Title { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(5000)]
        public required string Text { get; set; }

        public bool IsActive { get; set; }

        public long SubjectId { get; set; }

        public required string SubjectName { get; set; }

        public long UserId { get; set; }

        public required string UserName { get; set; }
    }
}
