using AutoMapper;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Models;

namespace Weather.Tracker.Infrastructure.Profiles;

public class WeatherEntityProfile : Profile
{
    public WeatherEntityProfile()
    {
        CreateMap<WeatherEntry, WeatherEntryDTO>();
        CreateMap<OpenMapWeatherResponse, WeatherEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Weather[0].Description))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Sys.Country))
            .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Main.Temp))
            .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.Main.Humidity))
            .ForMember(dest => dest.WindDegree, opt => opt.MapFrom(src => src.Wind.Deg))
            .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.Wind.Speed))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Dt));
    }
}