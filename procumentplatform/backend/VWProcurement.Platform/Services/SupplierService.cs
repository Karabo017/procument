using VWProcurement.Core.DTOs;
using VWProcurement.Core.Interfaces;
using VWProcurement.Core.Models;

namespace VWProcurement.Platform.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
            return suppliers.Select(MapToDto);
        }

        public async Task<SupplierDto?> GetSupplierByIdAsync(Guid id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            return supplier != null ? MapToDto(supplier) : null;
        }

        public async Task<SupplierDto?> GetSupplierByEmailAsync(string email)
        {
            var supplier = await _unitOfWork.Suppliers.GetByEmailAsync(email);
            return supplier != null ? MapToDto(supplier) : null;
        }

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            // First create the user
            var nameParts = (dto.Name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var user = new User
            {
                Email = dto.Email,
                FirstName = nameParts.FirstOrDefault(),
                LastName = nameParts.Skip(1).FirstOrDefault(),
                CompanyName = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Supplier,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var supplier = new Supplier
            {
                UserId = user.Id,
                CompanyName = dto.Name ?? string.Empty,
                ContactPerson = dto.Name ?? string.Empty,
                BusinessAddress = dto.Address ?? "",
                City = "Unknown",
                Province = "Unknown", 
                PostalCode = "0000",
                CompanyRegistrationNumber = dto.CompanyRegistrationNumber,
                BusinessRegistrationNumber = dto.CompanyRegistrationNumber ?? "",
                Description = dto.Description,
                Website = dto.Website,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                User = user
            };

            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(supplier);
        }

        public async Task<SupplierDto?> UpdateSupplierAsync(Guid id, UpdateSupplierDto dto)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) supplier.CompanyName = dto.Name;
            if (dto.CompanyRegistrationNumber != null) supplier.CompanyRegistrationNumber = dto.CompanyRegistrationNumber;
            if (dto.Address != null) supplier.BusinessAddress = dto.Address;
            if (dto.Website != null) supplier.Website = dto.Website;
            if (dto.Description != null) supplier.Description = dto.Description;
            
            supplier.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(supplier);
        }

        public async Task<bool> DeleteSupplierAsync(Guid id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) return false;

            _unitOfWork.Suppliers.Remove(supplier);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync()
        {
            var suppliers = await _unitOfWork.Suppliers.GetActiveSuppliers();
            return suppliers.Select(MapToDto);
        }

        private static SupplierDto MapToDto(Supplier supplier)
        {
            return new SupplierDto
            {
                Id = supplier.Id,
                Name = (supplier.User?.FirstName + " " + supplier.User?.LastName).Trim() != ""
                    ? ($"{supplier.User?.FirstName} {supplier.User?.LastName}").Trim()
                    : (supplier.User?.CompanyName ?? supplier.CompanyName),
                Email = supplier.User?.Email ?? "",
                PhoneNumber = supplier.User?.PhoneNumber,
                CompanyRegistrationNumber = supplier.CompanyRegistrationNumber,
                Address = supplier.BusinessAddress,
                Website = supplier.Website,
                Description = supplier.Description,
                IsActive = (supplier.User?.Status ?? UserStatus.Active) == UserStatus.Active,
                CreatedAt = supplier.CreatedAt
            };
        }
    }
}
