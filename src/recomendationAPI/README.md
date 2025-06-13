# Recomendation API

This project implements a recommendation system using FastAPI, KMeans clustering, and similarity matrices to provide asset recommendations based on client investment profiles.

## Project Structure

```
recomendationAPI
├── src
│   ├── main.py                     # Entry point of the API
│   ├── models                       # Contains model implementations
│   │   ├── __init__.py             # Initializes the models package
│   │   ├── kmeans_model.py          # KMeans clustering model implementation
│   │   └── recommendation_engine.py  # Recommendation logic using KMeans and similarity matrices
│   ├── schemas                      # Contains Pydantic models for request and response
│   │   ├── __init__.py             # Initializes the schemas package
│   │   └── request_response.py      # Defines request and response schemas
│   └── utils                        # Utility functions for data loading
│       ├── __init__.py             # Initializes the utils package
│       └── data_loader.py           # Functions for loading data from CSV files
├── data                             # Directory for data files
│   ├── raw                          # Raw data files
│   │   └── classificacao_de_tipos.csv # Classification data for asset types
│   ├── processed                    # Processed data files
│   │   ├── barema_por_ativo.csv     # Processed asset classification data
│   │   ├── subgrupo_distribuicao.csv # Processed subgroup distribution data
│   │   └── user_item_binario.csv    # Processed user-item interaction data
│   └── models                       # Directory for model files
│       ├── kmeans_model.joblib      # Serialized KMeans model
│       └── similarity_matrices      # Directory for similarity matrices
│           ├── item_sim_matrix_cluster_0.csv # Similarity matrix for cluster 0
│           ├── item_sim_matrix_cluster_1.csv # Similarity matrix for cluster 1
│           └── item_sim_matrix_cluster_2.csv # Similarity matrix for cluster 2
├── tests                            # Directory for test files
│   ├── __init__.py                 # Initializes the tests package
│   └── test_api.py                 # Unit tests for the API
├── requirements.txt                 # Lists project dependencies
├── Dockerfile                       # Instructions for building a Docker image
└── README.md                        # Documentation for the project
```

## Setup Instructions

1. Clone the repository:
   ```
   git clone <repository-url>
   cd recomendationAPI
   ```

2. Install the required dependencies:
   ```
   pip install -r requirements.txt
   ```

3. Run the API:
   ```
   uvicorn src.main:app --reload
   ```

## Usage

Send a POST request to the `/recomendacoes` endpoint with the required JSON body to receive asset recommendations based on the client's investment profile.

## License

This project is licensed under the MIT License.