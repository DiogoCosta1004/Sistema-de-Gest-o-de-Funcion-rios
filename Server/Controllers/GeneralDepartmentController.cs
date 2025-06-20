using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralDepartmentController(IGenericRepositoryInterface<DepartamentoGeral> genericRepositoryInterface) : GenericController<DepartamentoGeral>(genericRepositoryInterface)
    {
    }
}
