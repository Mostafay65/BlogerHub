using System.Net;

namespace BlogerHub.Models.DTOs;

public class APIResponse
{
    public bool Success { get; set; } = true;
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; } = new List<string>();
    public object? Result { get; set; } = null;
}