import axios from 'axios';

// Define the base URL of the API
const API_URL = 'http://localhost:5196/api'; // Change this to your actual API endpoint

// Types for Product, Product Type, and Colour
export interface ProductListResponse {
  id: number;
  name: string;
}

export interface ProductDetailsResponse {
  id: number;
  name: string;
  productType: string;
  colours: string[];
}

export interface ProductRequest {
  name: string;
  productTypeId: number;
  colourIds: number[];
}

export interface ProductType {
  id: number;
  name: string;
}

export interface Colour {
  id: number;
  name: string;
}

// API calls

export const getProducts = async () => {
  return axios.get<ProductListResponse[]>(`${API_URL}/products`);
};

export const getProductById = async (id: number) => {
  return axios.get<ProductDetailsResponse>(`${API_URL}/products/${id}`);
};

export const addProduct = async (productData: ProductRequest) => {
  return axios.post(`${API_URL}/products`, productData);
};

export const getProductTypes = async () => {
  return axios.get<ProductType[]>(`${API_URL}/product-types`);
};

export const getColors = async () => {
  return axios.get<Colour[]>(`${API_URL}/colours`);
};