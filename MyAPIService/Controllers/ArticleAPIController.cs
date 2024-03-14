using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.BLL.DTOs;
using System;
using System.Collections.Generic;

namespace MyWebFormApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleAPIController : ControllerBase
    {
        private readonly IArticleBLL _articleBLL;

        public ArticleAPIController(IArticleBLL articleBLL)
        {
            _articleBLL = articleBLL ?? throw new ArgumentNullException(nameof(articleBLL));
        }

        [HttpGet("category/{categoryId}")]
        public ActionResult<IEnumerable<ArticleDTO>> GetArticleByCategory(int categoryId)
        {
            var articles = _articleBLL.GetArticleByCategory(categoryId);
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public ActionResult<ArticleDTO> GetArticleById(int id)
        {
            var article = _articleBLL.GetArticleById(id);
            return Ok(article);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArticleDTO>> GetArticlesWithCategory()
        {
            var articles = _articleBLL.GetArticleWithCategory();
            return Ok(articles);
        }

        [HttpPost]
        public IActionResult CreateArticle([FromBody] ArticleCreateDTO articleDto)
        {
            _articleBLL.Insert(articleDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArticle(int id, [FromBody] ArticleUpdateDTO articleDto)
        {
            articleDto.ArticleID = id; // Ensure ID consistency
            _articleBLL.Update(articleDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(int id)
        {
            _articleBLL.Delete(id);
            return Ok();
        }
    }
}
