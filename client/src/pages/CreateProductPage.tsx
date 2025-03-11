import React, { useState } from 'react';
import ProductForm from '../components/ProductForm';
import ProductList from '../components/ProductList';
import '../styles/IkeaStyles.css';

const CreateProductPage: React.FC = () => {
  const [productsUpdated, setProductsUpdated] = useState(Date.now());

  const handleProductCreated = () => {
    setProductsUpdated(Date.now());
  };

  return (
    <div className="container">
      <h2>Create New Product</h2>
      <ProductForm onProductCreated={handleProductCreated} />
      <ProductList key={productsUpdated} />
    </div>
  );
};

export default CreateProductPage;