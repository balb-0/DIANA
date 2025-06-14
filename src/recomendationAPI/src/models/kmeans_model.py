from sklearn.cluster import KMeans
import joblib
import pandas as pd
import os

class KMeansModel:
    def __init__(self, model_path: str):
        self.model_path = model_path
        self.model = self.load_model()

    def load_model(self):
        if os.path.exists(self.model_path):
            return joblib.load(self.model_path)
        else:
            raise FileNotFoundError(f"KMeans model not found at {self.model_path}")

    def predict_cluster(self, client_vector: pd.Series) -> int:
        client_vector = client_vector.fillna(0).values.reshape(1, -1)
        return self.model.predict(client_vector)[0]