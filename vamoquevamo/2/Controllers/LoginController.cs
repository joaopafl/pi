using Microsoft.AspNetCore.Mvc;
using Projeto_MVC_Produtos.Data;

namespace Projeto_MVC_Produtos.Controllers
{
    public class LoginController :Controller
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(
                u => u.Email == email && u.Senha == senha);
            if (usuario == null)
            {
                ViewBag.Erro = "Usuário ou senha inválidos";
                return View();
            }
            if (usuario.Tipo == "Administrador")
            {
                HttpContext.Session.SetString("TipoUsuario", 
                    usuario.Tipo);
                return RedirectToAction("Index", "Produto");
            }
            if(usuario.Tipo == "fornecedor")
            {
                HttpContext.Session.SetString("TipoUsuario",
                    usuario.Tipo);
                return RedirectToAction("Index", "Fornecedor");
            }
            return View();
        }
    }
}
