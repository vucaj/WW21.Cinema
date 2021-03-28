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
import { Table, Tag, Space, Card } from 'antd';


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

    var requestOptions = {
        method: 'GET'
    };

    const getIMDbTop100Movies = () => {
        fetch(apiUrl, requestOptions)
            .then(response => response.text())
            .then((data) => {
                if (data) {
                    setState({ ...state, movies: data, isLoading: false })
                }
            })
            .catch(error => console.log('error', error));
    }

    // const getIMDbTop100Movies = () => {
    //     fetch('https://imdb-api.com/API/MostPopularMovies/k_l70nyqsj')
    //         .then((response) => {
    //             if (!response.ok) {
    //                 return Promise.reject(response);
    //             }
    //             return response.json();
    //         })
    //         .then((data) => {
    //             if (data) {
    //                 setState({ ...state, movies: data, isLoading: false });
    //             }
    //         })
    //         .catch((response) => {
    //             setState({ ...state, isLoading: false });
    //             NotificationManager.error(response.message || response.statusText);
    //             setState({ ...state, submitted: false });
    //         });
    // }

    const columns = [
        {
            title: 'Title',
            dataIndex: 'title',
            key: 'title'
        },
        {
            title: 'Full Title',
            dataIndex: 'fullTitle',
            key: 'fullTitle'
        },
        {
            title: 'Year',
            dataIndex: 'year',
            key: 'year'
        },
        {
            title: 'Crew',
            dataIndex: 'crew',
            key: 'crew'
        },
        {
            title: 'Rank',
            dataIndex: 'rank',
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
            key: 'image'
        },
        {
            title: 'IMDb Rating',
            dataIndex: 'iMDbRating',
            key: 'iMDbRating'
        },
        {
            title: 'IMDb Rating Count',
            dataIndex: 'iMDbRatingCount',
            key: 'iMDbRatingCount'
        },
    ]

    return (
        <React.Fragment>
            <Card style={{ margin: 10 }}>
                <Title level={2}>Top 100 Movies By IMDb</Title>
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