from abc import ABC, abstractmethod

class IRAGPipeline(ABC):
    @abstractmethod
    def generate_system_prompt(self, query: str) -> str:
        pass

        