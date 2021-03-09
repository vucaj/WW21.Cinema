namespace WinterWorkShop.Cinema.Domain.Common
{
    public static class Messages
    {
        #region Users

        #endregion

        #region Payments
        public const string PAYMENT_CREATION_ERROR = "Connection error, occured while creating new payment, please try again";
        #endregion

        #region Auditoriums
        public const string AUDITORIUM_GET_ALL_AUDITORIUMS_ERROR = "Error occured while getting all auditoriums, please try again.";
        public const string AUDITORIUM_PROPERTIE_NAME_NOT_VALID = "The auditorium Name cannot be longer than 50 characters.";
        public const string AUDITORIUM_PROPERTIE_SEATROWSNUMBER_NOT_VALID = "The auditorium number of seats rows must be between 1-20.";
        public const string AUDITORIUM_PROPERTIE_SEATNUMBER_NOT_VALID = "The auditorium number of seats number must be between 1-20.";
        public const string AUDITORIUM_CREATION_ERROR = "Error occured while creating new auditorium, please try again.";
        public const string AUDITORIUM_SEATS_CREATION_ERROR = "Error occured while creating seats for auditorium, please try again.";
        public const string AUDITORIUM_SAME_NAME = "Cannot create new auditorium, auditorium with same name alredy exist.";
        public const string AUDITORIUM_INVALID_CINEMAID = "Cannot create new auditorium, auditorium with given cinemaId does not exist.";
        public const string AUDITORIUM_DELTE_INVALID_CINEMAID =
            "Cannot delete auditorium, auditorium with given cinemaId does not exist.";
        public const string AUDITORIUM_NOT_FOUND = "Cannot find auditorium with given Auditorium ID.";
        public const string AUDITORIUM_UPDATE_ERROR = "Error occured while updating auditorium.";
        
        
        #endregion

        #region Cinemas
        public const string CINEMA_GET_ALL_CINEMAS_ERROR = "Error occured while getting all cinemas, please try again";
        #endregion

        #region Movies        
        public const string MOVIE_DOES_NOT_EXIST = "Movie does not exist.";
        public const string MOVIE_PROPERTIE_TITLE_NOT_VALID = "The movie title cannot be longer than 50 characters.";
        public const string MOVIE_PROPERTIE_YEAR_NOT_VALID = "The movie year must be between 1895-2100.";
        public const string MOVIE_PROPERTIE_RATING_NOT_VALID = "The movie rating must be between 1-10.";
        public const string MOVIE_CREATION_ERROR = "Error occured while creating new movie, please try again.";
        public const string MOVIE_GET_ALL_CURRENT_MOVIES_ERROR = "Error occured while getting current movies, please try again.";
        public const string MOVIE_GET_BY_ID = "Error occured while getting movie by Id, please try again.";
        public const string MOVIE_GET_ALL_MOVIES_ERROR = "Error occured while getting all movies, please try again.";
        #endregion

        #region Projections
        public const string PROJECTION_GET_ALL_PROJECTIONS_ERROR = "Error occured while getting all projections, please try again.";
        public const string PROJECTION_CREATION_ERROR = "Error occured while creating new projection, please try again.";
        public const string PROJECTIONS_AT_SAME_TIME = "Cannot create new projection, there are projections at same time alredy.";
        public const string PROJECTION_IN_PAST = "Projection time cannot be in past.";
        #endregion

        #region Seats
        public const string SEAT_GET_ALL_SEATS_ERROR = "Error occured while getting all seats, please try again.";
        #endregion

        #region User
        public const string USER_NOT_FOUND = "User does not exist.";
        #endregion
        
        #region Address
        public const string ADDRESS_CREATION_ERROR = "Error occured while creating new address, please try again.";
        public const string ADDRESS_GET_BY_ID = "Error occured while trying to get address by ID, please try again.";
        public const string ADDRESS_NOT_FOUND = "Cannot find address with given Address ID, please try again";
        public const string ADDRESS_UPDATE_ERROR = "Error occured while updating address.";
        public const string ADDRESS_CITY_NAME_NOT_VALID = "The Address City name cannot be longer than 20 characters.";
        public const string ADDRESS_COUNTRY_NOT_VALID = "The Address County name cannot be longer than 20 characters.";
        public const string ADDRESS_STREET_NAME_NOT_VALID = "The Address Street name cannot be longer than 50 characters.";
        public const string ADDRESS_LONGITUDE_NOT_VALID = "The Address longitude must be in range between -180.0 and 180.0";
        public const string ADDRESS_LATITUDE_NOT_VALID = "The Address longitude must be in range between -90.0 and 90.0";
        #endregion

        #region Participant
        public const string PARTICIPANT_CREATION_ERROR = "Error occured while creating new participant, please try again.";
        public const string PARTICIPANT_NOT_FOUND = "Cannot find participant with given Participant ID, please try again.";
        public const string PARTICIPANT_UPDATE_ERROR = "Error occured while updating participant.";
        public const string PARTICIPANT_FIRST_NAME_NOT_VALID = "The participant first name cannot be longer than 30 characters.";
        public const string PARTICIPANT_LAST_NAME_NOT_VALID = "The participant last name cannot be longer than 30 characters.";
        #endregion
    }
}
