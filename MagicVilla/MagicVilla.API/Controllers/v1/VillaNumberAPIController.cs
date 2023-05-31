using MagicVilla.API.Models.Dto;
using MagicVilla.API.Models;
using MagicVilla.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace MagicVilla.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var villaNumberList = await _villaNumberRepository.GetAll();
                _response.Result = villaNumberList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaNumberRepository.GetById(id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = villa;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaNumberCreateDTO createDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            try
            {
                if (await _villaNumberRepository.GetById(createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return BadRequest(ModelState);
                }
                if (await _villaRepository.GetById(createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                var model = await _villaNumberRepository.Create(createDTO);

                _response.Result = model;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = model.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _villaNumberRepository.Delete(id);
                if (villa == false)
                {
                    return NotFound();
                }

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }

                if (await _villaRepository.GetById(updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid!");
                    return BadRequest(ModelState);
                }

                var response = await _villaNumberRepository.Update(updateDTO);

                if (response == false)
                {
                    return NotFound();
                }

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
