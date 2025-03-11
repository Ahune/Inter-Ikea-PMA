import axios from 'axios';
export interface ProductType {
  id: number;
  name: string;
}

export interface Colour {
  id: number;
  name: string;
}

export const fetchProductTypesFromTxt = async (): Promise<ProductType[]> => {
  try {
    const response = await axios.get('../../public/data/product-type.txt');
    const lines = response.data.split('\n').filter((line: string) => line.trim() !== '');
    const productTypes: ProductType[] = lines.map((line: string, index: number) => ({
      id: index + 1,
      name: line.trim(),
    }));
    return productTypes;
  } catch (error) {
    console.error('Error fetching product types:', error);
    return [];
  }
};

export const fetchColorsFromTxt = async (): Promise<Colour[]> => {
  try {
    const response = await axios.get('../../public/data/colour.txt');
    const lines = response.data.split('\n').filter((line: string) => line.trim() !== '');
    const colors: Colour[] = lines.map((line: string, index: number) => ({
      id: index + 1,
      name: line.trim(),
    }));
    return colors;
  } catch (error) {
    console.error('Error fetching colors:', error);
    return [];
  }
};