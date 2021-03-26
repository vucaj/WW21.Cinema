import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";
import Spinner from "../../Spinner";
import { withRouter } from "react-router";
import { IAuditorium } from "../../../models";
import { Card, Row, Button } from 'antd';
import "antd/dist/antd.css";

interface IState{
    auditoriums: IAuditorium[];
    isLoading: boolean;
}

const ShowAllAuditoriums2: React.FC = (props:any) => {
    const [state, setState] = useState<IState>({
       auditoriums: [
           {
               id: "",
               name: "",
               cinemaId: "",
           },
       ],
       isLoading: true,
    });

    const getAuditoriums = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({...state,isLoading:true});
        fetch(`${serviceConfig.baseURL}/api/Auditoriums/getAll`, requestOptions).then((response) => {
            if(!response.ok){
                return Promise.reject(response);
            }
            return response.json();
        }).then((data) => {
            if(data){
                setState({auditoriums: data, isLoading:false});
            }
        }).catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({...state, isLoading:false});
        });
    };

    useEffect(() => {
        getAuditoriums();
    }, [getAuditoriums]);

    const fillCardWithData = () => {
        return state.auditoriums.map((auditorium) => {
            return(
                <Card>
                    <div>
                        <h2><b>Auditorium Name: </b>{auditorium.name}</h2>
                        <p><b>Cinema Id: </b>{auditorium.cinemaId}</p>
                    </div>
                </Card>
            );
        });
    };

    const cardData = fillCardWithData();
    const card = (
        <Card style={{ margin: 10 }}>
            {cardData}
        </Card>
    );

    return (
        <React.Fragment>
            <Row className="no-gutters pt-2">
                <h1 className="form-header form-heading">All Auditoriums</h1>
            </Row>
            {card}
        </React.Fragment>
    )

}

export default withRouter(ShowAllAuditoriums2);