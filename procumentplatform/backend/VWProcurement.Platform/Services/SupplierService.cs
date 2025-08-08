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

        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
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
            var supplier = new Supplier
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CompanyRegistrationNumber = dto.CompanyRegistrationNumber,
                Address = dto.Address,
                Website = dto.Website,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(supplier);
        }

        public async Task<SupplierDto?> UpdateSupplierAsync(int id, UpdateSupplierDto dto)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) supplier.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email)) supplier.Email = dto.Email;
            if (dto.PhoneNumber != null) supplier.PhoneNumber = dto.PhoneNumber;
            if (dto.CompanyRegistrationNumber != null) supplier.CompanyRegistrationNumber = dto.CompanyRegistrationNumber;
            if (dto.Address != null) supplier.Address = dto.Address;
            if (dto.Website != null) supplier.Website = dto.Website;
            if (dto.Description != null) supplier.Description = dto.Description;
            if (dto.IsActive.HasValue) supplier.IsActive = dto.IsActive.Value;
            
            supplier.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(supplier);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
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
                Name = supplier.Name,
                Email = supplier.Email,
                PhoneNumber = supplier.PhoneNumber,
                CompanyRegistrationNumber = supplier.CompanyRegistrationNumber,
                Address = supplier.Address,
                Website = supplier.Website,
                Description = supplier.Description,
                IsActive = supplier.IsActive,
                CreatedAt = supplier.CreatedAt
            };
        }
    }
}
