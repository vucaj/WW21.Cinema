import { IMovie } from "../../../models";
import React, { useEffect, useState } from "react";
import { serviceConfig } from "../../../appSettings";
import { NotificationManager } from "react-notifications";
import { Button, Card, Input, Space, Table, Typography, Tabs, Select } from "antd";
import Highlighter from 'react-highlight-words';
import { SearchOutlined } from "@ant-design/icons";




interface IState {
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
                rating: 0,
                numberOfOscars: 0,
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
                rating: 0,
                numberOfOscars: 0,
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

    useEffect(() => {
        getProjections();
    }, [])

    const getProjections = () => {
        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            }
        };

        setState({ ...state, isLoading: true });
        fetch(`${serviceConfig.baseURL}/api/Movies/top`, requestOptions)
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
            ...getColumnSearchProps('numberOfOscars'),
            key: 'numberOfOscars'
        }
    ];

    const { Title } = Typography;

    const getDescription = (record) => {
        return (<div>
            <Title level={4}>{record.title}</Title>
            <b>Genre:</b> {getGenre(record.genre)}
            <br></br>
            <b>Duration:</b> {record.duration}
            <br></br>
            <b>Description:</b>
            <p style={{ margin: 0 }}>{record.description}</p>
        </div>)
    };

    const getTopTenAllTime = () => {
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
        setState({ ...state, selectedYear: parseInt(value) })

        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("jwt")}`
            }
        };

        setState({ ...state, isLoading: true, isSelectedYear: true });

        var api = `${serviceConfig.baseURL}/api/Movies/top-` + value;

        fetch(api, requestOptions)
            .then((response) => {
                if (!response.ok) {
                    return Promise.reject(response);
                }
                return response.json();
            })
            .then((data) => {
                if (data) {
                    setState({ ...state, filteredMoviesByYear: data, isLoading: false });
                }
            })
            .catch((response) => {
                setState({ ...state, isLoading: false });
                NotificationManager.error(response.message || response.statusText);
                setState({ ...state, submitted: false });
            })
    }

    const { Option } = Select;

    const getOptions = () => {
        return (
            <Select.OptGroup>
                <Option value="1950">1950</Option>
                <Option value="1951">1951</Option>
                <Option value="1952">1952</Option>
                <Option value="1953">1953</Option>
                <Option value="1954">1954</Option>
                <Option value="1955">1955</Option>
                <Option value="1956">1956</Option>
                <Option value="1957">1957</Option>
                <Option value="1958">1958</Option>
                <Option value="1959">1959</Option>
                <Option value="1960">1960</Option>
                <Option value="1961">1961</Option>
                <Option value="1962">1962</Option>
                <Option value="1963">1963</Option>
                <Option value="1964">1964</Option>
                <Option value="1965">1965</Option>
                <Option value="1966">1966</Option>
                <Option value="1967">1967</Option>
                <Option value="1968">1968</Option>
                <Option value="1969">1969</Option>
                <Option value="1970">1970</Option>
                <Option value="1971">1971</Option>
                <Option value="1972">1972</Option>
                <Option value="1973">1973</Option>
                <Option value="1974">1974</Option>
                <Option value="1975">1975</Option>
                <Option value="1976">1976</Option>
                <Option value="1977">1977</Option>
                <Option value="1978">1978</Option>
                <Option value="1979">1979</Option>
                <Option value="1980">1980</Option>
                <Option value="1981">1981</Option>
                <Option value="1982">1982</Option>
                <Option value="1983">1983</Option>
                <Option value="1984">1984</Option>
                <Option value="1985">1985</Option>
                <Option value="1986">1986</Option>
                <Option value="1987">1987</Option>
                <Option value="1988">1988</Option>
                <Option value="1989">1989</Option>
                <Option value="1990">1990</Option>
                <Option value="1991">1991</Option>
                <Option value="1992">1992</Option>
                <Option value="1993">1993</Option>
                <Option value="1994">1994</Option>
                <Option value="1995">1995</Option>
                <Option value="1996">1996</Option>
                <Option value="1997">1997</Option>
                <Option value="1998">1998</Option>
                <Option value="1999">1999</Option>
                <Option value="2000">2000</Option>
                <Option value="2001">2001</Option>
                <Option value="2002">2002</Option>
                <Option value="2003">2003</Option>
                <Option value="2004">2004</Option>
                <Option value="2005">2005</Option>
                <Option value="2006">2006</Option>
                <Option value="2007">2007</Option>
                <Option value="2008">2008</Option>
                <Option value="2009">2009</Option>
                <Option value="2010">2010</Option>
                <Option value="2011">2011</Option>
                <Option value="2012">2012</Option>
                <Option value="2013">2013</Option>
                <Option value="2014">2014</Option>
                <Option value="2015">2015</Option>
                <Option value="2016">2016</Option>
                <Option value="2017">2017</Option>
                <Option value="2018">2018</Option>
                <Option value="2019">2019</Option>
                <Option value="2020">2020</Option>
                <Option value="2021">2021</Option>

            </Select.OptGroup>
        )
    }

    const getSelect = () => {
        return (
            <div>
                <Select defaultValue="2021" style={{ width: 120 }} onChange={(value) => handleYearChange(value)}>
                    {getOptions()}
                </Select>
            </div>
        )
    }

    const getTable = () => {
        return (
            <div>
                <Table
                    columns={columns}
                    dataSource={state.filteredMoviesByYear}
                    rowKey={record => record.id}

                    expandable={{
                        expandedRowRender: record => <div>{getDescription(record)}</div>,

                    }}
                >
                </Table>
            </div>
        );
    }

    const getTopTenByYear = () => {
        return (
            <div>
                <Title level={2}>Top 10: {state.selectedYear}</Title>
                {getSelect()}
                {getTable()}

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