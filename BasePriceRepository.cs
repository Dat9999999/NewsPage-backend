using Microsoft.EntityFrameworkCore;
using NewsPage.data;
using NewsPage.Models.entities;
using NewsPage.repositories.interfaces;

namespace NewsPage.Repositories
{
    

    public class BasePriceRepository : IBasePriceRepository
    {
        private readonly ApplicationDbContext _context;
        public BasePriceRepository(ApplicationDbContext context){
            _context = context;
        }

        public async Task<BasePrice> Create(BasePriceCreateDto dto)
        {
            BasePrice bp = new BasePrice{
                Id = new Guid(),
                PaidType = dto.PaidType,
                Cost = dto.Price
            };
            await _context.BasePrices.AddAsync(bp);
            await _context.SaveChangesAsync();
            return bp;
        }

        public async Task<bool> Delete(Guid id)
        {
            var bp = await _context.BasePrices.FirstOrDefaultAsync(p => p.Id == id);
            if(bp == null) return false;
            _context.Remove(bp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BasePrice> GetById(Guid BasePriceId)
        {
            return await _context.BasePrices.FirstOrDefaultAsync(b => b.Id == BasePriceId);
        }

        public async Task<List<BasePrice>> GetPrices()
        {
            return await _context.BasePrices.ToListAsync();
        }

        public async Task<BasePrice?> Update(Guid id, BasePriceUpdateDto dto)
        {
            BasePrice bp = await GetById(id);
            if(bp == null) return null;
            bp.Cost = dto.Price;
            bp.PaidType = dto.paidType;
            return bp;
        }
    }
}