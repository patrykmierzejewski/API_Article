using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .ToList();

            var articlesDtos = _mapper.Map<List<ArticleDetailsDTO>>(articles);

            return Ok(articlesDtos);
        }

        [HttpGet("{name}")]
        public ActionResult<ArticleDetailsDTO> Get(string name)
        {
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());
                

            if (articles == null)
                return NotFound();

            var articlesDto = _mapper.Map<ArticleDetailsDTO>(articles);

            return Ok(articlesDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ArticleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var article = _mapper.Map<Article>(model);

            _articleContext.Articles.Add(article);
            _articleContext.SaveChanges();

            var key = article.Name.Replace(" ", "-").ToLower();

            return Created($"api/article/{key}", null);
        }

        [HttpPut("{name}")]
        public ActionResult Put(string name, [FromBody] ArticleDTO articleDTO)
        {
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            articles.Name = articleDTO.Name;
            articles.Source.SourceName = articleDTO.SourceInformation;
            articles.TextArticle = articleDTO.TextArticle;
            articles.Date = articleDTO.Date;

            _articleContext.SaveChanges();

            return NoContent();
        }


        #region DELETE Methods
        [HttpDelete("{name}")]
        public ActionResult Delete(string name)
        {
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date) //najstarsze
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            _articleContext.RemoveRange(articles);
            _articleContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeleteById/{Id}")]
        public ActionResult DeleteById(int id)
        {
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date) //najstarsze
                .FirstOrDefault(m => m.Id == id);

            if (articles == null)
                return NotFound();

            _articleContext.RemoveRange(articles);
            _articleContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeleteBySplitId/{ids}")]
        public ActionResult DeleteBySplitId(string ids)
        {
            string[] idsString = ids.Split(",").ToArray();
            List<int> idsList = new List<int>();

            foreach (string v in idsString)
                idsList.Add(int.Parse(v));

            IEnumerable<Article> articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date) //najstarsze
                .Where(m => idsList.Contains(m.Id)).ToList();

            if (articles == null)
                return NotFound();

            _articleContext.RemoveRange(articles);
            _articleContext.SaveChanges();

            return NoContent();
        }
        #endregion

    }
}
