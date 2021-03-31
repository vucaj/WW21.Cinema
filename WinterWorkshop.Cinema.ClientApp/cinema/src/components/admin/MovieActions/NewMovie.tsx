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
  Form,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { YearPicker } from "react-dropdown-date";
import { Input, InputNumber } from 'antd';
import { IMovie } from "../../../models";

interface IState {
  movies: IMovie[];
  title: string;
  year: string;
  rating: string;
  current: boolean;
  isActive: boolean;
  description: string;
  genre: number;
  duration: number;
  distributer: string;
  numberOfOscars: number;
  titleError: string;
  submitted: boolean;
  canSubmit: boolean;
  tags: string;
  bannerUrl: string;
  yearError: string;
}

const NewMovie: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
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
    title: "",
    year: "",
    rating: "",
    current: false,
    isActive: false,
    description: "",
    genre: 0,
    duration: 0,
    distributer: "",
    numberOfOscars: 0,
    titleError: "",
    submitted: false,
    canSubmit: true,
    tags: "",
    bannerUrl: "",
    yearError: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
    // validate(id, value);
  };

  // const handleTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   setState({ ...state, tags: e.target.value });
  // };

  // const handleBannerUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   setState({ ...state, bannerUrl: e.target.value });
  // };

  // const validate = (id: string, value: string) => {
  //   if (id === "title") {
  //     if (value === "") {
  //       setState({
  //         ...state,
  //         titleError: "Fill in movie title",
  //         canSubmit: false,
  //       });
  //     } else {
  //       setState({ ...state, titleError: "", canSubmit: true });
  //     }
  //   }

  // if (id === "year") {
  //   const yearNum = +value;
  //   if (!value || value === "" || yearNum < 1895 || yearNum > 2100) {
  //     setState({ ...state, yearError: "Please chose valid year" });
  //   } else {
  //     setState({ ...state, yearError: "" });
  //   }
  // }
  // };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    const { title, year, rating } = state;
    if (title && year && rating) {
      addMovie();
    } else {
      NotificationManager.error("Please fill in data");
      setState({ ...state, submitted: false });
    }
  };

  const handleYearChange = (year: string) => {
    setState({ ...state, year: year });
    // validate("year", year);
  };

  const handleDurationChange = (duration: number) => {
    setState({ ...state, duration: duration });
  }

  // const handleDistributerChange = (distributer: string) => {
  //   setState({ ...state, distributer: distributer });
  // }

  const handleNumberOfOscarsChange = (numberOfOscars: number) => {
    setState({ ...state, numberOfOscars: numberOfOscars });
  }

  const handleGenreChange = (genre: number) => {
    setState({ ...state, genre: genre });
  }

  const addMovie = () => {
    const data = {
      Title: state.title,
      Year: +state.year,
      isActive: state.isActive == false,
      Rating: +state.rating,
      Description: state.description,
      Genre: +state.genre,
      Duration: state.duration,
      Distributer: state.distributer,
      NumberOfOscars: state.numberOfOscars,
    };

    const requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
      body: JSON.stringify(data),
    };

    fetch(`${serviceConfig.baseURL}/api/movies/create`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject("Something went wrong");
        }
        return response.statusText;
      })
      .then((result) => {
        NotificationManager.success("Successfuly added movie!");
        props.history.push(`AllMovies`);
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setState({ ...state, submitted: false });
      });
  };

  const getGenre = (genre) => {
    switch (genre) {
      case 0:
        return "Horror";
      case 1:
        return "Action";
      case 2:
        return "Drama";
      case 3:
        return "Sci-fi";
      case 4:
        return "Crime";
      case 5:
        return "Fantasy";
      case 6:
        return "Historical";
      case 7:
        return "Romance";
      case 8:
        return "Western";
      case 9:
        return "Thriler";
      case 10:
        return "Animated";
      case 11:
        return "Adult";
      case 12:
        return "Documentary";
    }
  }

  const { TextArea } = Input;

  return (
    <Container>
      <Row>
        <Col>
          <h1 className="form-header">Add New Movie</h1>
          <form onSubmit={handleSubmit}>
            <FormGroup>
              <FormControl
                id="title"
                type="text"
                maxLength={50}
                placeholder="Movie Title"
                value={state.title}
                onChange={handleChange}
                className="add-new-form"
                required
              />
              <FormText className="text-danger">{state.titleError}</FormText>
            </FormGroup>
            <FormGroup>
              <YearPicker
                defaultValue={"Select Movie Year"}
                start={1950}
                end={2021}
                reverse
                required={true}
                disabled={false}
                value={state.year}
                onChange={(year: string) => {
                  handleYearChange(year);
                }}
                id={"year"}
                name={"year"}
                classes={"form-control add-new-form"}
                optionClasses={"option classes"}
              />
              <FormText className="text-danger">{state.yearError}</FormText>
            </FormGroup>
            <FormGroup>
              <FormControl
                defaultValue={"Select Rating"}
                as="select"
                className="add-new-form"
                required={true}
                placeholder="Select Rating"
                id="rating"
                value={state.rating}
                onChange={handleChange}
              >
                <option value="0">Select Rating</option>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
                <option value="6">6</option>
                <option value="7">7</option>
                <option value="8">8</option>
                <option value="9">9</option>
                <option value="10">10</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                as="select"
                className="add-new-form"
                id="genre"
                value={state.genre}
                onChange={handleChange}
              >
                <option value="0">Horror</option>
                <option value="1">Action</option>
                <option value="2">Drama</option>
                <option value="3">Sci-fi</option>
                <option value="4">Crime</option>
                <option value="5">Fantasy</option>
                <option value="6">Historical</option>
                <option value="7">Romance</option>
                <option value="8">Western</option>
                <option value="9">Thriler</option>
                <option value="10">Animated</option>
                <option value="11">Adult</option>
                <option value="12">Documentary</option>
              </FormControl>
            </FormGroup>
            <FormGroup>
              <FormControl
                className="add-new-form"
                as="select"
                placeholder="IsActive"
                id="isActive"
                value={state.isActive.toString()}
                onChange={handleChange}
                style={{ marginTop: 10, marginBottom: 30 }}
              >
                <option value="false">Active</option>
                <option value="true">Not Active</option>
              </FormControl>
            </FormGroup>
            <label className="label-nos">Number of Oscars: </label>
            <InputNumber className="add-new-form" style={{ position: "relative", left: 355, marginTop: 10, marginBottom: 10 }} id="numberOfOscars" placeholder="Number of Oscars" min={0} max={20} value={state.numberOfOscars} pattern="[0-9]*" onChange={(numberOfOscars: number) => {
              handleNumberOfOscarsChange(numberOfOscars);
            }} />
            <FormControl
              id="distributer"
              type="text"
              placeholder="Distributer"
              maxLength={30}
              value={state.distributer}
              onChange={handleChange}
              className="add-new-form"
              style={{ marginTop: 10, marginBottom: 30 }}
            />
            <label className="label-duration">Duration(min): </label>
            <InputNumber id="duration" className="add-new-form" style={{ position: "relative", left: 355, marginTop: 10, marginBottom: 5 }} placeholder="Duration(min)" min={1} max={500} value={state.duration} pattern="[0-9]*" onChange={(duration: number) => {
              handleDurationChange(duration);
            }} />
            <FormControl
              as="textarea"
              rows={3}
              id="description"
              type="text"
              placeholder="Description"
              maxLength={300}
              value={state.description}
              onChange={handleChange}
              className="add-new-form"
            />
            <FormText className="text-danger">{state.titleError}</FormText>
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

export default withRouter(NewMovie);
