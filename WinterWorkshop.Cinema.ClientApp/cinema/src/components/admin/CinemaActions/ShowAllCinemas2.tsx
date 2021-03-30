import React, { useEffect, useState } from "react";
import { ICinema, ITag } from "../../../models";
import { toast } from "react-toastify";
import { isAdmin, isGuest, isSuperUser, isUser } from "../../helpers/authCheck";
import { serviceConfig } from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import { Card, Row, Button } from 'antd';
import "antd/dist/antd.css";

interface IState {
    cinemas: ICinema[];
    tags: ITag[];
    isLoading: boolean;
    id: string;
    name: string;
    addressId: number,
    cityName: string;
    country: string;
    latitude: number;
    longitude: number;
    streetName: string;
}

const ShowAllCinemas2: React.FC = (props: any) => {
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
        isLoading: true,
        id: "",
        name: "",
        addressId: 0,
        cityName: "",
        country: "",
        latitude: 0,
        longitude: 0,
        streetName: ""
    });

    toast.configure();

    let userShouldSeeAllCinemas;
    const shouldUserSeeAllCinemas = () => {
        if (userShouldSeeAllCinemas === undefined) {
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

        setState({ ...state, isLoading: true });

        fetch(`${serviceConfig.baseURL}/api/Cinemas/getAll`, requestOptions).then((response) => {
            if (!response.ok) {
                return Promise.reject(response);
            }
            return response.json();
        }).then((data) => {
            if (data) {
                setState({ ...state, cinemas: data, isLoading: false });
            }
        }).catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({ ...state, isLoading: false });
        });
    };

    const removeCinema = (id: string) => {
        const requestOptions = {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Cinemas/${id}`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                getCinemas();
                NotificationManager.success("Succesfuly delete cinema");
                return response.json();
            }).catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, isLoading: false });
            });
        setTimeout(() => window.location.reload(), 1000);
    };

    const deleteBtn = (id) => {
        if (isAdmin() || isSuperUser()) {
            return (
                <Button type="primary" style={{ margin: 5 }} onClick={() => removeCinema(id)} danger>
                    DELETE
                </Button>
            )
        }
    }

    const redirectBtn = () => {
        return(
            <Button type="primary" onClick={() => {props.history.push("NewAuditorium")}}>
                Add Auditorium
            </Button>
        )
    }

    const fillCardWithData = () => {
        return state.cinemas.map((cinema) => {
            return (
                <Card>
                    <div>
                        <h2>{cinema.name}</h2>
                        <p><b>Address:</b> {cinema.streetName}</p>
                        <p><b>City:</b> {cinema.cityName}</p>
                        <p><b>Country:</b> {cinema.country}</p>
                        {deleteBtn(cinema.id)}
                        {redirectBtn()}
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
                <h1 className="form-header form-heading">All Cinemas</h1>
            </Row>
            {card}
        </React.Fragment>
    );
};

export default ShowAllCinemas2;