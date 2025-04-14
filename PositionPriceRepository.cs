using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Enums;
using NewsPage.Models.RequestDTO;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories
{

    public class PositionPriceRepository : IPositionPriceRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionPriceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PositionPrice> Create(PositionPriceCreateDto dto)
        {
            PositionPrice p = new PositionPrice{
                Id = new Guid(),
                PositionType = dto.PositionType,
                Cost = dto.Cost
            };
            await _context.PositionPrices.AddAsync(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.PositionPrices.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.PositionPrices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PositionPrice> GetById(Guid PositionTypeId)
        {
            return await _context.PositionPrices.FirstOrDefaultAsync(p => p.Id == PositionTypeId);
        }

        public async Task<PositionPrice> GetByType(string positionTypeStr)
        {
            if (!Enum.TryParse<AdvertisePosition>(positionTypeStr, true, out var positionTypeEnum))
            {
                throw new ArgumentException("Loại vị trí không hợp lệ.");
            }

            return await _context.PositionPrices.FirstOrDefaultAsync(p => p.PositionType == positionTypeEnum);
        }

        public async Task<List<PositionPrice>> GetPrices()
        {
            return await _context.PositionPrices.ToListAsync();
        }

        public async Task<PositionPrice?> Update(Guid id, PositionPriceUpdateDto dto)
        {
            var p = await _context.PositionPrices.FirstOrDefaultAsync(c => c.Id == id);
            p.Cost = dto.Price;
            p.PositionType = dto.PositionType;
            await _context.SaveChangesAsync();
            return p;
        }
    }

}