export const getRole = () => {
  const roleEnum = localStorage.getItem('role');

  var role = '';

  if(roleEnum == '0'){
    role = 'user';
  }
  else if(roleEnum == '1'){
    role = 'admin'
  }
  else if(roleEnum == '2'){
    role = 'superUser'
  }
  return role;
};

export const getUserName = () => {
  let decodedToken = getDecodedToken();
  if (!decodedToken) {
    return;
  }

  return decodedToken.sub;
};

export const getDecodedToken = () => {
  var jwtDecoder = require("jwt-decode");
  const token = localStorage.getItem("jwt");

  if (!token) {
    return false;
  }
  return jwtDecoder(token);
};

export const getTokenExp = () => {
  let token = getDecodedToken();
  if (token) {
    return token.exp;
  }
};

export const isAdmin = () => {
  return getRole() == "admin";
};
export const isSuperUser = () => {
  return getRole() == "superUser";
};

export const isUser = () => {
  return getRole() == "user";
};

export const isGuest = () => {
  return getRole() == "guest";
};
