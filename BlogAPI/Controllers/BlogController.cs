using BlogAPI.Models;
using BlogAPI.Models.UserManagement;
using BlogAPI.Models.ViewModels;
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

        [HttpPost]
        public HttpResponseMessage Create(Blog newBlog, int authorId)
        {
            // Tesztelni kell, hogy a modell valid-e, vagyis minden elvárt mezője kivan-e töltve
            // Tesztelni kell, hogy nem null az értéke
            // Ha valahol hiba van, akkor hibaüzenettel kell visszatérni

            if (!ModelState.IsValid || newBlog == null)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Blog didn't create, because it is null or invalid.");

                return responseMessage;
            }

            try
            {
                User? author = _db.Users
                .FirstOrDefault(u => u.Id == authorId);

                if (author == null)
                {
                    throw new ArgumentNullException("Blog didn't create, because author didn't exist.");
                }

                newBlog.Author = author;

                _db.Blogs.Add(newBlog);
                _db.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent(ex.Message);

                return responseMessage;
            }
            catch (SqlException)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Blog didn't create, because database server didn't communicate.");

                return responseMessage;
            }
            catch (Exception)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Unknown error.");

                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public IQueryable<BlogViewModel> Read(int page)
        {
            IQueryable<BlogViewModel> blogs;

            try
            {
                blogs = _db.Blogs
                   .Where(b => b.IsArchived == false)
                   .Include(b => b.Author)
                   .OrderByDescending(b => b.PublishDate)
                   .Skip((page - 1) * 10)
                   .Take(10)
                   .Select(b => new BlogViewModel
                   {
                       Id = b.Id,
                       Title = b.Title,
                       Entry = b.Entry,
                       PublishDate = b.PublishDate,
                       IsArchived = b.IsArchived,
                       AuthorNickname = b.Author.IsArchived ? "Deleted user" : b.Author.Nickname
                   });
            }
            catch (Exception)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }

            return blogs;
        }

        [HttpPost]
        public HttpResponseMessage Update(Blog updatedBlog, int updatedId)
        {
            if(!ModelState.IsValid || updatedBlog == null)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Blog didn't update, because blog state was invalid or null.");

                return responseMessage;
            }

            try
            {
                Blog? targetBlog = _db.Blogs
                    .FirstOrDefault(b => b.Id == updatedId);

                if(targetBlog == null)
                {
                    throw new ArgumentNullException("The blog you are trying to update does not exist.");
                }

                targetBlog.Title = updatedBlog.Title;
                targetBlog.Entry = updatedBlog.Entry;

                _db.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent(ex.Message);

                return responseMessage;
                
            }
            catch (SqlException)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Blog didn't update, because database server didn't communicate.");

                return responseMessage;
            }
            catch (Exception)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Unknown error.");

                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage Delete(int blogId)
        {
            try
            {
                Blog? deletedBlog = _db.Blogs
                    .FirstOrDefault(b => b.Id == blogId);

                if(deletedBlog == null)
                {
                    throw new ArgumentNullException("The blog you are trying to archived does not exist.");
                }

                deletedBlog.IsArchived = true;
                _db.SaveChanges();

            }
            catch(ArgumentNullException ex)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent(ex.Message);

                return responseMessage;
            }
            catch(SqlException ex)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Blog didn't archive, because database server didn't communicate.");

                return responseMessage;
            }
            catch (Exception)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("Unknown error.");

                return responseMessage;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
