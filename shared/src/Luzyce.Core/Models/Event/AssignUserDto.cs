namespace Luzyce.Shared.Models.Event;

public class AssignUserDto
{
    public int UserId { get; set; }
    public List<GetEvent> Logs { get; set; } = [];
}