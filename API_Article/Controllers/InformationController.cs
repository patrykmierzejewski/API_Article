using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API_Article.Controllers
{
    [Route("api/article/{Name}/information")]
    public class InformationController : ControllerBase
    {
        private readonly ArticleContext _articleContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public InformationController(ArticleContext articleContext, IMapper mapper, ILogger<InformationController> logger)
        {
            _logger = logger;
            _articleContext = articleContext;
            _mapper = mapper;
        }

        #region GET
        [HttpGet]
        public ActionResult Get(string name)
        {
            var articles = _articleContext.Articles
                .Include(n => n.Informations)
                .OrderBy(x => x.Date)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            var articlesDto = _mapper.Map<List<InformationDTO>>(articles.Informations);

            return Ok(articlesDto);
        }
        #endregion

        #region POST
        [HttpPost]
        public ActionResult POST(string name, [FromBody]InformationDTO informationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Article articles = _articleContext.Articles
                .Include(n => n.Informations)
                .OrderBy(x => x.Date)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());//first article in db
            
            if (articles == null)
                return NotFound(); //404

            Information article = _mapper.Map<Information>(informationDTO);

            articles.Informations.Add(article);
            _articleContext.SaveChanges();

            return Created($"api/article/{name}", null);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public ActionResult DeleteById(string name, int id)
        {
            Article articles = _articleContext.Articles
               .Include(n => n.Informations)
               .OrderBy(x => x.Date)
               .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            Information information = _articleContext.Informations.FirstOrDefault(x => x.Id == id);
            if (information == null)
                return NotFound();

            _articleContext.Informations.Remove(information);
            _articleContext.SaveChanges();

            _logger.LogWarning($"Informacja o nr id={information.Id} dla artukułu-({articles.Name}) została usnięta");

            return NoContent();
        }

        [HttpDelete]
        public ActionResult DeleteAll(string name)
        {
            Article articles = _articleContext.Articles
               .Include(n => n.Informations)
               .OrderBy(x => x.Date)
               .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (articles == null)
                return NotFound();

            _articleContext.Informations.RemoveRange(articles.Informations);
            _articleContext.SaveChanges();

            _logger.LogWarning($"Informacje dla artukułu-({articles.Name}) zostały usnięte");

            return NoContent();
        }
        #endregion
    }
}
