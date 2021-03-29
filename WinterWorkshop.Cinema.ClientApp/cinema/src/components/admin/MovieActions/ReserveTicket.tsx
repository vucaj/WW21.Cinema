import React, { useEffect, useState } from "react";
import "antd/dist/antd.css";
import {withRouter} from "react-router-dom";
import {IProjection, ISeats, ITicket} from "../../../models";
import {serviceConfig} from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import { Button } from 'antd';

interface IState{
    seats: ISeats[];
    projection: IProjection;
    tickets: ITicket[];
}

const ReserveTicket: React.FC = (props: any) => {
    const { id } = props.match.params;

    const [state, setState] = useState<IState>({
        seats: [],
        projection: {
            id: '',
            dateTime: '',
            movieId: '',
            auditoirumId: '',
            cinemaId: '',
            ticketPrice: 0,
            movieTitle: '',
            auditoriumName: '',
            cinemaName: '',
            movieRating: 0
        },
        tickets: [],
    },)

    useEffect(() => {
        getProjection(id);
    }, [])

    const getAuditoriumSeats = (id) =>{
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };


        var url = serviceConfig.baseURL+'/api/seats/getByAuditId/'+ id;
        fetch(url, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, seats: data });
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getProjection = (id) =>{
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        var url = serviceConfig.baseURL+'/api/projections/getById/'+id;
        fetch(url, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, projection: data });
                    getAuditoriumSeats(data.auditoriumId);
                    getTickets(data.id)
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getTickets = (id) =>{
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        var url = serviceConfig.baseURL+'/api/tickets/getTicketsByProjection/'+ id;
        fetch(url, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, tickets: data });
                }
                console.log(data)
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getSeatsInAuditorium = () => {
        {
            let test = [] as any
            const seats = state.seats;
            seats.forEach(s => {
                if(s.number == 1)
                    test.push(<br key={s.id+'a'}></br>)
                test.push(<Button key={s.id.toString()}>{s.row}+{s.number}</Button>)
            });
            //console.log(state.seats)

            return test;
        }
    }

    const getSeats = () => {
        return(
            <div>
                {getSeatsInAuditorium()}
            </div>
        )
    }

    return(
        <div>
            {getSeats()}
        </div>
    )
}

export default withRouter(ReserveTicket)