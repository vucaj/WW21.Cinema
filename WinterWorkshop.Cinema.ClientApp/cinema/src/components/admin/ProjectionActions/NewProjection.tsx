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
  cinemaId:string;
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
    cinemaId:"",
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

  const handleChange = (e) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };

  // const validate = (id, value) => {
  //   if (id === "projectionTime") {
  //     if (!value) {
  //       setState({
  //         ...state,
  //         projectionTimeError: "Chose projection time",
  //         canSubmit: false,
  //       });
  //     } else {
  //       setState({ ...state, projectionTimeError: "", canSubmit: true });
  //     }
  //   } else if (id === "movieId") {
  //     if (!value) {
  //       setState({
  //         ...state,
  //         movieIdError: "Please chose movie from dropdown",
  //         canSubmit: false,
  //       });
  //     } else {
  //       setState({ ...state, movieIdError: "", canSubmit: true });
  //     }
  //   } else if (id === "auditoriumId") {
  //     if (!value) {
  //       setState({
  //         ...state,
  //         auditoriumIdError: "Please chose auditorium from dropdown",
  //         canSubmit: false,
  //       });
  //     } else {
  //       setState({ ...state, auditoriumIdError: "", canSubmit: true });
  //     }
  //   }
  // };

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
      ticketPrice: state.ticketPrice,
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

    fetch(`${serviceConfig.baseURL}/api/Movies/current`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setState({ ...state, movies: data });
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
          setState({ ...state, auditoriums: data });
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
          setState({ ...state, cinemas: data });
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
     // validate("movieId", movies[0]);
    } //else {
    //  validate("movieId", null);
   // }
  };

  const onAuditoriumChange = (auditoriums: IAuditorium[]) => {
    if (auditoriums[0]) {
      setState({ ...state, auditoriumId: auditoriums[0].id });
     // validate("auditoriumId", auditoriums[0]);
    } //else {
      //validate("auditoriumId", null);
   // }
  };

  const onCinemaChange = (cinemas: ICinema[]) => {
    setState({...state, cinemaId: cinemas[0].id});
  }

  const onDateChange = (date: Date) =>
    setState({ ...state, dateTime: date.toLocaleTimeString() });

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
              <Space direction="vertical">
	              <DatePicker
                 onChange={(e) => {
                  onDateChange(state.date);
                }} 
                />
              </Space>
              <FormText className="text-danger">
                {state.dateTimeError}
              </FormText>
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
