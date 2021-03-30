import { IIMDbTop100Movies, ITag } from "../../../models";
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from "react";
import { NotificationManager } from "react-notifications";
import { toast } from "react-toastify";
import { isAdmin, isGuest, isSuperUser, isUser } from "../../helpers/authCheck";
import { serviceConfig } from "../../../appSettings";
import "antd/dist/antd.css";
import { Spinner } from "react-bootstrap";
import Title from "antd/lib/typography/Title";
import { Table, Tag, Space, Card, Input, Button } from 'antd';
import AllMovies from "../../AllMovies";
import "../../../index.css";


interface IState {
    movies: IIMDbTop100Movies[];
    title: string;
    year: string;
    id: string;
    rank: string;
    rankUpDown: string;
    fullTitle: string;
    image: string;
    crew: string;
    iMDbRating: string;
    iMDbRatingCount: string;
    titleError: string;
    yearError: string;
    submitted: boolean;
    isLoading: boolean
    searchText: string;
    searchedColumn: string;
    expandedRowKeys: string[];
}

const IMDbTop100Movies: React.FC = (props: any) => {
    const [state, setState] = useState<IState>({
        movies: [
            {
                id: "",
                title: "",
                year: "",
                rank: "",
                rankUpDown: "",
                fullTitle: "",
                image: "",
                crew: "",
                iMDbRating: "",
                iMDbRatingCount: "",
            },
        ],
        title: "",
        year: "",
        id: "",
        rank: "",
        rankUpDown: "",
        fullTitle: "",
        image: "",
        crew: "",
        iMDbRating: "",
        iMDbRatingCount: "",
        titleError: "",
        yearError: '',
        submitted: false,
        isLoading: true,
        // search
        searchText: '',
        searchedColumn: '',
        expandedRowKeys: []
    });

    const apiData = {
        url: 'https://imdb-api.com/API/',
        type: 'MostPopularMovies',
        apiKey: 'k_l70nyqsj'
    }

    const { url, type, apiKey } = apiData;
    const apiUrl = `${url}${type}/${apiKey}`

    toast.configure();

    useEffect(() => {
        getIMDbTop100Movies()
    }, []);

    const getIMDbTop100Movies = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "text/json"
            },
        };

        setState({ ...state, isLoading: true });

        fetch(apiUrl, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                setState({ ...state, movies: data.items, isLoading: false });
            })
            .catch((response) => {
                setState({ ...state, isLoading: false })
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, submitted: false });
            });
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
            title: 'Title',
            dataIndex: 'title',
            ...getColumnSearchProps('title'),
            key: 'title'
        },
        {
            title: 'Full Title',
            dataIndex: 'fullTitle',
            ...getColumnSearchProps('fullTitle'),
            key: 'fullTitle'
        },
        {
            title: 'Year',
            dataIndex: 'year',
            ...getColumnSearchProps('year'),
            key: 'year'
        },
        {
            title: 'Crew',
            dataIndex: 'crew',
            ...getColumnSearchProps('crew'),
            key: 'crew'
        },
        {
            title: 'Rank',
            dataIndex: 'rank',
            ...getColumnSearchProps('rank'),
            key: 'rank'
        },
        {
            title: 'Rank Up Down',
            dataIndex: 'rankUpDown',
            key: 'rankUpDown'
        },
        {
            title: 'Image',
            dataIndex: 'image',
            render: theImageURL => <img className="Image-Movie-Poster" src={theImageURL} />,
            key: 'image'
        },
    ]

    return (
        <React.Fragment>
            <Card style={{ margin: 10 }}>
                <Title level={2}>Top 100 Movies From IMDb</Title>
                <Table
                    columns={columns}
                    dataSource={state.movies}
                    rowKey={record => record.id}
                >

                </Table>
            </Card>
        </React.Fragment>
    )
}

export default IMDbTop100Movies