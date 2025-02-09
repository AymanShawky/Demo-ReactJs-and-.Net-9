import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/Home";
import Login from "./components/Login";
import ProtectedRoute from "./hoc/ProtectedRoute";
import useAuth from "./hooks/useAuth";
import Layout from "./components/layout/Layout";
import Product from "./components/product/index";

const App = () => {
  const { loading, isAuthenticated } = useAuth();
  console.log("from HOC " + isAuthenticated);

  if (loading) {
    return <div>Loading...</div>; // Show loading until authentication state is determined
  }

  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={
            <Layout>
              <Home />
            </Layout>
          }
        />
        <Route path="/login" element={<Login />} />
        <Route
          path="/home"
          element={
            <ProtectedRoute
              element={
                <Layout>
                  <Home />
                </Layout>
              }
              isAuthenticated={isAuthenticated}
            />
          }
        />
        <Route path="/product" element={<Product />} />
      </Routes>
    </Router>
  );
};

export default App;
