import {Table, Typography, Input, Button, Space, Card} from 'antd';
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import "antd/dist/antd.css";
import {IAuditorium, ICinema, IMovie, IProjection} from "../../models";
import {serviceConfig} from "../../appSettings";
import {withRouter} from "react-router-dom";
import {isUser} from "../helpers/authCheck";


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
    searchText: string;
    searchedColumn: string;
    expandedRowKeys: string[];
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
                numberOfOscars: 0,
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
        cinemas: [{ id: "", name: "", addressId: 0, cityName: "", country: "", latitude: 0, longitude: 0, streetName: "" }],
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
                rating: 0,
                numberOfOscars: 0,
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
        searchText: '',
        searchedColumn: '',
        expandedRowKeys: []

    })

    useEffect(() => {
        //getAllCinemas();
        //getAllAuditoria();
        //getMoviesWithFutureProjections();
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

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/projections/getAllFuture`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, projections: data, isLoading: false });
                }
                console.log(data)
            })
            .catch((response) => {
                setState({ ...state, isLoading: false })
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

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/movies/withFutureProjections`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                console.log(data)
                setState({ ...state, movies: data.valueOf()});
                console.log(state.movies)
            })
            .catch((response) => {
                setState({ ...state, isLoading: false })
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

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Cinemas/getAll`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, cinemas: data, isLoading: false });
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, isLoading: false });
            });
    };

    const getAllAuditoria = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Auditoriums/getAll`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, auditoriums: data, isLoading: false });
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, isLoading: false });
            });
    }

    const getFormat = (value) => {
        var parts = value.toString().split('T');
        var dateParts = parts[0].split("-");

        return (parts[1] + '     ' + dateParts[2] + '.' + dateParts[1] + '.' + dateParts[0] + '.');
    }

    const formatDate = (date) => {
        return (
            <div>
                {getFormat(date)}
            </div>
        );
    }

    let searchInput;

    const getColumnSearchProps = dataIndex => ({
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters }) => (
            <div style={{ padding: 8 }}>
                <Input
                    ref={node => {
                        searchInput = node;
                    }}
                    placeholder={`Search ${dataIndex}`}
                    value={selectedKeys[0]}
                    onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter={() => handleSearch(selectedKeys, confirm, dataIndex)}
                    style={{ width: 188, marginBottom: 8, display: 'block' }}
                />
                <Space>
                    <Button
                        type="primary"
                        onClick={() => handleSearch(selectedKeys, confirm, dataIndex)}
                        icon={<SearchOutlined />}
                        size="small"
                        style={{ width: 90 }}
                    >
                        Search
                    </Button>
                    <Button onClick={() => handleReset(clearFilters)} size="small" style={{ width: 90 }}>
                        Reset
                    </Button>
                </Space>
            </div>
        ),
        filterIcon: filtered => <SearchOutlined style={{ color: filtered ? '#1890ff' : undefined }} />,
        onFilter: (value, record) =>
            record[dataIndex] ? record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()) : '',
        onFilterDropdownVisibleChange: visible => {
            if (visible) {
                setTimeout(() => searchInput.select(), 100)
            }
        },

        render: text =>
            state.searchedColumn === dataIndex ? (
                <Highlighter
                    highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
                    searchWords={[state.searchText]}
                    autoEscape
                    textToHighlight={text ? text.toString() : ''}
                />
            ) : (
                text
            ),
    });

    const handleSearch = (selectedKeys, confirm, dataIndex) => {
        confirm();
        setState({ ...state, searchText: selectedKeys[0], searchedColumn: dataIndex });
    };

    const handleReset = clearFilters => {
        clearFilters();
        setState({ ...state, searchText: '' })
    }

    const columns = [
        {
            title: "Cinema Name",
            dataIndex: 'cinemaName',
            ...getColumnSearchProps('cinemaName'),
            key: 'cinemaName'
        },
        {
            title: 'Movie Title',
            dataIndex: 'movieTitle',
            ...getColumnSearchProps('movieTitle'),
            key: 'movieTitle'
        },
        {
            title: 'Time',
            dataIndex: 'dateTime',
            key: 'dateTime',
            ...getColumnSearchProps('dateTime'),
            render: (date) => { return (formatDate(date)) }
        },
        {
            title: 'Ticket Price',
            dataIndex: 'ticketPrice',
            ...getColumnSearchProps('ticketPrice'),
            key: 'ticketPrice'
        },
        {
            title: 'Auditorium Name',
            dataIndex: 'auditoriumName',
            ...getColumnSearchProps('auditoriumName'),
            key: 'auditoriumName'
        },
        {
            title: 'Movie Rating',
            dataIndex: 'movieRating',
            key: 'movieRating'
        }


    ]

    const getGenre = (genre) => {
        switch (genre) {
            case 0:
                return "Horror";
            case 1:
                return "Action";
            case 2:
                return "Drama";
            case 3:
                return "Sci-fi";
            case 4:
                return "Crime";
            case 5:
                return "Fantasy";
            case 6:
                return "Historical";
            case 7:
                return "Romance";
            case 8:
                return "Western";
            case 9:
                return "Thriler";
            case 10:
                return "Animated";
            case 11:
                return "Adult";
            case 12:
                return "Documentary";
        }
    }

    const { Title } = Typography;

    const getDescription = (record) => {
        return (<div>
            <Title level={4}>{record.movieTitle}</Title>
            <a><b>Cinema:</b> {getGenre(record.cinemaName)}</a>
            <br></br>
            <a><b>Auditorium:</b> {record.auditoriumName}</a>
            <br></br>
            <a><b>Rating:</b> {record.movieRating}</a>
            <br></br>
            <a><b>Ticket price:</b> {record.ticketPrice}</a>

            <div style={{ margin: 0 }}>{getButton(record.id)}</div>
        </div>)
    };

    const getAuditoriumSeats = (id) =>{
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Auditoriums/getAll`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response)
                }
                return response.json()
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, auditoriums: data, isLoading: false });
                }
            })
            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, isLoading: false });
            });
    }

    const reserveTicketPage = (id) => {
        getAuditoriumById(id)
    }

    const getButton = (id) => {
        if(isUser()){
            return(
                <div>
                    <Button type='primary' onClick={() => {reserveTicketPage(id)}}>
                        Reserve Ticket
                    </Button>
                </div>
            )
        }
        return (<div></div>);
    }

    const getTable = () => {
        return (
            <div>
                <Table
                    expandable={{
                        expandedRowRender: projection => <div>{getDescription(projection)}</div>,
                    }}
                    columns={columns}
                    dataSource={state.projections}
                    rowKey={projection => projection.id}
                >


                </Table>
            </div>
        )
    }


    return (
        <React.Fragment>
            <Card style={{ margin: 10 }}>
                {getTable()}
            </Card>
        </React.Fragment>
    );
}

export default withRouter(Projection2)