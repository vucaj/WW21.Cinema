export interface IProjection {
  id: string;
  dateTime: string;
  movieId: string;
  auditoirumId: string;
  cinemaId: string;
  ticketPrice: number;
  movieTitle: string;
  auditoriumName: string;
  cinemaName: string;
  movieRating: number
}

export interface IAddress{
  id: 0;
  cityName: "";
  streetName: "";
  country: "";
  latitude: 0;
  longitude: 0;
}

export interface IMovie {
  id: "",
  bannerUrl: "",
  title: "",
  year: 0,
  isActive: false,
  duration: 0,
  distributer: "",
  description: "",
  genre: 0,
  rating: 0
  projections?: IProjection[];
}


export interface ICinema {
  id: string;
  name: string;
  addressId: number;
}

export interface IAuditorium {
  id: string;
  name: string;
  cinemaId?: string;
}

export interface ISeats {
  id: string;
  number: number;
  row: number;
}

export interface ICurrentReservationSeats {
  currentSeatId: string;
}

export interface IReservedSeats {
  seatId: string;
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  bonusPoints: string;
}

export interface IReservation {
  projectionId: string;
}

export interface ITag {
  name: string;
}
