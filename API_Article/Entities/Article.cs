using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TextArticle { get; set; }
        public DateTime Date { get; set; }

        public virtual Source Source { get; set; }
        public virtual List<Information> Informations { get; set; }
        
        public virtual User User { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}
