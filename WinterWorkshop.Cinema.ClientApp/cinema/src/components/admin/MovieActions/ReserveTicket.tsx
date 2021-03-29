import React, { useEffect, useState } from "react";
import "antd/dist/antd.css";
import {withRouter} from "react-router-dom";
import {IProjection, ISeats} from "../../../models";
import {serviceConfig} from "../../../appSettings";
import { NotificationManager } from "react-notifications";

interface IState{
    seats: ISeats[];
    projection: IProjection;
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
    })

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

        console.log(id)

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
                console.log(data)
                if (data) {
                    setState({ ...state, projection: data });
                    getAuditoriumSeats(data.auditoriumId);
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getSeats = () => {
        return(
            <div>{state.projection.auditoirumId}</div>
        )
    }

    return(
        <div>
            {getSeats()}
        </div>
    )
}

export default withRouter(ReserveTicket)