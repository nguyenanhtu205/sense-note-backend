from pydantic import BaseModel

class MedicalNoteRequest(BaseModel):
    medical_note: str