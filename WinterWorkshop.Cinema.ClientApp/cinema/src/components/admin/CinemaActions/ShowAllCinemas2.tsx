import React, { useEffect, useState } from "react";
import { ICinema, ITag } from "../../../models";
import {toast} from "react-toastify";
import {isAdmin, isGuest, isSuperUser, isUser} from "../../helpers/authCheck";
import { serviceConfig } from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import { Card, Row } from 'antd';
import "antd/dist/antd.css";
import Spinner from "../../Spinner";

interface IState{
    cinemas: ICinema[];
    tags: ITag[];
    isLoading: boolean;
}

const ShowAllCinemas2: React.FC = (props:any) =>{
    const [state, setState] = useState<IState>({
        cinemas: [
            {
                id: "",
                name: "",
                addressId: 0,  
                cityName: "",
                country: "",
                latitude: 0,
                longitude: 0,
                streetName: ""
            },
        ],
        tags: [
            {
                name: ""
            },
        ],
        isLoading: true
    });

    toast.configure();

    let userShouldSeeAllCinemas;
    const shouldUserSeeAllCinemas = () => {
        if(userShouldSeeAllCinemas === undefined){
            userShouldSeeAllCinemas = !isGuest() && isUser();
        }
        return userShouldSeeAllCinemas;
    }

    useEffect(() => {
        getCinemas();
    }, []);

    const getCinemas = () => {
            const requestOptions = {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            };

            setState({...state, isLoading:true});

            fetch(`${serviceConfig.baseURL}/api/Cinemas/getAll`, requestOptions).then((response) =>{
                if(!response.ok){
                    return Promise.reject(response);
                }
                return response.json();
            }).then((data) => {
                if(data){
                    setState({...state, cinemas: data, isLoading:false});
                }
            }).catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({...state, isLoading:false});
            });
    };
 

    const fillCardWithData = () => {
        return state.cinemas.map((cinema) => {
            return (
                <Card>
                    <div>
                        <h1>{cinema.name}</h1>
                        <p><b>Address:</b> {cinema.streetName}</p>
                        <p><b>City:</b> {cinema.cityName}</p>
                        <p><b>Country:</b> {cinema.country}</p>
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

    const showCard = state.isLoading ? <Spinner></Spinner> : card;

    return(
        <React.Fragment>
            <Row className="no-gutters pt-2">
                <h1 className="form-header form-heading">All Cinemas</h1>
            </Row>
                {showCard}
        </React.Fragment>
    );
};

export default ShowAllCinemas2;