import { useEffect, useState } from 'react';
import { logout } from '../services/auth';
import api from '../services/api';

const useAuth = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  // Check if token exists and is valid
  useEffect(() => {
    const checkAuthStatus = async () => {
      const accessToken = localStorage.getItem('access_token');
      const refreshToken = localStorage.getItem('refresh_token');

      if (accessToken) {
        setIsAuthenticated(true);
      } else if (refreshToken) {
        // Attempt to refresh the token if access token is missing but refresh token exists
        try {
          const response = await api.post('/auth/refresh', { refresh_token: refreshToken });
          const { access_token } = response.data;
          localStorage.setItem('access_token', access_token);
          setIsAuthenticated(true);
        } catch (error) {
          console.log('Failed to refresh token');
          setIsAuthenticated(false);
          logout(); // Log out if the refresh token fails
        }
      } else {
        setIsAuthenticated(false);
      }

      setLoading(false);
    };

    checkAuthStatus();
  }, []);

  // Protect routes when authentication is not complete or failed
  if (loading) {
    return { loading: true, isAuthenticated: false };
  }

  return { loading, isAuthenticated };
};

export default useAuth;
