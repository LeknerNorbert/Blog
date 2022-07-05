using BlogAPI.Models;
using BlogAPI.Models.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public BlogController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]BlogDto blog)
        {
            if (!ModelState.IsValid || blog == null)
            {
                return BadRequest("Blog is not correct");
            }

            Account? author = await _db.Accounts.FindAsync(blog.AuthorId);

            if (author == null)
            {
                return BadRequest("Author does not exist");
            }

            Blog createdBlog = new Blog()
            {
                Title = blog.Title,
                Entry = blog.Entry,
                Author = author
            };

            await _db.AddAsync(createdBlog);
            await _db.SaveChangesAsync();

            return Ok("Blog created");
        }

        [HttpGet, Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Read(int page)
        {
            var blogs = await _db.Blogs
                .Where(b => b.IsArchived == false)
                .Include(b => b.Author)
                .Skip(10 * (page - 1))
                .Take(10)
                .Select(b => new 
                {
                    Id = b.Id,
                    Title = b.Title,
                    Entry = b.Entry,
                    PublishDate = b.PublishDate,
                    AuthorId = b.Author.Id,
                    AuthorUsername = b.Author.Username
                })
                .ToListAsync();

            return Ok(blogs);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReadArchived()
        {
            IEnumerable<Blog> blogs = await _db.Blogs
                .Where(b => b.IsArchived == true)
                .ToListAsync();

            if (blogs.Count() == 0)
            {
                return BadRequest("No blog archived");
            }

            return Ok(blogs);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] BlogDto blog)
        {
            if (!ModelState.IsValid || blog == null)
            {
                return BadRequest("Blog is not corrent");
            }

            Blog? updatedBlog = await _db.Blogs.FindAsync(blog.Id);

            if (updatedBlog == null)
            {
                return BadRequest("Target blog does not exist");
            }

            if (updatedBlog.IsArchived) {
                return BadRequest("Traget blog is archived, you can not edit it");
            }

            updatedBlog.Title = blog.Title;
            updatedBlog.Entry = blog.Entry;
            await _db.SaveChangesAsync();

            return Ok("Blog updated");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromBody]int id)
        {
            Blog? deletedBlog = await _db.Blogs.FindAsync(id);

            if(deletedBlog == null)
            {
                return NotFound("Blog does not exist");
            }

            deletedBlog.IsArchived = true;
            await _db.SaveChangesAsync();

            return Ok("Blog deleted");
        }
    }
}
