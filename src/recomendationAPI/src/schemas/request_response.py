from pydantic import BaseModel, Field
from typing import List, Optional
from datetime import datetime

class AtivoClienteInput(BaseModel):
    CodigoAtivo: str = Field(..., example="1SANPN")
    ValorInvestido: float = Field(..., example=5000.0)

class RecomendacaoRequest(BaseModel):
    ClienteId: str = Field(..., example="Cliente_Teste_001")
    Ativos: List[AtivoClienteInput] = Field(..., example=[{"CodigoAtivo": "1SANPN", "ValorInvestido": 5000.0}])
    ClassificacaoDeclarada: str = Field(..., example="Moderado", description="Conservador, Moderado ou Sofisticado")

class AtivoRecomendadoOutput(BaseModel):
    CodigoAtivo: str
    TipoAtivo: Optional[str] = "N/A"
    Subgrupo: Optional[str] = "N/A"
    Score: float

class RecomendacaoResponse(BaseModel):
    ClienteIdSolicitado: str
    PerfilCalculadoCliente: str
    PerfilDeclaradoCliente: str
    ClusterAssignado: int
    TipoAtivoSugeridoAjuste: Optional[str] = None
    AtivosRecomendados: List[AtivoRecomendadoOutput] = Field(default_factory=list)
    Timestamp: str = Field(default_factory=lambda: datetime.now().isoformat())
    
    class Config:
        json_encoders = {
            datetime: lambda v: v.isoformat()
        }