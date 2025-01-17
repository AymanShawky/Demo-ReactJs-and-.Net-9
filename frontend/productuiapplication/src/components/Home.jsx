import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const Home = () => {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState('');
  const navigate = useNavigate();


 // Pagination state
 const [currentPage, setCurrentPage] = useState(1);
 const itemsPerPage = 5; // Number of products per page

 // Calculate total pages
 const totalPages = Math.ceil(products.length / itemsPerPage);

 // Calculate the products to display on the current page
 const indexOfLastProduct = currentPage * itemsPerPage;
 const indexOfFirstProduct = indexOfLastProduct - itemsPerPage;
 const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);

 // Function to handle page change
 const paginate = (pageNumber) => setCurrentPage(pageNumber);



  const getNewAccessToken = async () => {
    const refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken) {
        navigate('/login');
      return;
    }

    try {
      const response = await axios.post('http://localhost:5001/refresh-token', { refreshToken });
      const { accessToken } = response.data;

      localStorage.setItem('accessToken', accessToken);
      return accessToken;
    } catch (err) {
      console.error(err);
      navigate('/login');
    }
  };

  const fetchProducts = async () => {
    let accessToken = localStorage.getItem('accessToken');

    if (!accessToken) {
        navigate('/login');
      return;
    }

    try {
      const response = await axios.get('http://localhost:5001/product', {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      setProducts(response.data);
    } catch (err) {
      if (err.response && err.response.status === 401) {
        // Token expired
        accessToken = await getNewAccessToken();
        if (accessToken) {
          fetchProducts(); // retry fetching products with new token
        }
      } else {
        setError('Failed to fetch products');
      }
    }
  };

  useEffect(() => {
    fetchProducts();
  }, []);

  return (
   
    <div className="container mt-4"  style={{ width: '1200px'}}>
    <h2 className="text-center text-primary mb-4">Products</h2>
    {error && <p className="text-danger">{error}</p>}

    {/* Bootstrap Table */}
    <table className="table table-bordered table-hover table-striped fixed-table">
        <thead className="thead-dark">
        <tr>
          <th scope="col">ID</th>
          <th scope="col">Title</th>
          <th scope="col">Category</th>
          <th scope="col">Price</th>
          <th scope="col">Image</th>
        </tr>
      </thead>
      <tbody>
        {currentProducts.map((product) => (
          <tr key={product.id}>
            <td>{product.id}</td>
            <td>{product.title}</td>
            <td>{product.category}</td>
            <td>${product.price}</td>
            <td>
              <img
                src={product.imageUrl}
                alt={product.title}
                style={{ width: '100px', height: '100px', objectFit: 'cover' }}
              />
            </td>
          </tr>
        ))}
      </tbody>
    </table>

{/* Pagination Controls */}
<nav>
        <ul className="pagination justify-content-center">
          {/* Previous Button */}
          <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
            <button
              className="page-link"
              onClick={() => paginate(currentPage - 1)}
              disabled={currentPage === 1}
            >
              Previous
            </button>
          </li>

          {/* Page Numbers */}
          {Array.from({ length: totalPages }, (_, index) => (
            <li
              key={index + 1}
              className={`page-item ${currentPage === index + 1 ? 'active' : ''}`}
            >
              <button className="page-link" onClick={() => paginate(index + 1)}>
                {index + 1}
              </button>
            </li>
          ))}

          {/* Next Button */}
          <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
            <button
              className="page-link"
              onClick={() => paginate(currentPage + 1)}
              disabled={currentPage === totalPages}
            >
              Next
            </button>
          </li>
        </ul>
      </nav>
    </div>

  );
};

export default Home;
