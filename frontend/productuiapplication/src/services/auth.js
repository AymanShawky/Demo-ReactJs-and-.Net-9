import api from "../services/api";

// removing the tokens from localStorage
export const logout = () => {
    localStorage.removeItem('access-token');
    localStorage.removeItem('refresh-token');
  };
  

  export const login = async (username, password) => {
    try {
      const response = await api.post('/auth/login', { username, password });
            
      const { token, refreshToken } = response.data;

      // Save tokens to localStorage or sessionStorage
      localStorage.setItem('access-token', token);
      localStorage.setItem('refresh-token', refreshToken);

      return true;

    } catch (error) {
      console.error(error);
      return false;
    }
  }