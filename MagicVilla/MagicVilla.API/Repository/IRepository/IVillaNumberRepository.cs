using MagicVilla.API.Models.Dto;
using MagicVilla.API.Models;

namespace MagicVilla.API.Repository.IRepository
{
    public interface IVillaNumberRepository
    {
        Task<List<VillaNumberDTO>> GetAll();
        Task<VillaNumberDTO?> GetById(int id);
        Task<VillaNumber> Create(VillaNumberCreateDTO createDto);
        Task<bool> Update(VillaNumberUpdateDTO updateDto);
        Task<bool> Delete(int id);
    }
}
