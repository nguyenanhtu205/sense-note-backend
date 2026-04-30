import httpx
from typing import AsyncGenerator, Optional, Dict, Any
from app.core.config import get_settings

class OllamaService:
    def __init__(self):
        settings = get_settings()

        if not settings.OLLAMA_URL:
            raise ValueError("OLLAMA_URL is not configured")

        self.client = httpx.AsyncClient(
            base_url=settings.OLLAMA_URL,
            timeout=None,
            limits=httpx.Limits(
                max_connections=100,
                max_keepalive_connections=20
            )
        )

    async def request(self, method: str, url: str, *, json_body: Optional[Dict[str, Any]] = None, stream: bool):
        if not stream:
            response = await self.client.request(
                method,
                url,
                json=json_body
            )
            response.raise_for_status()
            return response.json()

        async def stream_generator() -> AsyncGenerator[str, None]:
            async with self.client.stream(
                method,
                url,
                json=json_body
            ) as stream_response:

                async for line in stream_response.aiter_lines():
                    if line:
                        yield line

        return stream_generator()

    async def generate(self, payload: Dict[str, Any], *, stream: bool):
        return await self.request(
            "POST",
            "/api/generate",
            json_body=payload,
            stream=stream
        )