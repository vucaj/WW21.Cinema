import { IMovie, ITag } from "../../../models";
import { Table, Card, Typography, Input, Button, Space } from 'antd';
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { toast } from "react-toastify";
import { isAdmin, isGuest, isSuperUser, isUser } from "../../helpers/authCheck";
import { serviceConfig } from "../../../appSettings";
import "antd/dist/antd.css";
import { Spinner } from "react-bootstrap";

interface IState {
    movies: IMovie[];
    tags: ITag[];
    title: string;
    year: number;
    id: string;
    rating: number;
    current: boolean;
    isActive: boolean;
    tag: string;
    listOfTags: string[];
    titleError: string;
    yearError: string;
    submitted: boolean;
    isLoading: boolean
    searchText: string;
    searchedColumn: string;
    expandedRowKeys: string[];
}

const ShowAllMovies2: React.FC = (props: any) => {
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
        tags: [
            {
                name: "",
            },
        ],
        title: "",
        year: 0,
        id: "",
        rating: 0,
        current: false,
        isActive: false,
        tag: "",
        listOfTags: [""],
        titleError: "",
        yearError: '',
        submitted: false,
        isLoading: true,
        // search
        searchText: '',
        searchedColumn: '',
        expandedRowKeys: []
    });

    toast.configure();

    let userShouldSeeWholeTable;
    const shouldUseerSeeWholeTable = () => {
        if (userShouldSeeWholeTable === undefined) {
            userShouldSeeWholeTable = !isGuest() && isUser();
        }
        return userShouldSeeWholeTable;
    }

    useEffect(() => {
        getProjections()
    }, []);

    const getProjections = () => {
        if (!isAdmin() === true || !isSuperUser() === true) {
            const requestOptions = {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            };

            setState({ ...state, isLoading: true });

            fetch(`${serviceConfig.baseURL}/api/movies/all`, requestOptions)
                .then((response) => {
                    if (!response.ok) {
                        return Promise.reject(response);
                    }
                    return response.json();
                })
                .then((data) => {
                    if (data) {
                        setState({ ...state, movies: data, isLoading: false });
                    }
                })
                .catch((response) => {
                    setState({ ...state, isLoading: false });
                    NotificationManager.error(response.message || response.statusText);
                    setState({ ...state, submitted: false });
                });
        } else {
            const requestOptions = {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            };

            setState({ ...state, isLoading: true });
            fetch(`${serviceConfig.baseURL}/api/movies/current`, requestOptions)
                .then((response) => {
                    if (!response.ok) {
                        return Promise.reject(response);
                    }
                    return response.json();
                })
                .then((data) => {
                    if (data) {
                        setState({ ...state, movies: data, isLoading: false });
                    }
                })
                .catch((response) => {
                    setState({ ...state, isLoading: false });
                    NotificationManager.error(response.message || response.statusText);
                    setState({ ...state, submitted: false });
                });
        }
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

    const activateDeactivateMovie = (id) => {
        const requestOptions = {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        fetch(`${serviceConfig.baseURL}/api/Movies/activateDeactivate/${id}`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.statusText;
            })
            .then((response) => {
                getProjections();
                NotificationManager.success("Successfuly activated/deactivated movie!");
            })
            .catch((response) => {
                NotificationManager.error("Can't deactivate movie that have future projections");
                setState({ ...state, submitted: false });
            });
    };

    const removeMovie = (id) => {
        const requestOptions = {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        };

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Movies/${id}`, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                getProjections();
                NotificationManager.success("Successfully deleted movie.");
                return response.json();
            })

            .catch((response) => {
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, isLoading: false });
            });

        setTimeout(() => window.location.reload(), 1000);
    };

    const handleSearch = (selectedKeys, confirm, dataIndex) => {
        confirm();
        setState({ ...state, searchText: selectedKeys[0], searchedColumn: dataIndex });
    };

    const handleReset = clearFilters => {
        clearFilters();
        setState({ ...state, searchText: '' })
    }

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


    const columns = [
        {
            title: 'Title',
            dataIndex: 'title',
            ...getColumnSearchProps('title'),
            key: 'title'
        },
        {
            title: 'Year',
            dataIndex: 'year',
            ...getColumnSearchProps('year'),
            key: 'year'
        },
        {
            title: 'Rating',
            dataIndex: 'rating',
            ...getColumnSearchProps('rating'),
            key: 'rating'
        },
        {
            title: 'Duration',
            dataIndex: 'duration',
            ...getColumnSearchProps('duration'),
            key: 'duration'
        },
        {
            title: 'Genre',
            dataIndex: 'genre',
            key: 'genre',
            render: genre => getGenre(genre)


        },
        {
            title: 'Distributer',
            dataIndex: 'distributer',
            ...getColumnSearchProps('distributer'),
            key: 'distributer'
        },
        {
            title: 'Number Of Oscars',
            dataIndex: 'numberOfOscars',
            key: 'numberOfOscars'
        }
    ];

    const { Title } = Typography;

    const getBtn = (value: boolean, id) => {
        if (value) {
            if (!isAdmin() || !isSuperUser())
                return (<Button style={{ background: "red", borderColor: "white" }} onClick={() => activateDeactivateMovie(id)} type="primary" id="btnAD" danger>DEACTIVATE</Button>);
        }
        else if (!isAdmin() || !isSuperUser()) {
            return (<Button style={{ background: "green", borderColor: "white" }} onClick={() => activateDeactivateMovie(id)} type="primary" id="btnAd" danger>ACTIVATE</Button>)
        }
    }

    const deleteBtn = (id) => {
        if (!isAdmin() || !isSuperUser())
            return (<Button type="primary" style={{ marginLeft: 10 }} onClick={() => removeMovie(id)} danger>DELETE</Button>)
    }

    const getDescription = (record) => {
        return (<div>
            <Title level={4}>{record.title}</Title>
            <a><b>Genre:</b> {getGenre(record.genre)}</a>
            <br></br>
            <a><b>Duration:</b> {record.duration}</a>
            <br></br>
            <a><b>Description:</b></a>
            <p style={{ margin: 0 }}>{record.description}</p>
            { getBtn(record.isActive, record.id)}
            { deleteBtn(record.id)}
        </div>)
    };

    return (
        <React.Fragment>
            <Card style={{ margin: 10 }}>
                <Title level={2}>All Movies</Title>
                <Table
                    columns={columns}
                    dataSource={state.movies}
                    rowKey={record => record.id}
                    expandable={{
                        expandedRowRender: record => <div>{getDescription(record)}</div>,
                    }}
                >

                </Table>
            </Card>
        </React.Fragment>
    )
}

export default ShowAllMovies2