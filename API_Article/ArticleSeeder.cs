using API_Article.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article
{
    public class ArticleSeeder
    {
        private readonly ArticleContext _articleContext;
        public ArticleSeeder(ArticleContext articleContext)
        {
            _articleContext = articleContext;
        }
        
        public void Seed()
        {
            if (_articleContext.Database.CanConnect())
            {
                if (!_articleContext.Articles.Any())
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {
            List<Article> articles = DefaultArticles();

            _articleContext.AddRange(articles);
            _articleContext.SaveChanges();
        }

        private List<Article> DefaultArticles()
        {
            var articles = new List<Article>
            {
                new Article
                {
                    Name = "Artykuł 1",
                    Date = DateTime.Now.AddDays(2),
                    TextArticle = "to jest treść 1",

                    Source = new Source
                    {
                        Information = "web",
                        SourceName = "www.wp.pl",
                    },

                    Informations = new List<Information>
                    {
                        new Information
                        {
                            InformationName = "info 1",
                            InformationPriority = "1"
                        },

                        new Information
                        {
                            InformationName = "inf 2",
                            InformationPriority = "2"
                        }
                    }
                },

                new Article
                {
                    Name = "Artykuł 2",
                    Date = DateTime.Now.AddDays(2),
                    TextArticle = "to jest treść 2",

                    Source = new Source
                    {
                        Information = "web 2",
                        SourceName = "www.ONET.pl",
                    },

                    Informations = new List<Information>
                    {
                        new Information
                        {
                            InformationName = "infoAAXX 22",
                            InformationPriority = "2"
                        },

                        new Information
                        {
                            InformationName = "infAAXXX 3",
                            InformationPriority = "1"
                        }
                    }
                }

            };

            return articles;
        }
    }
}
