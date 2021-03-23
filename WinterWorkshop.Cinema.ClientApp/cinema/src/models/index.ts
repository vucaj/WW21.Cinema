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
  streetName: string;
  cityName: string;
  country: string;
  latitude: number;
  longitude: number; 
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

export interface IAddress{
  id: number;
  cityName: string;
  streetName: string;
  country: string;
  longitude: number;
  latitude: number;
}