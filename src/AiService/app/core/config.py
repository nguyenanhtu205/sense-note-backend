from typing import Optional
from pydantic_settings import BaseSettings, SettingsConfigDict
from functools import lru_cache
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parent.parent.parent

class Settings(BaseSettings):
    OLLAMA_URL: Optional[str] = None
    DB_CONNECTION: Optional[str] = None

    model_config = SettingsConfigDict(env_file=BASE_DIR / ".env")

@lru_cache
def get_settings() -> Settings:
    return Settings()