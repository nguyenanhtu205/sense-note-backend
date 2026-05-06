import json
from fastapi import APIRouter, Depends
from app.services.ollama_service import get_ollama_service, OllamaService
from app.schemas.llm_request.medical_note_request import MedicalNoteRequest
from app.schemas.llm_request.abc_records_request import ABCRecord, ABCRecords

router = APIRouter()

@router.post("/medical/sensitivity-scores")
async def extract_scores(req: MedicalNoteRequest, service: OllamaService = Depends(get_ollama_service)) -> dict:
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

@router.post("/abc/suggested-intervention")
async def get_suggested_intervention(req: ABCRecord, service: OllamaService = Depends(OllamaService)) -> dict:
    prompt = f"""
    You are a pediatric behavioral analyst using Applied Behavior Analysis (ABA).

    Given the ABC record, infer the likely function of the behavior (e.g., escape, attention, sensory, access), 
    then provide exactly ONE structured intervention plan.
    
    Rules:
    - Return ONLY valid JSON
    - No explanation outside JSON
    - Write in Vietnamese only
    - Provide 2–4 clear, actionable steps
    - Each step must be specific and practical for a teacher
    - Do NOT give multiple alternative strategies
    
    ABC Record:
    Antecedent: {req.antecedent}
    Behavior: {req.behavior_description}
    Consequence: {req.consequence}
    Severity Level: {req.severity_level}
    
    Output:
    {{
      "SuggestedIntervention": "<liệt kê các bước, mỗi bước ngắn gọn, cách nhau bằng dấu chấm>""
    }}
    """

    result = await service.generate(
        payload={
            "model": "qwen2.5:1.5b",
            "prompt": prompt,
            "stream": False,
            "format": "json",
            "options": {
                "temperature": 0.1,
                "num_predict": 120,
                "top_p" : 0.9
            }
        },
        stream=False
    )

    raw = result["response"]
    raw = raw.strip()
    raw = raw.replace("```json", "").replace("```", "")

    data = json.loads(raw)
    return data

@router.post("/abc/analyze")
async def analyze_abc_trends(req: ABCRecords, service: OllamaService = Depends(get_ollama_service)) -> dict:
    prompt = f"""
    You are a pediatric behavioral analyst using Applied Behavior Analysis (ABA).

    Analyze the following ABC records and identify the overall behavioral trend of the student over the given time 
    period.
    
    Time Range:
    From: {req.start_time}
    To: {req.end_time}
    
    Tasks:
    1. Summarize the main behavioral pattern and triggers
    2. Identify if consequences are reinforcing the behavior
    3. Infer the likely function (escape, attention, sensory, access)
    4. Consider whether the behavior appears consistent, increasing, or situational over time
    5. Provide one concise intervention plan
    
    Rules:
    - Write in Vietnamese
    - Be concise but specific
    - No bullet points
    - Each field must be 1–2 sentences only
    - Return ONLY valid JSON
    
    ABC Records:
    {req.records}
    
    Output:
    {{
      "TrendSummary": "<mô tả xu hướng hành vi theo thời gian>",
      "RecommendedIntervention": "<đề xuất can thiệp>"
    }}
    """

    result = await service.generate(
        payload={
            "model": "qwen2.5:1.5b",
            "prompt": prompt,
            "stream": False,
            "format": "json",
            "options": {
                "temperature": 0.2,
                "num_predict": 150,
                "top_p": 0.9,
                "repeat_penalty": 1.1
            }
        },
        stream=False
    )

    raw = result["response"]
    raw = raw.strip()
    raw = raw.replace("```json", "").replace("```", "")

    data = json.loads(raw)
    return data