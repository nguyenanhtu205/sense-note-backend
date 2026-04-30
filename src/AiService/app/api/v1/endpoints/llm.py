import json
from fastapi import APIRouter
from app.services.ollama_service import OllamaService
from app.schemas.llm_request.medical_note_request import MedicalNoteRequest

router = APIRouter()

service = OllamaService()

@router.post("/medical/sensitivity-scores")
async def extract_scores(req: MedicalNoteRequest) -> dict:
    prompt = f""" 
    You are a strict pediatric sensory and attention scoring system. 
    The medical note is in Vietnamese. You must ONLY score based on explicit behavioral evidence. 

    ------------------------------------------------- 
    GOLDEN RULE – ZERO BY DEFAULT 
    ------------------------------------------------- 
    If the note does NOT contain an explicit, observable behavior matching a category, score 0. 
    General statements like "bình thường", "không có gì", "trẻ ngoan" → all 0. 

    ------------------------------------------------- 
    BEHAVIOR → CATEGORY MAPPING (MUST FOLLOW EXACTLY) 
    ------------------------------------------------- 
    - "che tai", "bịt tai", "covering ears", "reacts to loud noise", "distress from sound" → SoundSensitivity 
    - "nheo mắt", "squinting", "avoiding bright light", "discomfort in sunlight/strong light" → LightSensitivity 
    - "than nóng", "than lạnh", "khó chịu nhiệt độ", "discomfort in heat/cold" → TemperatureSensitivity 
    - "từ chối chạm", "không thích ai chạm", "avoiding touch", "khó chịu khi chạm" → TouchSensitivity 
    - "quần áo len/ thô ráp/ ngứa", "tags on clothing", "discomfort with clothing textures" → TouchSensitivity 
    - "mất tập trung", "dễ xao lãng", "distracted in noisy environments", "cannot focus in classroom" → Distractibility 

    - "che tai" NEVER means LightSensitivity or anything else. 
    - "quần áo" texture complaints NEVER mean TemperatureSensitivity. 

    ------------------------------------------------- 
    SCORING RULES 
    ------------------------------------------------- 
    0 = no explicit behavioral evidence for that specific category. 
    1-3 = mild / occasional behavior explicitly described. 
    4-6 = clear repeated evidence. 
    7-8 = strong and consistent behavioral response. 
    9-10 = severe, frequent, disruptive. 

    Do NOT downscore due to function. Do NOT infer evidence. 

    ------------------------------------------------- 
    CRITICAL RULES 
    ------------------------------------------------- 
    - Score each category independently. 
    - Only assign >0 if the note contains a direct behavior mapped to that category. 
    - Negated symptoms ("không sợ ánh sáng") → 0. 
    - Do NOT average or normalize. 

    ------------------------------------------------- 
    FEW-SHOT EXAMPLES (TO LEARN CORRECT MAPPING) 
    ------------------------------------------------- 
    Example 1: 
    Note: "Bé che tai, từ chối khi ai chạm vào, và than phiền về quần áo thô ráp" 
    Answer: {{"SoundSensitivity": 8, "LightSensitivity": 0, "TemperatureSensitivity": 0, "TouchSensitivity": 8, "Distractibility": 0}} 

    Example 2: 
    Note: "Trẻ rất nhạy cảm với âm thanh, thường xuyên che tai khi nghe tiếng máy khoan" 
    Answer: {{"SoundSensitivity": 8, "LightSensitivity": 0, "TemperatureSensitivity": 0, "TouchSensitivity": 0, "Distractibility": 0}} 

    Example 3: 
    Note: "Trẻ hay nheo mắt ngoài nắng, than nóng khi trời oi bức" 
    Answer: {{"SoundSensitivity": 0, "LightSensitivity": 6, "TemperatureSensitivity": 5, "TouchSensitivity": 0, "Distractibility": 0}} 

    Example 4: 
    Note: "Trẻ bình thường" 
    Answer: {{"SoundSensitivity": 0, "LightSensitivity": 0, "TemperatureSensitivity": 0, "TouchSensitivity": 0, "Distractibility": 0}} 

    Example 5: 
    Note: "Bé chỉ hơi ngứa với áo len" 
    Answer: {{"SoundSensitivity": 0, "LightSensitivity": 0, "TemperatureSensitivity": 0, "TouchSensitivity": 2, "Distractibility": 0}} 

    ------------------------------------------------- 
    OUTPUT FORMAT 
    ------------------------------------------------- 
    Return only valid JSON (no markdown, no text): 
    {{
        "SoundSensitivity": <0-10>, 
        "LightSensitivity": <0-10>, 
        "TemperatureSensitivity": <0-10>, 
        "TouchSensitivity": <0-10>, 
        "Distractibility": <0-10>
    }} 

    ------------------------------------------------- 
    MEDICAL NOTE 
    ------------------------------------------------- 
    {req.medical_note} 
    """

    result = await service.generate(
        payload={
            "model": "qwen2.5:1.5b",
            "prompt": prompt,
            "stream": False,
            "format": "json",
            "options": {
                "temperature": 0.0,
                "num_predict": 50,
                "top_p" : 1
            }
        },
        stream=False
    )

    raw = result["response"]
    raw = raw.strip()
    raw = raw.replace("```json", "").replace("```", "")

    data = json.loads(raw)
    for k, v in data.items():
        data[k] = max(0, min(10, int(v)))

    return data