using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Models
{
    public class ArticleDetailsDTO
    {
        //Article
        public string Name { get; set; }
        public string TextArticle { get; set; }
        public DateTime Date { get; set; }
        //Source
        public string SourceName { get; set; }
        //Information
        public List<InformationDTO> Informations { get; set; }

    }
}
