using NewsPage.Models.RequestDTO;

namespace NewsPage.repositories.interfaces
{
    public interface IPositionPriceRepository{
       Task<PositionPrice> GetByType(string PositionType);
       Task<PositionPrice> GetById(Guid PositionTypeId);
       Task<PositionPrice> Create(PositionPriceCreateDto dto);
       Task<List<PositionPrice>> GetPrices();
        Task<PositionPrice?> Update(Guid id, PositionPriceUpdateDto dto);
        Task<bool> Delete(Guid id);
    }
}