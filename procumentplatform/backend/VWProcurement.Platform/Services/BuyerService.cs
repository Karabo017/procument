using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Platform.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuyerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BuyerDto>> GetAllBuyersAsync()
        {
            var buyers = await _unitOfWork.Buyers.GetAllAsync();
            return buyers.Select(MapToDto);
        }

        public async Task<BuyerDto?> GetBuyerByIdAsync(Guid id)
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(id);
            return buyer != null ? MapToDto(buyer) : null;
        }

        public async Task<BuyerDto?> GetBuyerByEmailAsync(string email)
        {
            var buyer = await _unitOfWork.Buyers.GetByEmailAsync(email);
            return buyer != null ? MapToDto(buyer) : null;
        }

        public async Task<BuyerDto> CreateBuyerAsync(CreateBuyerDto dto)
        {
            var buyer = new Buyer
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Department = dto.Department,
                EmployeeId = dto.EmployeeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Buyers.AddAsync(buyer);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(buyer);
        }

        public async Task<BuyerDto?> UpdateBuyerAsync(Guid id, UpdateBuyerDto dto)
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(id);
            if (buyer == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) buyer.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email)) buyer.Email = dto.Email;
            if (dto.PhoneNumber != null) buyer.PhoneNumber = dto.PhoneNumber;
            if (dto.Department != null) buyer.Department = dto.Department;
            if (dto.EmployeeId != null) buyer.EmployeeId = dto.EmployeeId;
            if (dto.IsActive.HasValue) buyer.IsActive = dto.IsActive.Value;
            
            buyer.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Buyers.Update(buyer);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(buyer);
        }

        public async Task<bool> DeleteBuyerAsync(Guid id)
        {
            var buyer = await _unitOfWork.Buyers.GetByIdAsync(id);
            if (buyer == null) return false;

            _unitOfWork.Buyers.Remove(buyer);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BuyerDto>> GetActiveBuyersAsync()
        {
            var buyers = await _unitOfWork.Buyers.GetActiveBuyers();
            return buyers.Select(MapToDto);
        }

        private static BuyerDto MapToDto(Buyer buyer)
        {
            return new BuyerDto
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Email = buyer.Email,
                PhoneNumber = buyer.PhoneNumber,
                Department = buyer.Department,
                EmployeeId = buyer.EmployeeId,
                IsActive = buyer.IsActive,
                CreatedAt = buyer.CreatedAt
            };
        }
    }
}
