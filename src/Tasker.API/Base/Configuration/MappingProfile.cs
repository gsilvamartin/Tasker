using AutoMapper;
using Tasker.API.Model.Jobs;
using Tasker.Repository.Entity;

namespace Tasker.API.Base.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Job, JobDto>();
    }
}