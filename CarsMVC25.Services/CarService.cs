using AutoMapper;
using CarsMVC25.Data.Entities;
using CarsMVC25.Data.Repositories;
using CarsMVC25.Data.Repositories.Abstractions;
using CarsMVC25.Services.Abstractions;
using CarsMVC25.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsMVC25.Services
{
    public class CarService : ICarService
    {
        private readonly ICrudRepository<Car> _carRepository;
        private readonly IMapper _mapper;

        public CarService(ICrudRepository<Car> carRepository,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
        public async Task AddAsync(CarDTO model)
        {
            var car = _mapper.Map<Car>(model);  
            await _carRepository.AddAsync(car);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _carRepository.DeleteByIdAsync(id);
        }

        public async Task<List<CarDTO>> GetAllAsync()
        {
            var cars = await _carRepository.GetAllAsync();
            return _mapper.Map<List<CarDTO>>(cars);
        }

        public async Task<CarDTO> GetByIdAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            return _mapper.Map<CarDTO>(car);
        }

        public async Task<List<CarDTO>> GetCarByBrandAsync(string brand)
        {
            var brandCars = await _carRepository.GetByFilterAsync(car => car.Brand == brand);
            return _mapper.Map<List<CarDTO>>(brandCars);
        }

        public async Task UpdateAsync(CarDTO model)
        {
            var car = _mapper.Map<Car>(model);
            await _carRepository.UpdateAsync(car);
        }
    }
}
