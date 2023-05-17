using MagicVilla.API.Data;
using MagicVilla.API.Models.Dto;
using MagicVilla.API.Models;
using MagicVilla.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.API.Repository
{
    public class VillaNumberRepository : IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<VillaNumberDTO>> GetAll()
        {
            var villas = await (from v in _db.VillaNumbers.Include(v => v.Villa)
                                from vi in _db.Villas.Where(a => a.Id == v.VillaID).DefaultIfEmpty()
                                select new VillaNumberDTO()
                                {
                                    VillaNo = v.VillaNo,
                                    VillaID = v.VillaID,
                                    SpecialDetails = v.SpecialDetails,
                                    Villa = vi == null ? null : new VillaDTO()
                                    {
                                        Amenity = vi.Amenity,
                                        Details = vi.Details,
                                        Id = vi.Id,
                                        ImageUrl = vi.ImageUrl,
                                        Occupancy = vi.Occupancy,
                                        Name = vi.Name,
                                        Rate = vi.Rate,
                                        Sqft = vi.Sqft
                                    }
                                }).ToListAsync();
            return villas;
        }

        public async Task<VillaNumberDTO> GetById(int id)
        {
            var villa = await (from v in _db.VillaNumbers.Where(v => v.VillaNo == id)
                               from vi in _db.Villas.Where(a => a.Id == v.VillaID).DefaultIfEmpty()
                               select new VillaNumberDTO()
                               {
                                   VillaNo = v.VillaNo,
                                   VillaID = v.VillaID,
                                   SpecialDetails = v.SpecialDetails,
                                   Villa = vi == null ? null : new VillaDTO()
                                   {
                                       Amenity = vi.Amenity,
                                       Details = vi.Details,
                                       Id = vi.Id,
                                       ImageUrl = vi.ImageUrl,
                                       Occupancy = vi.Occupancy,
                                       Name = vi.Name,
                                       Rate = vi.Rate,
                                       Sqft = vi.Sqft
                                   }
                               }).FirstOrDefaultAsync();

            return villa;
        }

        public async Task<VillaNumber> Create(VillaNumberCreateDTO createDto)
        {
            var villaNumber = new VillaNumber()
            {
                VillaNo = createDto.VillaNo,
                VillaID = createDto.VillaID,
                SpecialDetails = createDto.SpecialDetails,
                CreatedDate = DateTime.UtcNow
            };

            await _db.VillaNumbers.AddAsync(villaNumber);
            await _db.SaveChangesAsync();

            return villaNumber;
        }

        public async Task<bool> Update(VillaNumberUpdateDTO updateDto)
        {
            var villa = await _db.VillaNumbers.FirstOrDefaultAsync(v => v.VillaNo == updateDto.VillaNo);

            if (villa == null)
            {
                return false;
            }

            villa.VillaNo = updateDto.VillaNo;
            villa.VillaID = updateDto.VillaID;
            villa.SpecialDetails = updateDto.SpecialDetails;
            villa.UpdatedDate = DateTime.UtcNow;

            _db.VillaNumbers.Update(villa);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var villa = await _db.VillaNumbers.FirstOrDefaultAsync(v => v.VillaNo == id);

            if (villa == null)
            {
                return false;
            }

            _db.VillaNumbers.Remove(villa);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
