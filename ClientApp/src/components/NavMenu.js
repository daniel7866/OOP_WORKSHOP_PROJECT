import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { getAddress } from "../Services";

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  showRports() {
    if(this.props.user !== null && this.props.user.email === "root")
      return (<NavLink tag={Link} className="text-dark" to={`/reports`}>Reports</NavLink>)
    else
      return null;
  }

    render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">OOP_WORKSHOP_PROJECT</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/search">Search</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/messages">Messages</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to={`/profile/${this.props.user?this.props.user.uid:""}`}>Profile</NavLink>
                </NavItem>
                <NavItem>
                  {this.showRports()}
                </NavItem>
                <NavItem>
                        <button className="btn btn-outline-dark"
                        onClick={() => {
                            var requestOptions = {
                                method: 'POST',
                                redirect: 'follow'
                            };

                            fetch(`${getAddress()}/api/user/logout`, requestOptions)
                                .then(response => response.text())
                                .then(result => { console.log(result); window.location.replace(window.location.origin); })
                                .catch(error => console.log('error', error));
                        }
                    }
                    >Log Out</button>
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
