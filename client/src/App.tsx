import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import CreateProductPage from "./pages/CreateProductPage";
import ProductDetails from "./components/ProductDetails";
import ProductPage from "./pages/ProductsPage";

function App() {
  return (
    <Router>
      <div>
        <nav>
          <ul className="ikea-nav">
            <li className="ikea-nav-item"><Link to="/">Create Product</Link></li>
            <li className="ikea-nav-item"><Link to="/products">Product List</Link></li>
          </ul>
        </nav>
        <Routes>
          <Route path="/" element={<CreateProductPage />} />
          <Route path="/products" element={<ProductPage />} />
          <Route path="/products/:id" element={<ProductDetails />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;