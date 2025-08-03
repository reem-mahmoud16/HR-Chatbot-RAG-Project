from abc import ABC, abstractmethod
import chromadb
from langchain_text_splitters import RecursiveCharacterTextSplitter
from datasetHandler import DatasetHandler

class IVectorDBService(ABC):
    @abstractmethod
    def initialize_collection(self, document_path: str, collection_name: str):
        pass
    
    @abstractmethod
    def query(self, query_embedding, n_results: int = 5):
        pass

class ChromaDBService(IVectorDBService):
    def __init__(self, embedding_service):
        self.embedding_service = embedding_service
        self.splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=200)
        self.datasetHandler = DatasetHandler()
        self.client = None
        self.collection = None

    def initialize_collection(self, document_path: str, collection_name: str):
        self.client = chromadb.Client()
        self.collection = self.client.create_collection(name=collection_name)
        
        self.datasetHandler.ConcatAllDatasets()
        with open(document_path, "r") as doc:
            text_context = doc.read()
        
        chunks = self.splitter.split_text(text_context)
        
        for i, chunk in enumerate(chunks):
            embedding = self.embedding_service.embed_query(chunk)
            self.collection.add(
                ids=[f"doc_{i}"],
                documents=[chunk],
                embeddings=[embedding]
            )
        
        return self.collection

    def query(self, query_embedding, n_results: int = 5):
        return self.collection.query(
            query_embeddings=[query_embedding],
            n_results=n_results
        )