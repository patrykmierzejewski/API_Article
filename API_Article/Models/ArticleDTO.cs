using API_Article.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Models
{
    public class ArticleDTO
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string TextArticle { get; set; }

        public string SourceName { get; set; }

        public string SourceInformation { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

    }
}
