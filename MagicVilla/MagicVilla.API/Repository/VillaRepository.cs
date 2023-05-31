using MagicVilla.API.Data;
using MagicVilla.API.Models.Dto;
using MagicVilla.API.Models;
using MagicVilla.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.API.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<VillaDTO>> GetAll(int pageNumber, int pageSize)
        {
            var villas = await (from v in _db.Villas
                                select new VillaDTO()
                                {
                                    Id = v.Id,
                                    Name = v.Name,
                                    Amenity = v.Amenity,
                                    Details = v.Details,
                                    ImageUrl = v.ImageUrl,
                                    Occupancy = v.Occupancy,
                                    Rate = v.Rate,
                                    Sqft = v.Sqft
                                }).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return villas;
        }

        public async Task<VillaDTO?> GetById(int id)
        {
            var villa = await (from v in _db.Villas.Where(v => v.Id == id)
                               select new VillaDTO()
                               {
                                   Id = v.Id,
                                   Name = v.Name,
                                   Amenity = v.Amenity,
                                   Details = v.Details,
                                   ImageUrl = v.ImageUrl,
                                   Occupancy = v.Occupancy,
                                   Rate = v.Rate,
                                   Sqft = v.Sqft
                               }).FirstOrDefaultAsync();

            return villa;
        }

        public async Task<VillaDTO?> GetByName(string name)
        {
            var villa = await (from v in _db.Villas.Where(v => v.Name.ToLower() == name.ToLower())
                               select new VillaDTO()
                               {
                                   Id = v.Id,
                                   Name = v.Name,
                                   Amenity = v.Amenity,
                                   Details = v.Details,
                                   ImageUrl = v.ImageUrl,
                                   Occupancy = v.Occupancy,
                                   Rate = v.Rate,
                                   Sqft = v.Sqft
                               }).FirstOrDefaultAsync();

            return villa;
        }

        public async Task<Villa> Create(VillaCreateDTO createDto)
        {
            var villa = new Villa()
            {
                Amenity = createDto.Amenity,
                Details = createDto.Details,
                ImageUrl = createDto.ImageUrl,
                Name = createDto.Name,
                Occupancy = createDto.Occupancy,
                Rate = createDto.Rate,
                Sqft = createDto.Sqft
            };
            await _db.Villas.AddAsync(villa);
            await _db.SaveChangesAsync();

            return villa;
        }

        public async Task<bool> Update(VillaUpdateDTO updateDto)
        {
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == updateDto.Id);

            if (villa == null)
            {
                return false;
            }

            villa.Name = updateDto.Name;
            villa.Amenity = updateDto.Amenity;
            villa.Details = updateDto.Details;
            villa.ImageUrl = updateDto.ImageUrl;
            villa.Occupancy = updateDto.Occupancy;
            villa.Rate = updateDto.Rate;
            villa.Sqft = updateDto.Sqft;

            _db.Villas.Update(villa);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return false;
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
