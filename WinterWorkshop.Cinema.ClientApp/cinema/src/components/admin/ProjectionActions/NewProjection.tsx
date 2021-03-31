import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Typeahead } from "react-bootstrap-typeahead";
import { IAuditorium, IMovie, ICinema } from "../../../models";
import { DatePicker, Space } from "antd";
import "antd/dist/antd.css";

interface IState {
  dateTime: string;
  movieId: string;
  auditoriumId: string;
  cinemaId: string;
  ticketPrice: number;
  date: Date;
  ticketPriceError: string;
  cinemaIdError: string;
  dateTimeError: string;
  movieIdError: string;
  auditoriumIdError: string;

  movies: IMovie[];
  auditoriums: IAuditorium[];
  cinemas: ICinema[];

  submitted: boolean;
  canSubmit: boolean;
}

const NewProjection: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    date: new Date(),
    ticketPrice: 0,
    ticketPriceError: "",
    dateTime: "",
    movieId: "",
    auditoriumId: "",
    cinemaId: "",
    cinemaIdError: "",
    submitted: false,
    dateTimeError: "",
    movieIdError: "",
    auditoriumIdError: "",
    movies: [
      {
        id: "",
        bannerUrl: "",
        title: "",
        year: 0,
        isActive: false,
        duration: 0,
        distributer: "",
        description: "",
        genre: 0,
        rating: 0,
        numberOfOscars: 0,
      },
    ],
    auditoriums: [
      {
        id: "",
        name: "",
        cinemaId: "",
      },
    ],
    cinemas: [
      {
        id: "",
        name: "",
        addressId: 0,
        cityName: "",
        country: "",
        latitude: 0,
        longitude: 0,
        streetName: "",
      },
    ],
    canSubmit: true,
  });

  useEffect(() => {
    getProjections();
    getAuditoriums();
    getCinemas();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };



  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });

    if (state.movieId && state.auditoriumId && state.dateTime) {
      addProjection();
    } else {
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

  const addProjection = () => {
    const data = {
      movieId: state.movieId,
      cinemaId: state.cinemaId,
      auditoriumId: state.auditoriumId,
      dateTime: state.dateTime,
      ticketPrice: +state.ticketPrice,
    };

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/Projections/create`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("New projection added!");
        props.history.push(`Projection`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getProjections = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/Movies/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState(prevState => { return { ...prevState, movies: data } });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getAuditoriums = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/Auditoriums/getAll`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState(prevState => { return { ...prevState, auditoriums: data } });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getCinemas = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(`${serviceConfig.baseURL}/api/Cinemas/getAll`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState(prevState => { return { ...prevState, cinemas: data } });
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const onMovieChange = (movies: IMovie[]) => {
    if (movies[0]) {
      setState({ ...state, movieId: movies[0].id });
    }
  };

  const onAuditoriumChange = (auditoriums: IAuditorium[]) => {
    if (auditoriums[0]) {
      setState({ ...state, auditoriumId: auditoriums[0].id });
    }
  };

  const onCinemaChange = (cinemas: ICinema[]) => {
    if (cinemas[0]) {
      setState({ ...state, cinemaId: cinemas[0].id });
    }
  }

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add Projection</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <Typeahead
                labelKey="title"
                options={state.movies}
                placeholder="Choose a movie..."
                id="movie"
                className="add-new-form"
                onChange={(e) => {
                  onMovieChange(e);
                }}
              />
              <FormText className="text-danger">{state.movieIdError}</FormText>
            </FormGroup>
            <FormGroup>
              <Typeahead
                labelKey="name"
                className="add-new-form"
                options={state.auditoriums}
                placeholder="Choose auditorium..."
                id="auditorium"
                onChange={(e) => {
                  onAuditoriumChange(e);
                }}
              />
              <FormText className="text-danger">
                {state.auditoriumIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <Typeahead
                labelKey="name"
                className="add-new-form"
                options={state.cinemas}
                placeholder="Choose cinema..."
                id="cinema"
                onChange={(e) => {
                  onCinemaChange(e);
                }}
              />
              <FormText className="text-danger">
                {state.cinemaIdError}
              </FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                id="dateTime"
                type="text"
                placeholder="Date Time - HH:MM:SS MM.DD.YYYY."
                value={state.dateTime}
                className="add-new-form"
                onChange={handleChange}
              />
              <FormText className="text-danger">
                {state.dateTimeError}
              </FormText>
            </FormGroup>
            <label className="label-nos">Ticket Price: </label>
            <FormGroup>
              <FormControl
                id="ticketPrice"
                type="number"
                placeholder="Ticket Price"
                value={state.ticketPrice.toString()}
                className="add-new-form"
                onChange={handleChange}
                min={1}
              />
              <FormText className="text-danger">
                {state.ticketPriceError}
              </FormText>
            </FormGroup>
            <Button
              className="btn-add-new"
              type="submit"
              disabled={state.submitted || !state.canSubmit}
              block
            >
              Add
            </Button>
          </form>
        </Col>
      </Row>
    </Container>
  );
};

export default withRouter(NewProjection);
