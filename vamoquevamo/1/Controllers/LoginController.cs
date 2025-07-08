using Microsoft.AspNetCore.Mvc;
using Projeto_MVC_Produtos.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Projeto_MVC_Produtos.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
            if (usuario == null)
            {
                ViewBag.Erro = "Usuário ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetString("TipoUsuario", usuario.Tipo);
            HttpContext.Session.SetString("EmailUsuario", usuario.Email);

            if (usuario.Tipo == "Administrador")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
