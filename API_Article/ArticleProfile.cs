using API_Article.Entities;
using API_Article.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDetailsDTO>()
                .ForMember(x => x.SourceName, map => map.MapFrom(a => a.Source.SourceName))
                .ForMember(x => x.InformationsName, map => map.MapFrom(a => a.Informations.Select(x=>x.InformationName)));

            //jezeli nazwy się pokrywają, nie potrzeba tworzyć mapy
            CreateMap<ArticleDTO, Article>()
                .ForMember(x => x.Name, map => map.MapFrom(x => x.Name))
                .ForMember(x => x.TextArticle, map => map.MapFrom(x => x.TextArticle))
                .ForMember(x => x.Source, map => map.MapFrom(i => new Source
                {
                    SourceName = i.SourceName,
                    Information = i.SourceInformation
                }));
        }

    }
}
