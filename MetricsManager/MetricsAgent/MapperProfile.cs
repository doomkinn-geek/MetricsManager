using AutoMapper;
using MetricsAgent.DAL.Models;
using MetricsAgent.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<Metric, MetricDto>()
                .ForMember(destinationMember => destinationMember.Time, 
                memberOptions => memberOptions.MapFrom(sourceMember =>
                new DateTime(sourceMember.Time.Ticks)));
        }
    }

}
