import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { withRouter } from "react-router";
import { IAuditorium } from "../../../models";
import { Card, Row, Button } from 'antd';
import "antd/dist/antd.css";
import { toast } from "react-toastify";

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

    toast.configure();

    useEffect(() => {
        getAuditoriums();
    }, []);

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
                setState({...state, auditoriums: data, isLoading:false});
            }
        }).catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({...state, isLoading:false});
        });
    };

    const removeAuditorium = (id: string) => {
        const requestOptions = {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };
        
        setState({...state, isLoading:true});
        fetch(
            `${serviceConfig.baseURL}/api/Auditoriums/${id}`,
            requestOptions
        )
        .then((response) => {
            if (!response.ok) {
                return Promise.reject(response);
              }
              getAuditoriums();
              NotificationManager.success("Successfully deleted auditorium.");
              return response.json();
        }).catch((response) =>{
            NotificationManager.error(response.message || response.statusText);
            setState({ ...state, isLoading: false });
        });
        setTimeout(() => window.location.reload(), 1000);
    };

    const deleteBtn = (id) => {
            return (
                <Button type="primary" style={{ margin: 5 }} onClick={() => removeAuditorium(id)} danger>
                    DELETE
                </Button>
            )
    }

    const fillCardWithData = () => {
        return state.auditoriums.map((auditorium) => {
            return(
                <Card>
                    <div>
                        <h2><b>Auditorium Name: </b>{auditorium.name}</h2>
                        <p><b>Cinema Id: </b>{auditorium.cinemaId}</p>
                        {deleteBtn(auditorium.id)}
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
    );
};

export default withRouter(ShowAllAuditoriums2);