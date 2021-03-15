using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        /// Get all Addresses
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<AddressDomainModel>>> GetAllAsync()
        {
            IEnumerable<AddressDomainModel> addressDomainModels;

            addressDomainModels = await _addressService.GetAllAddressesAsync();

            if (addressDomainModels == null)
            {
                addressDomainModels = new List<AddressDomainModel>();
            }

            return Ok(addressDomainModels);

        }

        [HttpPost]
        [Route("getById")]
        public async Task<ActionResult<AddressDomainModel>> GetAddressByIdAsync([FromBody]AddressDomainModel domainModel)
        {
            var addressById = await _addressService.GetAddressByIdAsync(new AddressDomainModel
            {
                Id = domainModel.Id
            });
            if (addressById == null)
            {
                return NotFound();
            }
            return Ok(addressById);
        }




        /// Adds a new address
        //[Authorize(Roles = "admin")]

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AddressDomainModel>> CreateAddressAsync([FromBody] CreateAddressModel createAddressModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AddressDomainModel addressDomainModel = new AddressDomainModel
            {
                CityName = createAddressModel.CityName,
                StreetName = createAddressModel.StreetName,
                Country = createAddressModel.Country,
                Longitude = createAddressModel.Longitude,
                Latitude = createAddressModel.Latitude
            };

            CreateAddressResultModel createAddressResultModel;

            try
            {
                createAddressResultModel = await _addressService.AddAddress(addressDomainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!createAddressResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createAddressResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("addresses//" + createAddressResultModel.Address.Id, createAddressResultModel.Address);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<AddressDomainModel>> DeleteAddress([FromBody] DeleteAddressModel deleteAddressModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateAddressResultModel address = await _addressService.GetAddressByIdAsync(new AddressDomainModel
            {
                Id = deleteAddressModel.AddressId
            });

            if (!address.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = address.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponseModel);
            }

            AddressDomainModel addressDomainModel = new AddressDomainModel()
            {
                Id = address.Address.Id,
                StreetName = address.Address.StreetName,
                CityName = address.Address.CityName,
                Country = address.Address.Country,
                Latitude = address.Address.Latitude,
                Longitude = address.Address.Longitude
            };

            DeleteAddressResultModel resultModel = await _addressService.DeleteAddress(addressDomainModel);

            if (!resultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = resultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponseModel);
            }

            return Accepted("Address//" + resultModel.Address.Id, resultModel.Address);
        }
        
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateAddress([FromBody] UpdateAddressModel updateAddressModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = await _addressService.GetAddressByIdAsync(new AddressDomainModel
            {
                Id = updateAddressModel.Id
            });

            address.Address.Country = updateAddressModel.Country;
            address.Address.CityName = updateAddressModel.CityName;
            address.Address.StreetName = updateAddressModel.StreetName;
            address.Address.Longitude = updateAddressModel.Longitude;
            address.Address.Latitude = updateAddressModel.Longitude;


            UpdateAddressResultModel updateAddressResultModel = await _addressService.UpdateAddress(new AddressDomainModel
            {
                Id = address.Address.Id,
                Country = address.Address.Country,
                CityName = address.Address.CityName,
                StreetName = address.Address.StreetName,
                Longitude = address.Address.Longitude,
                Latitude = address.Address.Latitude
            });

            if (!updateAddressResultModel.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}

