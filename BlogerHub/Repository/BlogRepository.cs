using AutoMapper;
using BlogerHub.Data;
using BlogerHub.Models;
using BlogerHub.Models.DTOs;
using BlogerHub.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlogerHub.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly ApplicationDbContext _context;

    public BlogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Blog?> Get(int id)
    {
        return await _context.Blogs.FindAsync(id);
    }

    public async Task<List<Blog>> GetAll()
    {
        return await _context.Blogs.ToListAsync();
    }

    public async Task<Blog> Remove(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog> Create(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog> Update(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
}