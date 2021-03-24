import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Navbar, Nav, FormControl } from "react-bootstrap";
import {Form, Button, Input, PageHeader} from 'antd';
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
    setTimeout(() => {
      if (localStorage.getItem("userLoggedIn") !== null) {
        hideLoginButtonElement();
      } else {
        hideLogoutButtonElement();
      }
    }, 500);
  }, []);

  let shouldDisplayUserProfile = false;
  const shouldShowUserProfile = () => {
    if (shouldDisplayUserProfile === undefined) {
      shouldDisplayUserProfile = !isGuest();
    }
    return shouldDisplayUserProfile;
  };

  useEffect(() => {
    let tokenExp = getTokenExp();
    let currentTimestamp = +new Date();
    if (!tokenExp || tokenExp * 1000 < currentTimestamp) {
      getTokenForGuest();
    }
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setState({ ...state, [id]: value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setState({ ...state, submitted: true });
    const { username } = state;
    if (username) {
      login();
    } else {
      setState({ ...state, submitted: false });
    }
  };

  const handleSubmitLogout = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    localStorage.removeItem("userLoggedIn");
    setState({ ...state, submitted: true });
    setState({ ...state, token: false });
    getTokenForGuest();
  };

  const hideLoginButtonElement = () => {
    let loginButton = document.getElementById("login");
    if (loginButton) {
      loginButton.style.display = "none";
    }
    let logoutButton = document.getElementById("logout");
    if (logoutButton) {
      logoutButton.style.display = "block";
    }
    document.getElementById("username")!.style.display = "none";
  };

  const hideLogoutButtonElement = () => {
    let loginButton = document.getElementById("login");

    if (loginButton) {
      loginButton.style.display = "block";
    }
    let logoutButton = document.getElementById("logout");
    if (logoutButton) {
      logoutButton.style.display = "none";
    }
    //document.getElementById("username")!.style.display = "block";
  };

  const login = () => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    fetch(
        `${serviceConfig.baseURL}/api/users/username/${state.username}`,
        requestOptions
    )
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.json();
        })
        .then((data) => {
          setState({ ...state, token: true });
          var isGuest = false;
          if (data.userName) {
            setState({ ...state, shouldHide: false });
            if (!data.isAdmin && !data.isSuperUser && !data.isUser) {
              isGuest = true;
            }
            getToken(data.isAdmin, data.isSuperUser, data.isUser, isGuest);
            NotificationManager.success(`Welcome, ${data.firstName}!`);
          }
        })
        .catch((response) => {
          NotificationManager.error("Username does not exists.");
          setState({ ...state, submitted: false });
        });
  };

  const getToken = (
      IsAdmin: boolean,
      isSuperUser: boolean,
      isUser: boolean,
      isGuest: boolean
  ) => {
    const requestOptions = {
      method: "GET",
    };
    fetch(
        `${serviceConfig.baseURL}/get-token?name=${state.username}&admin=${IsAdmin}&superUser=${isSuperUser}&user=${isUser}&guest=${isGuest}`,
        requestOptions
    )
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.json();
        })
        .then((data) => {
          console.log(data.token);
          if (data.token) {
            localStorage.setItem("jwt", data.token);
            setTimeout(() => {
              window.location.reload();
            }, 500);
          }
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          setState({ ...state, submitted: false });
        });
  };

  const getTokenForGuest = () => {
    const requestOptions = {
      method: "GET",
    };
    fetch(`${serviceConfig.baseURL}/get-token?guest=true`, requestOptions)
        .then((response) => {
          if (!response.ok) {
            return Promise.reject(response);
          }
          return response.json();
        })
        .then((data) => {
          setState({ ...state, shouldHide: true });
          if (data.token) {
            localStorage.setItem("jwt", data.token);
            window.location.reload();
          }
        })
        .catch((response) => {
          NotificationManager.error(response.message || response.statusText);
          setState({ ...state, submitted: false });
        });
    state.token = true;
  };

  const redirectToUserPage = () => {
    props.history.push(`userprofile`);
  };

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
        .then((data) =>{
          setState({...state, isLoggedIn: true})
          localStorage.setItem("jwt", data.token);
          localStorage.setItem('isLoggedIn', 'true');
          localStorage.setItem('role', data.role);
        })
        .catch((response) => {
          NotificationManager.error("Username does not exists.");
        })
  }



  const onFinish = (value) => {
    login2(value.username);
  }

  const logout = () => {
    setState({...state, isLoggedIn: false})
    localStorage.removeItem("jwt");
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('role');
    window.location.reload();
  }


  const getForm = () => {
    if(state.isLoggedIn || localStorage.getItem('jwt')){
      return(
          <Button danger onClick={logout}>
            Logout
          </Button>
      )
    }
    else if(!state.isLoggedIn || localStorage.getItem('jwt')){
      return(
          <Form
              name="basic"
              initialValues={{ remember: true }}
              onFinish={onFinish}
          >
            <Form.Item
                name="username"
                rules={[{ required: true, message: 'Please input your username!' }]}
            >
              <Input style={{width: '15%'}}/>
            </Form.Item>

            <Form.Item>
              <Button type="primary"  htmlType="submit">
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