from fastapi import FastAPI
from fastapi.responses import RedirectResponse
from app.api.v1.api import api_router

app = FastAPI(title="AI Service", version="1.0.0")

app.include_router(api_router, prefix="/api/v1")

@app.get("/", include_in_schema=False)
async def root():
    return RedirectResponse(url="/docs")