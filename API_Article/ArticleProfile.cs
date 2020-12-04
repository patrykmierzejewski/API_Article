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
            //GET
            CreateMap<Article, ArticleDetailsDTO>()
                .ForMember(x => x.SourceName, map => map.MapFrom(a => a.Source.SourceName));

            //POST*************************
            //jezeli nazwy się pokrywają, nie potrzeba tworzyć mapy
            CreateMap<ArticleDTO, Article>()
                .ForMember(x => x.Source, map => map.MapFrom(i => new Source
                {
                    SourceName = i.SourceName,
                    Information = i.SourceInformation
                }));

            //zmienne pokrywają się nazwami
            //CreateMap<ArticleDTO, Information>();

            //****************************
            CreateMap<InformationDTO, Information>()
                .ReverseMap() ;

            CreateMap<UserDTO, User>()
                .ReverseMap();
        }
    }

}
