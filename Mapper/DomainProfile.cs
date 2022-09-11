using AutoMapper;
using Movie.Database.Entity;
using Movie.Models;

namespace Movie.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Movies, MoviesDetalisDto>();
            CreateMap<MoviesDto, Movies>()
                .ForMember(src => src.Poster, opt => opt.Ignore());
        }



    }
    }

