import React, {useCallback, useEffect, useState} from "react";
import "antd/dist/antd.css";
import {withRouter} from "react-router-dom";
import {IProjection, ISeats2, ITicket} from "../../../models";
import {serviceConfig} from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import {Button, Card, Row, Col, Typography} from 'antd';
import {forEach} from "react-bootstrap/ElementChildren";

interface IState{
    seats: ISeats2[];
    projection: IProjection;
    tickets: ITicket[];
    isReady: boolean;
    selectedSeats: ISeats2[];
    selectedRow: number;
    selectedNumberMin: number;
    selectedNumberMax: number;
}

const ReserveTicket: React.FC = (props: any) => {
    const { projectionId, auditoriumId } = props.match.params;

    const [state, setState] = useState<IState>({
        seats: [],
        tickets: [],
        isReady: false,
        selectedSeats: [],
        selectedRow: -1,
        selectedNumberMin: -1,
        selectedNumberMax: -1,
        projection: {
            id: 'string',
            dateTime: 'string',
            movieId: 'string',
            auditoirumId: 'string',
            cinemaId: 'string',
            ticketPrice: 0,
            movieTitle: 'string',
            auditoriumName: 'string',
            cinemaName: 'string',
            movieRating: 0
        }
    },)


    useEffect(() => {
        getProjection(projectionId);
        getAuditoriumSeatsAndTickets(projectionId, auditoriumId);
        //getTickets(projectionId)
        //getAuditoriumSeats(auditoriumId);
    }, [])

    const getAuditoriumSeatsAndTickets = (projectionId, auditoriumId) => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };


        var url = `${serviceConfig.baseURL}/api/seats/getByAuditIdAndProjectionId/auditId/${auditoriumId}/projectionId/${projectionId}`;
        fetch(url, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState(prevState => {return { ...prevState, seats: data }});
                    console.log(data);
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

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
                console.log('getAuditoirumSeats')
                if (data) {
                    setState(prevState => {return { ...prevState, seats: data }});
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
                console.log('getProjection')
                if (data) {
                    setState( prevState => {return {...prevState, projection: data }});

                    console.log(state)
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
                console.log('getTickets', data)

                if (data) {
                    setState(prevState => {return {...prevState, seats: state.seats, tickets: data}});
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
            });
    }

    const getSeatsInAuditorium = () => {
        {
            console.log(state)

                let test = [] as any
                const seats = state.seats;

                seats.forEach(s => {
                    if(s.number == 1){
                        test.push(<br key={s.id+'a'}></br>)
                        test.push(<a key={s.id+'b'}>Row: {s.row}</a>)
                    }

                    if(!s.isFree){
                        test.push(<Button key={s.id} disabled style={{margin: 10, background:'#FF6666'}}>{s.number}</Button>)
                    }
                    else if(s.seatType == 1){
                        test.push(<Button key={s.id} onClick={() => reserve(s)} style={{margin: 10, background:'#D4AF37'}}>{s.number}</Button>)
                    }
                    else if(s.seatType == 0){
                        test.push(<Button key={s.id} onClick={() => reserve(s)} style={{margin: 10, background: '#99FF99'}}>{s.number}</Button>)
                    }
                    else if(s.seatType == 2) {
                        test.push(<Button key={s.id} onClick={() => reserve(s)} style={{margin: 10, background:'#FF5BA5'}}>{s.number}</Button>)
                    }
                });

                return test;
        }
    }

    const reserve = (s: ISeats2) => {
        let list = state.selectedSeats
        let max = state.selectedNumberMax;
        let min = state.selectedNumberMin;
        if(list.findIndex(seat => seat == s) < 0){
            if(list.length == 0){
                list.push(s)
                setState(prevState => {return {...prevState, selectedSeats: list, selectedRow: s.row, selectedNumberMin: s.number, selectedNumberMax: s.number}})
                console.log(state.selectedSeats);
            }
            else if(s.number > max + 1 || s.number < min -1){
                NotificationManager.error('Select seat next to already selected!');
            }
            else if(s.number == max +1 || s.number == min-1){
                list.push(s)
                if(s.number > max)
                    setState(prevState => {return {...prevState, selectedSeats: list, selectedNumberMax: s.number}})
                else
                    setState(prevState => {return {...prevState, selectedSeats: list, selectedNumberMin: s.number}})
            }
            else if(s.row == state.selectedRow){
                list.push(s)
                if(s.number > max)
                    setState(prevState => {return {...prevState, selectedSeats: list, selectedNumberMax: s.number}})
                else
                    setState(prevState => {return {...prevState, selectedSeats: list, selectedNumberMin: s.number}})
                console.log(state.selectedSeats);
            }
            else{
                NotificationManager.error('Select seats from same row!');
            }

        }

    }

    const { Title } = Typography;

    const getPrices = () => {
        return (<div>
            <Title level={4}>Price List:</Title>
            <a><b>Regular:</b> {state.projection?.ticketPrice}</a>
            <br></br>
            <a><b>Love:</b> {getLove(state.projection?.ticketPrice)}</a>
            <br></br>
            <a><b>VIP:</b> {getVip(state.projection?.ticketPrice)}</a>
        </div>)
    };

    const getLove = (price) =>{
        return price + 20;
    }

    const getVip = (price) => {
        return price + 50;
    }

    const getFormatedSelectedSeats = () => {
        let test = [] as any

        state.selectedSeats.forEach( s => {
            test.push(<br></br>)
            test.push(<a>Row: {s.row} Number: {s.number}</a>)
        })

        return test;
    }

    const getTotalPrice = () => {
        let sum = 0;
        state.selectedSeats.forEach(s => {
            if(s.seatType == 0){
                sum = sum + state.projection.ticketPrice;
            }
            else if(s.seatType == 1){
                sum = sum + getVip(state.projection.ticketPrice);
            }
            else{
                sum = sum + getLove(state.projection.ticketPrice);
            }
        })

        return sum;
    }

    const getButtons = () => {
        if(state.selectedSeats.length > 0)
            return (
                <div>
                    <Title level={5}>Price: {getTotalPrice()}</Title>
                    <Button style={{margin: 5}} type="primary">
                        Reserve
                    </Button>
                    <Button style={{margin: 5}} danger>
                        Clear All
                    </Button>
                </div>
            )
        return(<div></div>)
    }

    const getSelectedSeats = () => {
        return (<div>
            {getFormatedSelectedSeats()}
            {getButtons()}
        </div>)

    }

    return(
        <React.Fragment>
            <Row>
                <Col span={15}>
                    <Card style={{margin: 10}}>
                        <Title level={4}>Select Seats</Title>
                        {getSeatsInAuditorium()}
                    </Card>
                </Col>
                <Col span={3}>
                    <Card style={{margin: 10}}>
                        {getPrices()}
                    </Card>
                </Col>
                <Col span={5}>
                    <Card style={{margin: 10}}>
                        <Title level={4}>Selected Seats:</Title>
                        {getSelectedSeats()}
                    </Card>
                </Col>
            </Row>
        </React.Fragment>
    )
}

export default ReserveTicket