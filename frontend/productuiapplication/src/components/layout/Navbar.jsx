import React from "react";
import "./Navbar.css"; // Import the CSS file
import Logout from "../Logout";

const Navbar = () => {
  const username = "User"; // Replace with actual username logic

  return (
    <nav className="navbar">
      <div className="navbar-left">
        <img src="/path/to/logo.png" alt="Site Logo" className="navbar-logo" />
        <span className="navbar-site-name">Site Name</span>
      </div>
      <ul className="navbar-links">
        <li>
          <a href="/">Home</a>
        </li>
        <li>
          <a href="/about">About</a>
        </li>
        <li>
          <a href="/services">Services</a>
        </li>
      </ul>
      <div className="navbar-right">
        <span className="navbar-username">{username}</span>
        <Logout />
      </div>
    </nav>
  );
};

export default Navbar;
