import React from "react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../services/api";

const index = () => {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const [pageSize] = useState(5);
  const [totalPages, setTotalPages] = useState(0);
  const [totalCount, setTotalCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await api.get(
          `/product?page=${currentPage}&size=${pageSize}`
        );
        console.log(response);

        setProducts(response.data.products);
        setTotalPages(response.data.totalPagesCount);
        setTotalCount(response.data.totalProductsCount);
        setCurrentPage(response.data.currentPage);
      } catch (error) {
        console.error("Failed to fetch products:", error);
        setError("Failed to fetch products. Please try again later.");
      }
    };

    fetchProducts();
  }, []);

  return <div></div>;
};

export default index;
