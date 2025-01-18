// services/api.js
import axios from 'axios';

// Set up axios instance
const api = axios.create({
  baseURL: 'http://localhost:5001', // Replace with your API URL
  headers: {
    'Content-Type': 'application/json',
  }
});

// Request interceptor to add Authorization token to the request headers
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('access-token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle token expiration and refreshing the token
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response && error.response.status === 401) {
      // Unauthorized request (token expired or invalid)
      const refreshToken = localStorage.getItem('refresh-token');
      if (refreshToken) {
        try {
          const response = await axios.post('http://localhost:5001/auth/refresh', { refreshToken: refreshToken });
          const { access_token } = response.data;
            
          console.log(access_token);
            
          // Store the new access token
          localStorage.setItem('access-token', access_token.Token);

          // Retry the original request with the new access token
          error.config.headers['Authorization'] = `Bearer ${access_token}`;
          return axios(error.config);
        } catch (e) {
          console.error('Failed to refresh token. Logging out...');
          // If refresh token is invalid or expired, log the user out
          localStorage.removeItem('access-token');
          localStorage.removeItem('refresh-token');
          return Promise.reject(error);
        }
      } else {
        // If there's no refresh token, log the user out
        console.error('No refresh token found. Logging out...');
        localStorage.removeItem('access-token');
        localStorage.removeItem('refresh-token');
        return Promise.reject(error);
      }
    }

    return Promise.reject(error);
  }
);

export default api;
