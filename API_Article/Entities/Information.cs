using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Entities
{
    public class Information
    {
        public int Id { get; set; }
        public string InformationName { get; set; }
        public string InformationPriority { get; set; }

        public virtual Article Article { get; set; }
        public int ArticleId { get; set; }

    }
}
