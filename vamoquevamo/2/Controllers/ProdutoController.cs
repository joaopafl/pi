using Microsoft.AspNetCore.Mvc;
using Projeto_MVC_Produtos.Data;
using Projeto_MVC_Produtos.Models;

namespace Projeto_MVC_Produtos.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;
        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Produtos.ToList());
        }
        [HttpPost]
        public IActionResult Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null) return NotFound();
            return View(produto);
        }
        [HttpPost]
        public IActionResult Edit(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if(produto == null) return NotFound();
            return View(produto);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var produto = _context.Produtos.Find(id);
            if(produto != null) { 
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
