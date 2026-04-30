from fastapi import APIRouter
from app.api.v1.endpoints import llm

api_router = APIRouter()

api_router.include_router(llm.router, prefix="/llm_request", tags=["LLM"])