using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
        }

        //GET: test
        [Authorize]
        [HttpGet("test-roles")]
        public IActionResult TestRoles()
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            return Ok(new { roles });
        }

        //GET: api/Productos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        {
            var productos = await _unitOfWork.Productos
                .GetAllAsync();
            return _Mapper.Map<List<ProductoListDto>>(productos);
        }

        //GET: api/Productos/4
        [Authorize(Roles = "Administrador")]
        [Authorize(Policy = "Gestionar Inventario")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            return _Mapper.Map<ProductoDto>(producto);
        }

        //POST: api/Productos        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto)
        {
            var producto = _Mapper.Map<Producto>(productoDto);
            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveAsync();

            if (producto == null)
            {
                return BadRequest();
            }
            productoDto.Id = producto.Id;
            return CreatedAtAction(nameof(Post), new { id = productoDto.Id }, productoDto);
        }

        //PUT: api/Productos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto)
        {
            if (productoDto == null)
            {
                return NotFound();
            }
            var producto = _Mapper.Map<Producto>(productoDto);

            _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveAsync();

            return productoDto;
        }


        //DELETE: api/Productos/7
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }


    }
}
