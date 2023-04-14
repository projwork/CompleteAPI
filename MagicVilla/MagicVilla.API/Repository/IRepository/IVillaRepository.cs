using MagicVilla.API.Models.Dto;
using MagicVilla.API.Models;

namespace MagicVilla.API.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<VillaDTO>> GetAll();
        Task<VillaDTO?> GetById(int id);
        Task<VillaDTO?> GetByName(string name);
        Task<Villa> Create(VillaCreateDTO createDto);
        Task<bool> Update(VillaUpdateDTO updateDto);
        Task<bool> Delete(int id);
    }
}
