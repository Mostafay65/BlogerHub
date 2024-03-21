using System.Net;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using AutoMapper;
using BlogerHub.Data;
using BlogerHub.Models;
using BlogerHub.Models.DTOs;
using BlogerHub.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlogerHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBlogRepository _blogRepository;

        public BlogController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment, IBlogRepository blogRepository)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _blogRepository = blogRepository;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Blog blog = await _blogRepository.Get(id);
            if (blog is null)
            {
                return NotFound(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = {"There are no Blogs with the provided id!"}
                });
            }

            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<BlogResponseDTO>(blog)
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Blog> blogs = await _blogRepository.GetAll();
            if (blogs is null)
            {
                return NotFound(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = {"There are no Blogs!!"}
                });
            }

            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<List<BlogResponseDTO>>(blogs)
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm]BlogRequestDTO blogRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = Errors()
                });
            }
            Blog blog = _mapper.Map<Blog>(blogRequestDto);
            if (blogRequestDto.Media is not null && blogRequestDto.Media.FileName.Length > 0)
            {
                // save the media in the wwwroot
                var FileName = Guid.NewGuid().ToString() + "_" + blogRequestDto.Media.FileName;
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Media", FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await blogRequestDto.Media.CopyToAsync(stream);
                }
                // create the url of the media
                var URL = Path.Combine(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value,
                    HttpContext.Request.PathBase.Value, "Media", FileName);
                // save the url in the newblog.media
                blog.Media = URL;
            }
            blog.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            blog.CreatedAt = blog.UpdatedAt = DateTime.Now;
            blog = await _blogRepository.Create(blog);
            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<BlogResponseDTO>(blog)
            });
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(int id,[FromForm] BlogRequestDTO blogRequestDto)
        {
            Blog blog = await _blogRepository.Get(id);
            if (blog is null)
            {
                return NotFound(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = {"There are no Blogs with the provided id!"}
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = Errors()
                });
            }
            
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != blog.AuthorId)
            {
                return Unauthorized(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorMessages = { "You are not allowed to update this Blog!"}
                });
            }

            blog.Title = blogRequestDto.Title;
            blog.Content = blogRequestDto.Content;
            if (blogRequestDto.Media is not null && blogRequestDto.Media.FileName.Length > 0)
            {
                // delete the current media if exists
                if (blog.Media is not null)
                {
                    var MediaToRemove = Path.GetFileName(blog.Media);
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Media", MediaToRemove);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                // save the new media in the wwwroot
                var FileName = Guid.NewGuid().ToString() + "_" + blogRequestDto.Media.FileName;
                var newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Media", FileName);
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await blogRequestDto.Media.CopyToAsync(stream);
                }

                // create the url of the media
                var URL = Path.Combine(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value,
                    HttpContext.Request.PathBase.Value, "Media", FileName);
                // save the url in the newblog.media
                blog.Media = URL;
            }
            blog.UpdatedAt = DateTime.Now;
            await _blogRepository.Update(blog);
            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<BlogResponseDTO>(blog)
            });
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await _blogRepository.Get(id);
            if (blog is null)
            {
                return NotFound(new APIResponse()
                {
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = {"There are no Blogs with the provided id!"}
                });
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Media", Path.GetFileName(blog.Media));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            await _blogRepository.Remove(id);
            return Ok(new APIResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Result = _mapper.Map<BlogResponseDTO>(blog)
            });
        }
        
        private List<string> Errors()
        {
            var Errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach(var error in value.Errors)
                {
                    Errors.Add(error.ErrorMessage);
                }
            }
            return Errors;
        }
    }
}
