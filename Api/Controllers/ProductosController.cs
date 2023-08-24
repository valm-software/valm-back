using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
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
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            if (producto == null)
            {
                return BadRequest();
            }
            _unitOfWork.Productos.Add(producto);
            _unitOfWork.Save();
            return CreatedAtAction(nameof(Post), new { id = producto.Id }, producto);
        }

        //PUT: api/Productos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Put(int id, [FromBody] Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            _unitOfWork.Productos.Update(producto);
            _unitOfWork.Save();

            return producto;
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
            _unitOfWork.Save();

            return NoContent();
        }


    }
}
