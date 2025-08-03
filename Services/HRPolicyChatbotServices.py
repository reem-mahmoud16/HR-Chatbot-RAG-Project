from config import DATABASE_FULL_DOCUMENT_PATH
from pipelines.langchain.pipeline import LangChainRAGPipeline

class HRPolicyChatbot:
    def __init__(self):
        self.RAG_pipeline = LangChainRAGPipeline(DATABASE_FULL_DOCUMENT_PATH)

    def get_answer(self, user_prompt):
        prompt = self.RAG_pipeline.generate_system_prompt(user_prompt)
        LLM_response = self.RAG_pipeline.llm.invoke(prompt).content
        return LLM_response