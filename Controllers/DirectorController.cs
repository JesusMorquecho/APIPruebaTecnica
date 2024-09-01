using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPrueba.Models;
using System.IO;

namespace APIPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        public readonly MoviesDbContext _dbcontext;
        public DirectorController(MoviesDbContext _context)
        {
            _dbcontext = _context;
        }
        [HttpGet]
        [Route("lista")]
        public IActionResult GetAll()
        {
            List<Director> lista = new List<Director>();
            try
            {
                lista = _dbcontext.Directors.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });

            }
        }
        [HttpGet]
        [Route("Obter/{idDirector:int}")]
        public IActionResult Get(int idDirector)
        {
            Director lista = new Director();
            Director oDirector = _dbcontext.Directors.Find(idDirector);
            if (oDirector == null)
            {
                return BadRequest("Director no encontrado");
            }
            try
            {
                oDirector = _dbcontext.Directors.Include(c => c.Movies).Where(p => p.Id == idDirector).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oDirector });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oDirector });

            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Director objeto)
        {
            try
            {
                _dbcontext.Directors.Add(objeto);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });

            }

        }
        [HttpPut]
        [Route("Actualizar")]
        public IActionResult Editar([FromBody] Director objeto)
        {
            if (objeto == null)
            {
                return BadRequest("El objeto no puede ser nulo.");
            }
            var existingDirector = _dbcontext.Directors
                .AsNoTracking()
                .FirstOrDefault(d => d.Id == objeto.Id);
            if (existingDirector == null)
            {
                return BadRequest("Director no encontrado.");
            }
            try
            {
                existingDirector.Name = objeto.Name ?? existingDirector.Name;
                existingDirector.Nationality = objeto.Nationality ?? existingDirector.Nationality;
                existingDirector.Age = objeto.Age ?? existingDirector.Age;
                existingDirector.Active = objeto.Active ?? existingDirector.Active;
                _dbcontext.Directors.Update(existingDirector);
                _dbcontext.SaveChanges(); 
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        

    }
        [HttpDelete]
        [Route("Eliminar/{idDirector:int}")]
        public IActionResult Eliminar(int idDirector)
        {
            var existingDirector = _dbcontext.Directors
                .FirstOrDefault(d => d.Id == idDirector);

            if (existingDirector == null)
            {
                return NotFound("Director no encontrado."); 
            }

            try
            {
                _dbcontext.Directors.Remove(existingDirector);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}
