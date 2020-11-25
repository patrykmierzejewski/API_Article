using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Entities
{
    public class Source
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public string Information { get; set; }

        public virtual Article Article { get; set; }
        public int ArticleId { get; set; }
    }
}
