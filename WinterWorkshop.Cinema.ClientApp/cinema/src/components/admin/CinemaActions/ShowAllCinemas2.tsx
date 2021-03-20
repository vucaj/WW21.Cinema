import React, { useEffect, useState } from "react";
import { ICinema, ITag, IAddress } from "../../../models";
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
    address: IAddress[];
    isLoading: boolean;
}

const ShowAllCinemas2: React.FC = (props:any) =>{
    const [state, setState] = useState<IState>({
        cinemas: [
            {
                id: "",
                name: ""    
            },
        ],
        tags: [
            {
                name: ""
            },
        ],
        address: [
            {
                streetName: "",
                cityName: "",
                country: "",
                latitude: 0,
                longitude: 0,
                id: 0
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
                <div key={cinema.id}>
                    <h1>{cinema.name}</h1>
                </div>
            );
        });
    };
    const fillCardWithAddressData = () =>{
        return state.address.map((adrs) => {
            return ( 
                <div key={adrs.id}>
                    <p><b>Address: </b>{adrs.streetName}</p>
                    <p><b>City: </b>{adrs.cityName}</p>
                    <p><b>Country: </b>{adrs.country}</p>
                </div>
            );
        });
    };

    const cardData = fillCardWithData();
    const cardAddressData = fillCardWithAddressData();
    
    const card = (
        <Card style={{ margin: 10 }}>
            {cardData}
            {cardAddressData}
        </Card>
    );
    
   // const showCard = state.isLoading ? <Spinner></Spinner> : card;

    return(
        <React.Fragment>
            <Row className="no-gutters pt-2">
                <h1 className="form-header form-heading">All Cinemas</h1>
            </Row>
            {card}
        </React.Fragment>
    );
};

export default ShowAllCinemas2;