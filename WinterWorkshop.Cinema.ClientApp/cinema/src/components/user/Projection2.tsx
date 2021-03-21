import {IAuditorium, ICinema, IMovie, IProjection} from "../../models";
import { NotificationManager } from "react-notifications";
import React, { useEffect, useState } from "react";
import {withRouter} from "react-router-dom";
import {serviceConfig} from "../../appSettings";
import {Table} from "antd";


interface IState {
    movies: IMovie[];
    cinemas: ICinema[];
    auditoriums: IAuditorium[];
    projections: IProjection[];
    filteredAuditoriums: IAuditorium[];
    filteredMovies: IMovie[];
    filteredProjections: IProjection[];
    dateTime: string;
    id: string;
    current: boolean;
    tag: string;
    titleError: string;
    yearError: string;
    submitted: boolean;
    isLoading: boolean;
    selectedCinema: boolean;
    selectedAuditorium: boolean;
    selectedMovie: boolean;
    selectedDate: boolean;
    date: Date;
    cinemaId: string;
    auditoriumId: string;
    movieId: string;
    name: string;
}

const Projection2: React.FC = (props: any) => {
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
            },
        ],
        projections: [
            {
                id: "",
                movieId: "",
                dateTime: "",
                auditoirumId: "",
                cinemaId: '',
                ticketPrice: 0,
                movieTitle: '',
                auditoriumName: '',
                cinemaName: '',
                movieRating: 0
            },
        ],
        cinemas: [{ id: "", name: "", addressId: 0 }],
        auditoriums: [
            {
                id: "",
                name: "",
            },
        ],
        filteredAuditoriums: [
            {
                id: "",
                name: "",
            },
        ],
        filteredMovies: [
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
                rating: 0
            },
        ],
        filteredProjections: [
            {
                id: "",
                movieId: "",
                dateTime: "",
                auditoirumId: "",
                cinemaId: '',
                ticketPrice: 0,
                movieTitle: '',
                auditoriumName: '',
                cinemaName: '',
                movieRating: 0
            },
        ],
        cinemaId: "",
        auditoriumId: "",
        movieId: "",
        dateTime: "",
        id: "",
        name: "",
        current: false,
        tag: "",
        titleError: "",
        yearError: "",
        submitted: false,
        isLoading: true,
        selectedCinema: false,
        selectedAuditorium: false,
        selectedMovie: false,
        selectedDate: false,
        date: new Date(),
    })

    useEffect(() => {
        getAllCinemas();
        getAllAuditoria();
        getMoviesWithFutureProjections();
        getAllProjections();
    }, [])

    const getAllProjections = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            }
        };

        setState({...state, isLoading: true});
        fetch(`${serviceConfig.baseURL}/api/projections/getAllFuture`, requestOptions)
            .then((response) => {
                if(!response.ok){
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if(data){
                    setState({...state, projections: data, isLoading: false});
                }
            })
            .catch((response) => {
                setState({...state, isLoading: false})
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getMoviesWithFutureProjections = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            }
        };

        setState({...state, isLoading: true});
        fetch(`${serviceConfig.baseURL}/api/movies/withFutureProjections`, requestOptions)
            .then((response) => {
                if(!response.ok){
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if(data){
                    setState({...state, movies: data, isLoading: false});
                }
            })
            .catch((response) => {
                setState({...state, isLoading: false})
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getAllCinemas = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            },
        };

        setState({...state, isLoading: true});
        fetch(`${serviceConfig.baseURL}/api/Cinemas/getAll`, requestOptions)
            .then((response) => {
                if(!response.ok){
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if(data){
                    setState({...state, cinemas: data, isLoading: false});
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({...state, isLoading: false});
            });
    };

    const getAllAuditoria = () =>{
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({...state, isLoading: true});
        fetch(`${serviceConfig.baseURL}/api/Auditoriums/getAll`, requestOptions)
            .then((response) => {
                if(!response.ok){
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if(data) {
                    setState({...state, auditoriums: data, isLoading: false});
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({...state, isLoading: false});
            });
    }

    const formatDate = (date) => {
        return(
            <div></div>
        );
    }

    const columns = [
        {
            title: "Cinema Name",
            dataIndex: 'cinemaName',
            key: 'cinemaName'
        },
        {
            title: 'Movie Title',
            dataIndex: 'movieTitle',
            key: 'movieTitle'
        },
        {
            title: 'Time',
            dataIndex: 'dateTime',
            key: 'dateTime',
            //render: (date) => {return(formatDate(date))}
        },
        {
            title: 'Ticket Price',
            dataIndex: 'ticketPrice',
            key: 'ticketPrice'
        },
        {
            title: 'Auditorium Name',
            dataIndex: 'auditoriumName',
            key: 'auditoriumName'
        },
        {
            title: 'Movie Rating',
            dataIndex: 'movieRating',
            key: 'movieRating'
        }


    ]

    const getTable = () => {
        return(
            <div>
                <Table
                    columns = {columns}
                    dataSource={state.projections}
                    rowKey={projection => projection.id}>
                </Table>
            </div>
        )
    }


    return (
      <React.Fragment>
          <div>
              {getTable()}
          </div>
      </React.Fragment>
    );
}

export default withRouter(Projection2)