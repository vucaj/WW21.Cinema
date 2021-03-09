using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDomainModel>> GetAllAddressesAsync();
        Task<CreateAddressResultModel>  GetAddressByIdAsync(AddressDomainModel newAddress);
        Task<CreateAddressResultModel> AddAddress(AddressDomainModel newAddress);
        Task<UpdateAddressResultModel> UpdateAddress(AddressDomainModel updateAddress);
        Task<DeleteAddressResultModel> DeleteAddress(AddressDomainModel deleteAddress);
    }
}
