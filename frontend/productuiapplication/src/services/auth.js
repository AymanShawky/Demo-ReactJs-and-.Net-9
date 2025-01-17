// services/auth.js

// This function logs the user out by removing the tokens from localStorage
export const logout = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
  };
  