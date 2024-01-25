using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Models;
using Personal.Blog.Services;
 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesApiController : ControllerBase
    {
        private readonly IArticleService _articleService; 

        public ArticlesApiController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return Ok(articles);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

      
        [HttpPost]
        public async Task<ActionResult<Article>> CreateArticle([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _articleService.CreateArticleAsync(article);
            return CreatedAtAction("GetArticleById", new { id = article.ArticleId }, article);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            await _articleService.UpdateArticleAsync(article);
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }

    }
}
