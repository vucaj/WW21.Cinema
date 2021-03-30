import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Navbar, Nav, FormControl } from "react-bootstrap";
import { Form, Button, Input, PageHeader } from 'antd';
import "antd/dist/antd.css";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../appSettings";
import {
  getTokenExp,
  isGuest,
  getUserName,
} from "../../src/components/helpers/authCheck";

interface IState {
  username: string;
  submitted: boolean;
  token: boolean;
  shouldHide: boolean;
  isLoggedIn: boolean
}

const Header: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    username: "",
    submitted: false,
    token: false,
    shouldHide: true,
    isLoggedIn: false
  });

  useEffect(() => {
  }, []);


  const login2 = (username) => {
    var requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    }

    var url = serviceConfig.baseURL + '/get-token/' + username;
    fetch(url, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        setState({ ...state, isLoggedIn: true })
        localStorage.setItem("jwt", data.token);
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('role', data.role);
        localStorage.setItem('userId', data.userId);
        window.location.reload();
      })
      .catch((response) => {
        NotificationManager.error("Username does not exists.");
      })
  }



  const onFinish = (value) => {
    login2(value.username);
  }

  const logout = () => {
    setState({ ...state, isLoggedIn: false })
    localStorage.removeItem("jwt");
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('role');
    localStorage.removeItem('userId');
    window.location.reload();
  }


  const getForm = () => {
    if (state.isLoggedIn || localStorage.getItem('jwt')) {
      return (
        <Button danger onClick={logout}>
          Logout
        </Button>
      )
    }
    else if (!state.isLoggedIn || localStorage.getItem('jwt')) {
      return (
        <Form
          name="basic"
          initialValues={{ remember: true }}
          onFinish={onFinish}
        >
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your username!' }]}
          >
            <Input style={{ width: '15%' }} />
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit">
              Login
              </Button>
          </Form.Item>
        </Form>
      )
    }
  }

  return (
    <React.Fragment>
      <PageHeader>
        {getForm()}
      </PageHeader>
    </React.Fragment>
  );
};

export default withRouter(Header);