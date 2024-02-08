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

    [HttpGet]
    [Route("isJobNameUnique/{jobName}")]
    public async Task<IActionResult> IsJobNameUnique(string jobName)
    {
        var job = await _jobRepository.GetWhere(x => x.Name == jobName);

        return Success(!job.Any());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JobDto jobDto)
    {
        await _jobRepository.Add(new Job
        {
            Name = jobDto.Name,
            MaximumRetries = jobDto.MaximumRetries,
            CurrentlyRetries = 0,
            StatusId = 2,
            SchedulingTypeId = 1,
            InProgress = false,
            CreationDate = DateTime.Now,
            LastUpdate = DateTime.Now,
            CronExpression = jobDto.CronExpression,
            IntervalInMinutes = jobDto.IntervalInMinutes,
        });

        return Success();
    }
}