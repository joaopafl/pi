using Microsoft.AspNetCore.Mvc;
using Projeto_MVC_Produtos.Data;
using Projeto_MVC_Produtos.Models;

namespace Projeto_MVC_Produtos.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly AppDbContext _context;

        public FornecedorController(AppDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View(_context.Fornecedores.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Fornecedor forn)
        {
            if (ModelState.IsValid)
            {
                _context.Fornecedores.Add(forn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }
        [HttpPost]
        public IActionResult Edit(Fornecedor forn)
        {
            if (ModelState.IsValid)
            {
                _context.Fornecedores.Update(forn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forn);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor == null) return NotFound();
            return View(fornecedor);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
