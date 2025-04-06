using CarsMVC25.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsMVC25.Services.Abstractions
{
    public interface ICarService
    {
        Task<List<CarDTO>> GetAllAsync();
        Task<CarDTO> GetByIdAsync(int id);
        Task<List<CarDTO>> GetCarByBrandAsync(string brand);
        Task AddAsync(CarDTO model);
        Task UpdateAsync(CarDTO model);
        Task DeleteByIdAsync(int id);
    }
}
