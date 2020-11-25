using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_Article.Controllers
{
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleContext _articleContext;
        private readonly IMapper _mapper;

        public ArticleController(ArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<ArticleDetailsDTO>> Get()
        {
            var articles = _articleContext.Articles.ToList();
            var info = _articleContext.Informations.ToList();
            var sources = _articleContext.Sources.ToList();

            var articlesDtos = _mapper.Map<List<ArticleDetailsDTO>>(articles);

            return Ok(articlesDtos);
        }

        [HttpGet("{name}")]
        public ActionResult<ArticleDetailsDTO> Get(string name)
        {
            var articles = _articleContext.Articles
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            var articlesDto = _mapper.Map<ArticleDetailsDTO>(articles);

            return Ok(articlesDto);
        }
        

        [HttpPost]
        public ActionResult Post([FromBody]ArticleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var article = _mapper.Map<Article>(model);

            _articleContext.Articles.Add(article);
            _articleContext.SaveChanges();

            var key = article.Name.Replace(" ", "-").ToLower();

            return Created($"api/article/{key}" , null);
        }

    }
}
