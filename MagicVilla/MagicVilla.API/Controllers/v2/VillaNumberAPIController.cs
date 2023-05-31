using System.Net;
using MagicVilla.API.Models;
using MagicVilla.API.Models.Dto;
using MagicVilla.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IVillaRepository villaRepository)
        {
            _villaNumberRepository = dbVillaNumber;
            _villaRepository = villaRepository;
            _response = new();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
