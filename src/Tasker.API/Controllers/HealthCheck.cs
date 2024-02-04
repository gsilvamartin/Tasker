using Microsoft.AspNetCore.Mvc;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;

namespace Tasker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheck : Controller
{
    private readonly IBaseRepository<Job> _jobRepository;

    public HealthCheck(IBaseRepository<Job> jobRepository)
    {
        _jobRepository = jobRepository;
    }

    [HttpGet]
    public bool IsHealthy()
    {
        return true;
    }

    [HttpGet]
    [Route("version")]
    public string GetVersion()
    {
        _jobRepository.GetAll();
        return "1.0.0";
    }
}