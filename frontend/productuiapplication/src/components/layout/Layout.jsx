import React from "react";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";
import { Outlet } from "react-router-dom"; // For React Router v6

const Layout = () => {
  return (
    <div className="layout">
      <Navbar />
      <div className="main-container">
        <Sidebar />
        <div className="content">
          <Outlet /> {/* This will render the current page component */}
        </div>
      </div>
    </div>
  );
};

export default Layout;
