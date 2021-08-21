using AutoMapper;
using MetricsManager.DAL.Models;
using MetricsManager.Request;
using MetricsManager.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
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
            CreateMap<AgentItem, AgentResponse>()
                .ForMember(destinationMember => destinationMember.Url,
                memberOptions => memberOptions.MapFrom(sourceMember =>
                sourceMember.AgentUrl.AbsoluteUri));
            CreateMap<AgentRequest, AgentItem>()
                .ForMember(destinationMember => destinationMember.AgentUrl,
                memberOptions => memberOptions.MapFrom(sourceMember =>
                new Uri(sourceMember.Uri)));
            CreateMap<Uri, string>().ConvertUsing<UriStringConverter>();
            CreateMap<string, Uri>().ConvertUsing<StringUriConverter>();
        }
        private class UriStringConverter : ITypeConverter<Uri, string>
        {
            public string Convert(Uri source, string destination, ResolutionContext context)
            {
                return source.AbsoluteUri;
            }
        }

        private class StringUriConverter : ITypeConverter<string, Uri>
        {            

            public Uri Convert(string source, Uri destination, ResolutionContext context)
            {
                return new Uri(source);
            }
        }
    }   

}
