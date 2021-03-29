import * as React from "react";
import { Row, Col } from "react-bootstrap";
import { Switch, Route } from "react-router-dom";
import "./../../index.css";

// Admin actions
import NewMovie from "./MovieActions/NewMovie";
import EditMovie from "./MovieActions/EditMovie";
import ShowAllMovies from "./MovieActions/ShowAllMovies";
import TopTenMovies from "./MovieActions/TopTenMovies";
import NewCinema from "./CinemaActions/NewCinema";
import EditCinema from "./CinemaActions/EditCinema";
import ShowAllCinemas from "./CinemaActions/ShowAllCinemas";
import NewAuditorium from "./AuditoriumActions/NewAuditorium";
import ShowAllAuditoriums from "./AuditoriumActions/ShowAllAuditoriums";
import ShowAllProjections from "./ProjectionActions/ShowAllProjections";
import NewProjection from "./ProjectionActions/NewProjection";
import ProjectionDetails from "./../user/ProjectionDetails";
import UserProfile from "./../user/UserProfile";
import Projection from "../user/Projection";
import EditAuditorium from "./AuditoriumActions/EditAuditorium";
import EditProjection from "./ProjectionActions/EditProjection";
import { checkRole } from "../helpers/roleCheck";
import { PrivateRouteAdminAndSuperUser } from "../hoc/privateRouteAdminAndSuperUser";
import { PrivateRouteAdmin } from "../hoc/privateRouteAdmin";
import { PrivateRouteAdminSuperUserAndUser } from "../hoc/privateRouteAdminSuperUserAndUser";
import ShowAllMovies2 from "./MovieActions/ShowAllMovies2";
import TopTenMovies2 from "./MovieActions/TopTenMovies2";
import Projection2 from "../user/Projection2";
import ShowAllCinemas2 from "./CinemaActions/ShowAllCinemas2";
import NewCinema2 from "./CinemaActions/NewCinema2";
import ShowAllAuditoriums2 from "./AuditoriumActions/ShowAllAuditoriums2";
import ReserveTicket from "./MovieActions/ReserveTicket";

// higher order component

const Dashboard: React.FC = () => {
  return (
    <Row className="justify-content-center no-gutters">
      {checkRole()}
      <Col className="pt-2 app-content-main">
        <Switch>
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/NewMovie"
            component={NewMovie}
          />
          <Route path="/dashboard/AllMovies" component={ShowAllMovies2} />
          <Route path="/dashboard/TopTenMovies" component={TopTenMovies2} />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/EditMovie/:id"
            component={EditMovie}
          />
          <PrivateRouteAdminSuperUserAndUser
          path='/dashboard/reserveticket/projectionId/:projectionId/auditroiumid/:auditoriumId'
          component={ReserveTicket}/>

          <PrivateRouteAdmin
            path="/dashboard/NewCinema"
            component={NewCinema2}
          />
          <Route
            path="/dashboard/AllCinemas"
            component={ShowAllCinemas2}
          />
          <PrivateRouteAdmin
            path="/dashboard/EditCinema/:id"
            component={EditCinema}
          />
          <PrivateRouteAdmin
            path="/dashboard/NewAuditorium"
            component={NewAuditorium}
          />
          <PrivateRouteAdmin
            path="/dashboard/EditAuditorium"
            component={EditAuditorium}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/AllAuditoriums"
            component={ShowAllAuditoriums2}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/AllProjections"
            component={ShowAllProjections}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/NewProjection"
            component={NewProjection}
          />
          <Route path="/dashboard/Projection" component={Projection2} />
          <Route
            path="/dashboard/ProjectionDetails"
            component={ProjectionDetails}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/EditProjection"
            component={EditProjection}
          />
          <PrivateRouteAdminSuperUserAndUser
            path="/dashboard/UserProfile"
            component={UserProfile}
          />
        </Switch>
      </Col>
    </Row>
  );
};

export default Dashboard;
