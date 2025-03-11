# Product Management Application

This application consists of a Web API backend and a React frontend for managing product information.

## Prerequisites

Before you begin, ensure you have the following installed:

* **.NET SDK:** For running the Web API.
* **Node.js and npm (or yarn):** For running the React client.
* **SQLite:** For the database.

## Getting Started

### 1. Web API (Backend)

1.  **Navigate to the Web API Directory:**

    ```bash
    cd webapi
    ```

2.  **Restore Dependencies:**

    ```bash
    dotnet restore
    ```

3.  **Update Database Connection:**

    * Open the `appsettings.json` file.
    * Update the connection string to point to your SQLite database file. For example:

        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Data Source=Products.db"
        }
        ```

        * If the database file `Products.db` doesn't exist, SQLite will create it when the application runs.
        * If you want to use an in-memory database, use:

        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Data Source=:memory:"
        }
        ```

4.  **Run Migrations (if necessary):**

    * If you have Entity Framework Core migrations, run them to create or update the database:

        ```bash
        dotnet ef database update
        ```

5.  **Run the Web API:**

    ```bash
    dotnet run
    ```

    * The API will start running on a specified port (e.g., `http://localhost:5000`). Check the console output for the exact URL.

### 2. React Client (Frontend)

1.  **Navigate to the Client Directory:**

    ```bash
    cd client
    ```

2.  **Install Dependencies:**

    ```bash
    npm install
    # or
    yarn install
    ```

3.  **Update API URL (if necessary):**

    * Open the `src/services/api.tsx` file.
    * Ensure that the `API_URL` variable points to the correct URL of your running Web API (e.g., `http://localhost:5000/api`).

4.  **Start the React Client:**

    ```bash
    npm run dev
    # or
    yarn dev
    ```

    * The React application will open in your default browser, typically on `http://localhost:5173` (or the URL shown in your terminal). Vite uses port 5173 by default.

5.  **Build the React Client (for production):** 

    ```bash
    npm run build
    # or
    yarn build
    ```

    * The production build will be created in the `dist` directory.

6. **Preview the production build:**

    ```bash
    npm run preview
    # or
    yarn preview
    ```
    * This will allow you to preview the production build in your browser.

## Accessing the Application

* **Web API:** Access the API endpoints using tools like Postman, Insomnia, or your browser.
* **React Client:** Open your web browser and navigate to `http://localhost:5173` (or the URL shown in your terminal).

## Notes

* When using SQLite, the database file (`Products.db` in the example above) will be created in the same directory as your Web API executable by default.
* If you are using an in-memory database, the data will be lost when the application stops.
* If you encounter CORS errors, make sure that your Web API is configured to allow requests from your React client's origin (`http://localhost:5173`).
* If you are using different ports, make sure to update the `api.ts` file to reflect the change.
* Ensure that your database server is running before starting the Web API.
* If you are using a different package manager than npm or yarn, make sure to change the commands in the README to reflect that.
* If you are using a different operating system than the one you are writing the readme for, be sure to add any operating system specific instructions.
* Vite uses port 5173 by default.

## Deployment

For information on deploying the application to a production environment, please refer to the deployment documentation.

## Contributing

If you'd like to contribute to this project, please follow the contributing guidelines.

## License

This project is licensed under the [MIT License](LICENSE).