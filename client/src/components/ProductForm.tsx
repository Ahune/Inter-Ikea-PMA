import React, { useState, useEffect } from 'react';
import { addProduct, ProductType, Colour } from '../services/api';
import { fetchProductTypesFromTxt, fetchColorsFromTxt } from '../services/data';

interface ProductFormProps {
    onProductCreated: () => void;
  }

const ProductForm: React.FC<ProductFormProps> = ({ onProductCreated}) => {
  const [name, setName] = useState<string>('');
  const [productType, setProductType] = useState<string>('');
  const [colours, setColours] = useState<number[]>([]);
  const [productTypes, setProductTypes] = useState<ProductType[]>([]);
  const [availableColours, setAvailableColours] = useState<Colour[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const types = await fetchProductTypesFromTxt();
        const colors = await fetchColorsFromTxt();
        setProductTypes(types);
        setAvailableColours(colors);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };
    fetchData();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const productData = {
      name,
      productTypeId: Number(productType),
      colourIds: colours,
    };
    try {
      await addProduct(productData);
      alert('Product added successfully');
      setName('');
      setProductType('');
      setColours([]);
      onProductCreated();
    } catch (error) {
      console.error('Error adding product:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Product Name:</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Product Type:</label>
        <select value={productType} onChange={(e) => setProductType(e.target.value)} required>
          <option value="">Select Type</option>
          {productTypes.map((type) => (
            <option key={type.id} value={type.id}>{type.name}</option>
          ))}
        </select>
      </div>
      <div>
        <label>Colours:</label>
        <select
          multiple
          value={colours.map(String)} // Convert number array to string array for the select element
          onChange={(e) =>
            setColours([...e.target.selectedOptions].map((option) => Number(option.value)))
          }
        >
          {availableColours.map((color) => (
            <option key={color.id} value={color.id}>{color.name}</option>
          ))}
        </select>
      </div>
      <button type="submit">Add Product</button>
    </form>
  );
};

export default ProductForm;