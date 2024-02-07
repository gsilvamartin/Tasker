using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Base;
using Tasker.API.Model.Jobs;
using Tasker.Repository.Entity;
using Tasker.Repository.Interfaces;

namespace Tasker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<Job> _jobRepository;

    public JobsController(IBaseRepository<Job> jobRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var jobs = await _jobRepository.GetAll();
        var result = _mapper.Map<IEnumerable<JobDto>>(jobs);

        return Success(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var job = await _jobRepository.GetById(id);
        var result = _mapper.Map<JobDto>(job);

        return Success(result);
    }
}