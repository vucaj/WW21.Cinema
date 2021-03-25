import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  Button,
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { ICinema } from "../../../models";

interface IState {
    cinemas: ICinema[];

    name: string;
    nameError: string;

    addressId: number;
    addressIdError: string;

    submitted: boolean;
    canSubmit: boolean;
}

const NewCinema2: React.FC = (props: any) => {
    const[state, setState] = useState<IState>({
        cinemas:[
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
        name: "",
        addressId: 0,
        nameError: "",
        addressIdError: "",
        submitted: false,
        canSubmit: true,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {id, value} = e.target;
        setState({...state, [id]:value});
        //validate(id, value);
    }
    //NOTE: Proveriti zbog cega ne radi
 /*   const validate = (id:string, value:string) =>{
        if(id === "name"){
            if(value ===""){
                setState({
                    ...state,
                    nameError: "Fill in cinema name",
                    canSubmit: false,
                });
            } else {
                setState({...state, nameError:"", canSubmit: true});
            }
        }
        if(id === "addressId"){
            if(value ===""){
                setState({
                    ...state,
                    addressIdError: "Fill in address Id",
                    canSubmit: false,
                });
            } else {
                setState({...state, addressIdError:"", canSubmit: true});
            }
        }
    }; */

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        setState({...state, submitted: true});
        const {name, addressId} = state;
        if(name && addressId){
            addCinema();
        } else {
            NotificationManager.error("Please fill in data");
            setState({...state, submitted: false});
        }
    };

    const addCinema = () => {
        const data = {
            Name: state.name,
            AddressId: +state.addressId
        };

        const requestOptions = {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
            body: JSON.stringify(data),
        };

        fetch(`${serviceConfig.baseURL}/api/Cinemas/create`, requestOptions).then((response)=>{
            if(!response.ok){
                return Promise.reject(response);
            }
            return response.statusText;
        }).then((result) => {
            NotificationManager.success("Successfuly added cinema");
            props.history.push(`AllCinemas`);
        }).catch((response) => {
            NotificationManager.error(response.message || response.statusText);
            setState({...state, submitted:false});
        });
    };

    return(
        <Container>
            <Row>
                <Col>
                <h1 className="form-header">Add NEW Cinema</h1>
                <form onSubmit={handleSubmit}>
                    <FormGroup>
                        <FormControl 
                            id="name"
                            type="text"
                            placeholder="Cinema Name"
                            value={state.name}
                            className="add-new-form"
                            onChange={handleChange}
                            maxLength={30}
                        />
                        <FormText className="text-danger">
                            {state.nameError}
                        </FormText>
                    </FormGroup>
                    <FormGroup>
                        <FormControl 
                            id="addressId"
                            type="number"
                            placeholder="Address Id"
                            value={state.addressId.toString()}
                            className="add-new-form"
                            onChange={handleChange}
                        />
                        <FormText className="text-danger">
                            {state.addressIdError}
                        </FormText>
                    </FormGroup>
                    <Button
                        className="btn-add-new"
                        type="submit"
                        disabled={state.submitted || !state.canSubmit}
                    >
                        Add
                    </Button>
                </form>
                </Col>
            </Row>
        </Container>
    );
};

export default withRouter(NewCinema2);