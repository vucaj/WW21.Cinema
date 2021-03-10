using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatsRepository _seatRepository;
        public SeatService(ISeatsRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }
        public async Task<IEnumerable<SeatDomainModel>> GetAllAsync()
        {
            var seat = await _seatRepository.GetAllAsync();
            return seat.Select(seats => new SeatDomainModel
            {
                Id = seats.Id,
                AuditoriumId = seats.AuditoriumId,
                Number = seats.Number,
                Row = seats.Row,
                SeatType = seats.SeatType
            });
        }


        public async Task<IEnumerable<GetSeatResultModel>> GetAllByAuditoriumIdAsync(AuditoriumDomainModel domainModel)
        {
            var seats = await _seatRepository.GetAllByAuditoriumIdAsync(domainModel.Id);

            // ne radi jer je IEnumerable, kako onda proveru izvsiti da li je null, ovo je privremeno resenje
            if (seats == null)
            {
                return null;
            }

            return seats.Select(seat => new GetSeatResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Seat = new SeatDomainModel
                {
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Number = seat.Number,
                    Row = seat.Row,
                    SeatType = seat.SeatType
                }
            });
        }

        public async Task<GetSeatResultModel> GetByIdAsync(SeatDomainModel domainModel)
        {
            var seat = await _seatRepository.GetByIdAsync(domainModel.Id);
            if (seat == null)
            {
                return new GetSeatResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.SEAT_NOT_FOUND
                };
            }
            return new GetSeatResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Seat = new SeatDomainModel
                { 
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Row = seat.Row,
                    Number = seat.Number,
                    SeatType = seat.SeatType
                }
            };
        }

        public async Task<IEnumerable<SeatDomainModel>> GetByAllSeatTypeAsync(SeatDomainModel domainModel)
        {
            var seat = await _seatRepository.GetByAllSeatTypeAsync(domainModel.SeatType);

            //treba i provera da li je dobar seattype (znaci iz getseatResultModel sve promeniti)
            //samo skontati kako to da se postigne
            // ne radi jer je IEnumerable, kako onda proveru izvsiti da li je null, ovo je privremeno resenje
            if (seat == null)
            {
                return null;
            }


            return seat.Select(seats => new SeatDomainModel
            {
                Id = seats.Id,
                AuditoriumId = seats.AuditoriumId,
                Number = seats.Number,
                Row = seats.Row,
                SeatType = seats.SeatType
            });
        }
    }
}
