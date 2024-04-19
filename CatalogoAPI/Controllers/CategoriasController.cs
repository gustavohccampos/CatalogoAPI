using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly ApplicationContext _context;

        public CategoriasController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
                //throw new DataMisalignedException(); 
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no Servidor!");
            }


        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                // return _context.Categorias.AsNoTracking().Include(p => p.Produtos).ToList();
                return _context.Categorias.AsNoTracking().Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
            }
            catch (System.Exception)
            {

                throw;
            }

        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<IEnumerable<Categoria>> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não encontrada!");
                }
                return Ok(categoria);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            { return BadRequest(); }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId) { return BadRequest(); }
            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            //var produto = _context.Categorias.Find(id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada!");
            }
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }



    }
}