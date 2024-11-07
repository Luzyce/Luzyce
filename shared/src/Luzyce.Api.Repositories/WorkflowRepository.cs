using Luzyce.Api.Db.AppDb.Data;

namespace Luzyce.Api.Repositories;

public class WorkflowRepository(ApplicationDbContext applicationDbContext)
{
    public List<string> GetIps()
    {
        return applicationDbContext.Clients.Where(c => c.Type == "Terminal").Select(c => c.IpAddress).ToList();
    }
}
