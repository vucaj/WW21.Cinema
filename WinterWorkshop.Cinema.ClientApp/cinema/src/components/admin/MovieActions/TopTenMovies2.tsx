import {IMovie} from "../../../models";
import React, {useEffect, useState} from "react";
import {serviceConfig} from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import {Button, Card, Input, Space, Table, Typography, Tabs, Select} from "antd";
import Highlighter from 'react-highlight-words';
import {SearchOutlined} from "@ant-design/icons";




interface IState{
    movies: IMovie[];
    filteredMoviesByYear: IMovie[];
    id: string;
    bannerUrl: string;
    title: string;
    year: number;
    isActive: boolean;
    duration: number;
    distributer: string;
    description: string;
    genre: number;
    rating: number;
    isLoading: boolean;
    isSelectedYear: boolean;
    selectedYear: number;
    titleError: string;
    yearError: string;
    submitted: boolean;
    searchText: string;
    searchedColumn: string;
    expandedRowKeys: string[];

}

const TopTenMovies2: React.FC = (props: any) => {
    const [state, setState] = useState<IState>({
        bannerUrl: "", description: "", distributer: "", duration: 0, genre: 0,
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
                rating: 0
            },
        ],
        filteredMoviesByYear: [
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
                rating: 0
            },
        ],
        title: "",
        year: 0,
        id: "",
        rating: 0,
        isActive: false,
        titleError: "",
        yearError: "",
        submitted: false,
        isLoading: true,
        isSelectedYear: false,
        selectedYear: 2021,
        searchText: '',
        searchedColumn: '',
        expandedRowKeys: []
    });

    useEffect( () =>{
        getProjections();
    }, [])

    const getProjections = () => {
        const requestOptions ={
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            }
        };

        setState({...state, isLoading: true});
        fetch(`${serviceConfig.baseURL}/api/Movies/top`, requestOptions)
            .then((response) =>{
                if(!response.ok){
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if(data) {
                    setState({...state, movies: data, isLoading: false});
                }
            })
            .catch((response) => {
                setState({...state, isLoading: false});
                NotificationManager.error(response.message || response.statusText);
                setState({...state, submitted: false});
            })
    }

    let searchInput;

    const getColumnSearchProps = dataIndex => ({
        filterDropdown: ({setSelectedKeys, selectedKeys, confirm, clearFilters}) => (
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
            record[dataIndex]? record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()): '',
        onFilterDropdownVisibleChange: visible => {
            if(visible){
                setTimeout(() => searchInput.select(), 100 )
            }
        },

        render: text =>
            state.searchedColumn === dataIndex? (
                <Highlighter
                    highlightStyle = {{backgroundColor: '#ffc069', padding: 0}}
                    searchWords = {[state.searchText]}
                    autoEscape
                    textToHighlight={text? text.toString(): ''}
                />
            ):(
                text
            ),
    });

    const handleSearch = (selectedKeys, confirm, dataIndex) =>{
        confirm();
        setState({...state, searchText: selectedKeys[0], searchedColumn: dataIndex});
    };

    const handleReset = clearFilters =>{
        clearFilters();
        setState({...state, searchText: ''})
    }

    const getGenre = (genre) =>{
        switch (genre){
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

    const getDescription = (record) => {
        return (<div>
            <Title level={4}>{record.title}</Title>
            <a><b>Genre:</b> {getGenre(record.genre)}</a>
            <br></br>
            <a><b>Duration:</b> {record.duration}</a>
            <br></br>
            <a><b>Description:</b></a>
            <p style={{ margin: 0 }}>{record.description}</p>
        </div>)
    };

    const getTopTenAllTime = () =>{
        return (
            <div>
                <Title level={2}>Top 10: All Time</Title>
                <Table
                    columns={columns}
                    dataSource={state.movies}
                    rowKey={record => record.id}
                    expandable={{
                        expandedRowRender: record => <div>{getDescription(record)}</div>,
                    }}
                >
                </Table>
            </div>
        )
    }

    const handleYearChange = (value) => {
        setState({...state, selectedYear: parseInt(value)})
        console.log(state.selectedYear)
    }

    const { Option } = Select;
    const getSelect = () => {
        return (
            <div>
                <Select defaultValue="2021" style={{ width: 120 }} onChange={handleYearChange}>
                    <Option value="2021">2021</Option>
                    <Option value="2020">2020</Option>
                </Select>
            </div>
        )
    }

    const getTopTenByYear = () => {
        return(
            <div>
                <Title level={2}>Top 10: {state.selectedYear}</Title>
                {getSelect()}
                <Table
                    columns={columns}
                    dataSource={state.movies}
                    rowKey={record => record.id}
                    expandable={{
                        expandedRowRender: record => <div>{getDescription(record)}</div>,
                    }}
                >
                </Table>
            </div>
        )
    }


    const { TabPane } = Tabs;

    return (
        <React.Fragment>
            <Card style={{ margin: 10 }}>
                <Tabs defaultActiveKey="1" size="large">
                     <TabPane tab="All Time" key="1">
                         {getTopTenAllTime()}
                    </TabPane>
                    <TabPane tab="By Year" key="2">
                        {getTopTenByYear()}
                    </TabPane>
                </Tabs>
            </Card>
        </React.Fragment>
    )
}

export default TopTenMovies2