from abc import ABC, abstractmethod
from langchain_google_genai import GoogleGenerativeAIEmbeddings
from config import GOOGLE_API_KEY, GOOGLE_EMBEDDING_MODEL
import os

class IEmbeddingService(ABC):
    @abstractmethod
    def embed_query(self, text: str):
        pass

class GoogleEmbeddingService(IEmbeddingService):
    def __init__(self):
        if not os.environ.get("GOOGLE_API_KEY"):
            os.environ["GOOGLE_API_KEY"] = GOOGLE_API_KEY
        self.embeddings = GoogleGenerativeAIEmbeddings(model=GOOGLE_EMBEDDING_MODEL)

    def embed_query(self, text: str):
        return self.embeddings.embed_query(text)