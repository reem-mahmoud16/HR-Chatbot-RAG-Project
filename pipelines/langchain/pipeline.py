import os
import sys
from pathlib import Path
import chromadb
from langchain.chat_models import init_chat_model
from langchain_openai import OpenAIEmbeddings
from langchain_core.vectorstores import InMemoryVectorStore
from langchain_text_splitters import RecursiveCharacterTextSplitter
from langchain_google_genai import GoogleGenerativeAIEmbeddings

ROOT_DIR = Path(__file__).parent.parent.parent
sys.path.append(str(ROOT_DIR))

from ..interfaces.pipeline import IRAGPipeline
from config import GOOGLE_API_KEY, GOOGLE_EMBEDDING_MODEL, LLM_CHAT_MODEL
from datasetHandler import DatasetHandler


class LangChainRAGPipeline(IRAGPipeline):

    def __init__(self, document_path: str, collection_name: str = "HR_Policy"):
        if not os.environ.get("GOOGLE_API_KEY"):
            os.environ["GOOGLE_API_KEY"] = GOOGLE_API_KEY
        
        self.llm = init_chat_model(LLM_CHAT_MODEL, model_provider="google_genai")

        self.embeddings = GoogleGenerativeAIEmbeddings(model=GOOGLE_EMBEDDING_MODEL)

        self.collection = self.initialize_vectorDB(document_path, collection_name)  


    def initialize_vectorDB(self, document_path: str, collection_name: str):
        _DatasetHandler = DatasetHandler()
        _DatasetHandler.ConcatAllDatasets()
        doc = open(document_path, "r")
        text_context = doc.read()
        

        splitter = RecursiveCharacterTextSplitter(chunk_size=500, chunk_overlap=200)
        chunks = splitter.split_text(text_context)

        client = chromadb.Client()
        collection = client.create_collection(name=collection_name)
        for i, chunk in enumerate(chunks):
            
            embedding = self.embeddings.embed_query(chunk)  
            
            collection.add(
                ids=[f"doc_{i}"],
                documents=[chunk],
                embeddings=[embedding] 
            )

        return collection

    def generate_system_prompt(self, user_prompt: str) -> str:
        
        user_prompt_embedding = self.embeddings.embed_query(user_prompt)

        context = self.collection.query(
            query_embeddings=[user_prompt_embedding],
            n_results=5
        )        

        system_prompt = f'''
                        you are an hr related questions assistant agent
                        expect a question from the user
                        make an answer based of the context you are provided only
                        do not make up unreal information

                        your context: {context["documents"][0]}

                        input: {user_prompt}
                        '''
        return system_prompt





  