using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task<CreateAddressResultModel> AddAddress(AddressDomainModel newAddress)
        {
            Address addressToAdd = new Address
            {
                Id = newAddress.Id,
                StreetName = newAddress.StreetName,
                CityName = newAddress.CityName,
                Country = newAddress.Country,
                Latitude = newAddress.Latitude,
                Longitude = newAddress.Longitude
            };
            var insertedAddress = _addressRepository.Insert(addressToAdd);
            if(insertedAddress == null)
            {
                return new CreateAddressResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.ADDRESS_CREATION_ERROR
                };
            }
            _addressRepository.Save();
            CreateAddressResultModel resultModel = new CreateAddressResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = new AddressDomainModel
                {
                    Id = insertedAddress.Id,
                    StreetName = insertedAddress.StreetName,
                    CityName = insertedAddress.CityName,
                    Country = insertedAddress.Country,
                    Latitude = insertedAddress.Latitude,
                    Longitude = insertedAddress.Longitude
                }
            };
            return resultModel;
        }

        public async Task<DeleteAddressResultModel> DeleteAddress(AddressDomainModel domainModel)
        {
            var addressToDelete = await _addressRepository.GetByIdAsync(domainModel.Id);
            if (addressToDelete == null)
            {
                return new DeleteAddressResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.ADDRESS_NOT_FOUND
                };
            }

            _addressRepository.Delete(addressToDelete.Id);

            _addressRepository.Save();

            DeleteAddressResultModel resultModel = new DeleteAddressResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };
            return resultModel;
        }

        public async Task<CreateAddressResultModel> GetAddressByIdAsync(AddressDomainModel newAddress)
        {
            var addressToGetById = await _addressRepository.GetByIdAsync(newAddress.Id);
            if (addressToGetById == null)
            {
                return new CreateAddressResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.ADDRESS_GET_BY_ID
                };
            }
            return new CreateAddressResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = new AddressDomainModel 
                { 
                    Id = addressToGetById.Id,
                    CityName = addressToGetById.CityName,
                    Country = addressToGetById.Country,
                    StreetName = addressToGetById.StreetName,
                    Latitude = addressToGetById.Latitude,
                    Longitude = addressToGetById.Longitude
                }
            };
            
        }

        public async Task<IEnumerable<AddressDomainModel>> GetAllAddressesAsync()
        {
            var address = await _addressRepository.GetAllAsync();
            return address.Select(address => new AddressDomainModel
            {
                Id = address.Id,
                CityName = address.CityName,
                Country = address.Country,
                StreetName = address.StreetName,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            });
        }

        public async Task<UpdateAddressResultModel> UpdateAddress(AddressDomainModel domainModel)
        {
            var address = await _addressRepository.GetByIdAsync(domainModel.Id);

            if(address == null)
            {
                return new UpdateAddressResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.ADDRESS_NOT_FOUND
                };
            }

            address.Country = domainModel.Country;
            address.CityName = domainModel.CityName;
            address.StreetName = domainModel.StreetName;
            address.Latitude = domainModel.Latitude;
            address.Longitude = domainModel.Longitude;

            var updateAddress = _addressRepository.Update(address);
            if(updateAddress == null)
            {
                return new UpdateAddressResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.ADDRESS_UPDATE_ERROR
                };
            }

            _addressRepository.Save();

            UpdateAddressResultModel resultModel = new UpdateAddressResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

            return resultModel;
        }
    }
}
