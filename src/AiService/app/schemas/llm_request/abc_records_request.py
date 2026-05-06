from pydantic import BaseModel
from typing import List

class ABCRecord(BaseModel):
    antecedent: str
    behavior_description: str
    consequence: str
    severity_level: int

class ABCRecords(BaseModel):
    records: List[ABCRecord]
    start_time: str
    end_time: str