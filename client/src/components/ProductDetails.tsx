import React, { useEffect, useState } from 'react';
import { getProductById, ProductDetailsResponse } from '../services/api';
import { useParams } from 'react-router-dom';

const ProductDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<ProductDetailsResponse | null>(null);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await getProductById(Number(id));
        setProduct(response.data);
      } catch (error) {
        console.error('Error fetching product details:', error);
      }
    };
    fetchProduct();
  }, [id]);

  if (!product) return <div>Loading...</div>;

  return (
    <div>
      <h2>{product.name}</h2>
      <p>Type: {product.productType}</p>
      <p>Colours: {product.colours.join(', ')}</p>
    </div>
  );
};

export default ProductDetails;