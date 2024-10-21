using Luzyce.Api.Core.Dictionaries;
using Luzyce.Api.Domain.Models;
using Luzyce.Shared.Models.Document;

namespace Luzyce.Api.Mappers;

public static class DocumentMappers
{
    public static Document ToDocumentFromCreateDto(this CreateDocumentDto dto)
    {
        return new Document
        {
            WarehouseId = dto.WarehouseId,
            DocumentsDefinitionId = dto.DocumentsDefinitionId
        };
    }

    public static DocumentPositions ToDocumentPositionFromCreateDto(this CreateDocumentPositionDto dto)
    {
        return new DocumentPositions
        {
            QuantityNetto = dto.QuantityNetto,
            QuantityLoss = dto.QuantityLoss,
            QuantityToImprove = dto.QuantityToImprove,
            QuantityGross = dto.QuantityGross,
            StartTime = DateTime.Now.ConvertToEuropeWarsaw(),
            LampshadeId = dto.LampshadeId
        };
    }
}
