import React, { useState } from "react";

const ProductSearch = ({ onSearch }) => {
  const [query, setQuery] = useState("");

  const handleInputChange = (e) => {
    setQuery(e.target.value);
  };

  const handleSearch = () => {
    onSearch(query);
  };

  return (
    <div>
      <input
        type="text"
        value={query}
        onChange={handleInputChange}
        placeholder="Search for a product..."
      />
      <button onClick={handleSearch}>Search</button>
    </div>
  );
};

export default ProductSearch;
