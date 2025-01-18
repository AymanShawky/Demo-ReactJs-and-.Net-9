import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/Home";
import Login from "./components/Login";
import ProtectedRoute from "./hoc/ProtectedRoute";
import useAuth from "./hooks/useAuth";

const App = () => {
  const { loading, isAuthenticated } = useAuth();
  console.log("from HOC " + isAuthenticated);

  if (loading) {
    return <div>Loading...</div>; // Show loading until authentication state is determined
  }

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route
          path="/home"
          element={
            <ProtectedRoute
              element={<Home />}
              isAuthenticated={isAuthenticated}
            />
          }
        />
      </Routes>
    </Router>
  );
};

export default App;
