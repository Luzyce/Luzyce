using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using Luzyce.Api.Domain.Models;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Document;
using Luzyce.Shared.Models.Kwit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[Route("api/document")]
[ApiController]
public class KwitController(KwitRepository kwitRepository, EventRepository eventRepository) : Controller
{
    private readonly KwitRepository kwitRepository = kwitRepository;
    private readonly EventRepository _eventRepository = eventRepository;

    [HttpGet("{id:int}")]
    [Authorize]
    public IActionResult GetKwit(int id)
    {
        var kwit = kwitRepository.GetKwit(id);

        if (kwit == null)
        {
            _eventRepository.AddLog(User, "Nie udało się pobrać kwitu — kwit nie został znaleziony",
                JsonSerializer.Serialize(id));
            return NotFound();
        }

        _eventRepository.AddLog(User, "Pobrano kwit", JsonSerializer.Serialize(id));

        return Ok(kwit);
    }

    [HttpGet("{id:int}/revert")]
    [Authorize]
    public IActionResult RevertKwit(int id)
    {
        var kwit = kwitRepository.GetKwit(id);

        if (kwit == null)
        {
            _eventRepository.AddLog(User, "Nie udało się cofnąć kwitu — kwit nie został znaleziony",
                JsonSerializer.Serialize(id));
            return NotFound();
        }

        kwitRepository.RevertKwit(id);

        _eventRepository.AddLog(User, "Cofnięto kwit", JsonSerializer.Serialize(id));

        return Ok();
    }

    [HttpGet("{id:int}/close")]
    [Authorize]
    public IActionResult CloseKwit(int id)
    {
        var kwit = kwitRepository.GetKwit(id);

        if (kwit == null)
        {
            _eventRepository.AddLog(User, "Nie udało się zamknąć kwitu — kwit nie został znaleziony",
                JsonSerializer.Serialize(id));
            return NotFound();
        }

        kwitRepository.CloseKwit(id);

        _eventRepository.AddLog(User, "Zamknięto kwit", JsonSerializer.Serialize(id));

        return Ok();
    }

    [HttpGet("{id:int}/unlock")]
    [Authorize]
    public IActionResult UnlockKwit(int id)
    {
        var kwit = kwitRepository.GetKwit(id);

        if (kwit == null)
        {
            _eventRepository.AddLog(User, "Nie udało się odblokować kwitu — kwit nie został znaleziony",
                JsonSerializer.Serialize(id));
            return NotFound();
        }

        kwitRepository.UnlockKwit(id);

        _eventRepository.AddLog(User, "Odblokowano kwit", JsonSerializer.Serialize(id));

        return Ok();
    }

