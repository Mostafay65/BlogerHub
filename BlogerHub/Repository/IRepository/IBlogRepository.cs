using BlogerHub.Models;
using BlogerHub.Models.DTOs;

namespace BlogerHub.Repository.IRepository;

public interface IBlogRepository
{
    public Task<Blog> Get(int id);
    public Task<List<Blog>> GetAll();
    public Task<Blog> Remove(int id);
    public Task<Blog> Create(Blog blog);
    public Task<Blog> Update(Blog blog);
}