using AutoMapper;
using CarsMVC25.Data.Entities;
using CarsMVC25.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsMVC25.Services.Profiles
{
    class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDTO>()
                .ReverseMap();
        }
    }
}
