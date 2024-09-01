using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using APIPrueba.Models;


namespace APIPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        public readonly MoviesDbContext _dbcontext;

        public MoviesController(MoviesDbContext _context)
        {
            _dbcontext = _context;
        }
        [HttpGet]
        [Route("lista")]
        public IActionResult Get()
        {
            List<Movie>lista = new List<Movie>();
            try
            {
                lista = _dbcontext.Movies.Include(c=>c.oDirector).ToList();
                return StatusCode(StatusCodes.Status200OK, new {mensaje="ok",response=lista});
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });

            }
        }
        [HttpGet]
        [Route("Obter/{idMovie:int}")]
        public IActionResult Get(int idMovie)
        {
            Movie lista = new Movie();
            Movie oMovie = _dbcontext.Movies.Find(idMovie);
            if (oMovie == null)
            {
                return BadRequest("Pelicula no encontrada");
            }
            try
            {
                oMovie = _dbcontext.Movies.Include(c => c.oDirector).Where(p => p.Id == idMovie).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oMovie });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oMovie });

            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Movie objeto)
        {
            try
            {
                _dbcontext.Movies.Add(objeto);
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
        public IActionResult Editar([FromBody] Movie objeto)
        {
            if (objeto == null)
            {
                return BadRequest("El objeto no puede ser nulo.");
            }

            // Busca la película existente
            var existingMovie = _dbcontext.Movies
                .FirstOrDefault(m => m.Id == objeto.Id);

            if (existingMovie == null)
            {
                return NotFound("Película no encontrada.");
            }

            try
            {
                // Actualiza los campos solo si se proporcionan valores nuevos
                if (!string.IsNullOrEmpty(objeto.Name))
                {
                    existingMovie.Name = objeto.Name;
                }

                if (objeto.ReleaseYear.HasValue)
                {
                    existingMovie.ReleaseYear = objeto.ReleaseYear.Value;
                }

                if (!string.IsNullOrEmpty(objeto.Gender))
                {
                    existingMovie.Gender = objeto.Gender;
                }

                if (objeto.Duration.HasValue)
                {
                    existingMovie.Duration = objeto.Duration.Value;
                }

                if (objeto.Fkdirector.HasValue)
                {
                    existingMovie.Fkdirector = objeto.Fkdirector.Value;
                }

                // No es necesario marcar la entidad como modificada ya que EF lo hace automáticamente
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Película actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{idMovie:int}")]
        public IActionResult Eliminar(int idMovie)
        {
            // Busca la película existente
            var existingMovie = _dbcontext.Movies
                .FirstOrDefault(m => m.Id == idMovie);

            if (existingMovie == null)
            {
                return NotFound("Película no encontrada.");
            }

            try
            {
                // Elimina la película
                _dbcontext.Movies.Remove(existingMovie);
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
