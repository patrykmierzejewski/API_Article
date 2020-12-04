using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Helpers;
using API_Article.Identity;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Article.Controllers
{
    [Route("api/article")]
    [Authorize(Policy = "HasActive")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleContext _articleContext;
        private readonly IMapper _mapper;

        public ArticleController(ArticleContext articleContext, IMapper mapper)
        {
            _articleContext = articleContext;
            _mapper = mapper;
        }

        #region GET Methods
        [HttpGet]
        public ActionResult<List<ArticleDetailsDTO>> Get()
        {
            var a = HttpContext.User.Identity.Name;
            var b = HttpContext.User.Identities.Select(x=> x.Claims);

            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .ToList();

            var articlesDtos = _mapper.Map<List<ArticleDetailsDTO>>(articles);

            return Ok(articlesDtos);
        }

        [HttpGet("{name}")]
        [Authorize(Policy = "HasCountry")] // własna autrozyzacja
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

        #endregion

        #region POST Methods
        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
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
        #endregion

        #region PUT Methods
        [HttpPut("{name}")]
        [Authorize(Roles = "Admin,Moderator")]
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
        #endregion

        #region DELETE Methods
        [HttpDelete("{name}")]
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Delete(string name)
        {
            var articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date) //najstarsze
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            _articleContext.Remove(articles);
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

            _articleContext.Remove(articles);
            _articleContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("DeleteBySplitId/{splitName}")]
        public ActionResult DeleteBySplitId(string splitName)
        {
            List<int> idsList = StaticHelpers.GetIdsBySplitName(splitName);

            IEnumerable<Article> articles = _articleContext.Articles
                .Include(m => m.Source)
                .Include(n => n.Informations)
                .OrderBy(x => x.Date) //najstarsze
                .Where(m => idsList.Contains(m.Id));

            if (articles == null)
                return NotFound();

            _articleContext.RemoveRange(articles);
            _articleContext.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
