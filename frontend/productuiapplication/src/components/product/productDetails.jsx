import React, { useEffect, useState } from "react";

const ProductDetails = ({ productId }) => {
  const [product, setProduct] = useState(null);

  useEffect(() => {
    // Fetch product details from an API or data source
    const fetchProductDetails = async () => {
      try {
        const response = await fetch(`/api/products/${productId}`);
        const data = await response.json();
        setProduct(data);
      } catch (error) {
        console.error("Error fetching product details:", error);
      }
    };

    fetchProductDetails();
  }, [productId]);

  if (!product) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      <p>Price: ${product.price}</p>
      <p>Category: {product.category}</p>
      <p>Stock: {product.stock}</p>
      {/* Add more product details as needed */}
    </div>
  );
};

export default ProductDetails;