    [HttpPut("updateKwit")]
    [Authorize]
    public IActionResult UpdateKwit([FromBody] UpdateKwit updateKwit)
    {
        var kwit = kwitRepository.GetKwitForOperation(updateKwit.Id);

        if (kwit == null)
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować kwitu — kwit nie został znaleziony",
                JsonSerializer.Serialize(updateKwit));
            return NotFound();
        }

        if (kwit.DocumentPositions.First().QuantityNetto - updateKwit.QuantityNetto > 0 ||
            kwit.DocumentPositions.First().QuantityLoss - updateKwit.QuantityLoss > 0 ||
            kwit.DocumentPositions.First().QuantityToImprove - updateKwit.QuantityToImprove > 0)
        {
            kwitRepository.CancelPreviousOperations(kwit.DocumentPositions.First().QuantityNetto - updateKwit.QuantityNetto,
                kwit.DocumentPositions.First().QuantityLoss - updateKwit.QuantityLoss,
                kwit.DocumentPositions.First().QuantityToImprove - updateKwit.QuantityToImprove, updateKwit.Id);
        }

        kwitRepository.AddOperation(new Operation
        {
            DocumentId = updateKwit.Id,
            OperatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
            QuantityNetDelta = updateKwit.QuantityNetto - kwit.DocumentPositions.First().QuantityNetto,
            QuantityLossDelta = updateKwit.QuantityLoss - kwit.DocumentPositions.First().QuantityLoss,
            QuantityToImproveDelta = updateKwit.QuantityToImprove - kwit.DocumentPositions.First().QuantityToImprove,
            ClientId = int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? "0"),
            ErrorCodeId = 1
        });

        kwitRepository.UpdateKwit(updateKwit);

        _eventRepository.AddLog(User, "Zaktualizowano kwit", JsonSerializer.Serialize(updateKwit));

        return Ok();
    }

    [HttpPost("terminal/getKwit")]
    [Authorize]
    public IActionResult TerminalGetKwit([FromBody] GetDocumentByQrCodeDto dto)
    {
        var kwit = kwitRepository
            .TerminalGetKwit(Int32.Parse(Regex.Match(dto.Number, "(kwit-(\\d+))").Groups[2].Value));

        if (kwit == null || kwit.DocumentsDefinition is not { Code: "KW" })
        {
            _eventRepository.AddLog(User, "Nie udało się pobrać kwitu — kwit nie został znaleziony lub nie jest kwitem",
                JsonSerializer.Serialize(dto));
            return NotFound(new
            {
                ErrorCode = "Nie udało się pobrać kwitu — kwit nie został znaleziony lub nie jest kwitem"
            });
        }

        if (kwitRepository.IsKwitLocked(kwit.Id) &&
            !kwitRepository.IsKwitLockedByUser(kwit.Id, int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? "")))
        {
            _eventRepository.AddLog(User, "Nie udało się pobrać kwitu — kwit jest zablokowany",
                JsonSerializer.Serialize(dto));
            return Conflict(new
            {
                ErrorCode = "Nie udało się pobrać kwitu — kwit jest zablokowany"
            });
        }

        if (kwitRepository.IsKwitClosed(kwit.Id))
        {
            _eventRepository.AddLog(User, "Nie udało się pobrać kwitu — kwit jest zamknięty",
                JsonSerializer.Serialize(dto));
            return Conflict(new
            {
                ErrorCode = "Nie udało się pobrać kwitu — kwit jest zamknięty"
            });
        }

        kwitRepository.LockKwit(kwit.Id, int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? ""));

        _eventRepository.AddLog(User, "Pobrano kwit na terminalu", JsonSerializer.Serialize(dto));

        return Ok(new GetDocumentByQrCodeResponseDto
        {
            Id = kwit.Id,
            Number = kwit.Number,
            DocumentPositions = kwitRepository.GetKwitPositions(kwit.Id).Select(x => new GetDocumentPositionResponseDto
            {
                Id = x.Id,
                QuantityNetto = x.QuantityNetto,
                QuantityLoss = x.QuantityLoss,
                QuantityToImprove = x.QuantityToImprove
            }).ToList()
        });
    }

    [HttpPut("terminal/updateKwit/{id:int}")]
    [Authorize]
    public IActionResult TerminalUpdateKwit(int id, [FromBody] UpdateDocumentPositionOnKwitDto dto)
    {
        var kwit = kwitRepository.TerminalGetKwit(id);

        if (kwit?.DocumentsDefinition is not { Code: "KW" } || (dto.Type != '+' && dto.Type != '-') ||
            kwit.LockedBy is null)
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować kwitu - nieprawidłowe żądanie",
                JsonSerializer.Serialize(dto));
            return Conflict(new
            {
                ErrorCode = "Invalid request"
            });
        }

        if (!kwitRepository.IsKwitLockedByUser(id, int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? "")))
        {
            _eventRepository.AddLog(User,
                "Nie udało się zaktualizować kwitu – kwit nie jest zablokowany lub został zablokowany przez innego użytkownika",
                JsonSerializer.Serialize(dto));
            return Conflict(new
            {
                ErrorCode = "Document is locked by another user or not locked"
            });
        }

        var documentPosition = kwitRepository.GetKwitPositions(id).FirstOrDefault();
        var documentPositionBefore = kwitRepository.GetKwitPositions(id).FirstOrDefault();

        if (documentPosition == null)
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować kwitu - pozycja dokumentu nie została znaleziona",
                JsonSerializer.Serialize(dto));
            return NotFound(new
            {
                ErrorCode = "Nie udało się zaktualizować kwitu - pozycja dokumentu nie została znaleziona"
            });
        }

        if (dto.Field == "Dobrych")
        {
            if (documentPosition.QuantityNetto == 0 && dto.Type == '-')
            {
                return Conflict(new
                {
                    ErrorCode = "Invalid request"
                });
            }

            documentPosition.QuantityNetto =
                dto.Type == '+' ? documentPosition.QuantityNetto + 1 : documentPosition.QuantityNetto - 1;
        }
        else if (dto.Field == "DoPoprawy")
        {
            if (documentPosition.QuantityToImprove == 0 && dto.Type == '-')
            {
                return Conflict(new
                {
                    ErrorCode = "Invalid request"
                });
            }

            documentPosition.QuantityToImprove = dto.Type == '+'
                ? documentPosition.QuantityToImprove + 1
                : documentPosition.QuantityToImprove - 1;
        }
        else if (dto.Field == "Zlych")
        {
            if (documentPosition.QuantityLoss == 0 && dto.Type == '-')
            {
                return Conflict(new
                {
                    ErrorCode = "Invalid request"
                });
            }

            if ((dto.ErrorCode == null || kwitRepository.GetError(dto.ErrorCode ?? "0") == null) && dto.Type == '+')
            {
                return Conflict(new
                {
                    ErrorCode = "Invalid request"
                });
            }

            documentPosition.QuantityLoss =
                dto.Type == '+' ? documentPosition.QuantityLoss + 1 : documentPosition.QuantityLoss - 1;
        }
        else
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować kwitu - nieprawidłowe pole",
                JsonSerializer.Serialize(dto));
            return Conflict(new
            {
                ErrorCode = "Invalid request"
            });
        }

        if (dto.Type == '-')
        {
            kwitRepository.CancelPreviousOperation(dto.Field);
        }

        documentPosition.QuantityGross = documentPosition.QuantityNetto + documentPosition.QuantityLoss +
                                         documentPosition.QuantityToImprove;

        var newOperation = new Operation
        {
            DocumentId = id,
            Operator = kwit.Operator,
            OperatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
            QuantityNetDelta = documentPosition.QuantityNetto - (documentPositionBefore?.QuantityNetto ?? 0),
            QuantityLossDelta = documentPosition.QuantityLoss - (documentPositionBefore?.QuantityLoss ?? 0),
            QuantityToImproveDelta =
                documentPosition.QuantityToImprove - (documentPositionBefore?.QuantityToImprove ?? 0),
            ErrorCodeId = kwitRepository.GetError(dto.ErrorCode ?? "0")?.Id,
            IsCancelled = false,
            ClientId = kwit.LockedBy.Id,
        };

        kwitRepository.AddOperation(newOperation);
        kwitRepository.UpdateKwitPosition(documentPosition);

        _eventRepository.AddLog(User, "Zaktualizowano Kwit przez Terminal", JsonSerializer.Serialize(dto));

        return Ok(new GetDocumentPositionResponseDto
        {
            Id = documentPosition.Id,
            QuantityNetto = documentPosition.QuantityNetto,
            QuantityLoss = documentPosition.QuantityLoss,
            QuantityToImprove = documentPosition.QuantityToImprove
        });
    }

    [HttpGet("terminal/closeKwit/{id:int}")]
    [Authorize]
    public IActionResult TerminalCloseKwit(int id)
    {
        if (!kwitRepository.IsKwitLockedByUser(id, int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? "")))
        {
            _eventRepository.AddLog(User,
                "Nie udało się zamknąć kwitu - kwit nie jest zablokowany lub został zablokowany przez innego użytkownika",
                JsonSerializer.Serialize(id));
            return Conflict(new
            {
                ErrorCode =
                    "Nie udało się zamknąć kwitu - kwit nie jest zablokowany lub został zablokowany przez innego użytkownika"
            });
        }

        kwitRepository.UnlockKwit(id);

        _eventRepository.AddLog(User, "Zamknięto Kwit", JsonSerializer.Serialize(id));

        return Ok();
    }
}
