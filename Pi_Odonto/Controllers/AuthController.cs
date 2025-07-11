using Microsoft.AspNetCore.Mvc;
using Pi_Odonto.Data;
using Pi_Odonto.Models;
using Pi_Odonto.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Pi_Odonto.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login de Responsável
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            // Se já estiver logado, redireciona para o perfil
            if (User.Identity.IsAuthenticated && User.HasClaim("TipoUsuario", "Responsavel"))
            {
                return RedirectToAction("Index", "Perfil");
            }
            return View();
        }

        // POST: Login de Responsável
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var responsavel = _context.responsavel
                    .FirstOrDefault(r => r.Email == model.Email && r.Ativo);

                if (responsavel != null && PasswordHelper.VerifyPassword(model.Senha, responsavel.Senha))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, responsavel.Nome),
                        new Claim(ClaimTypes.Email, responsavel.Email),
                        new Claim("ResponsavelId", responsavel.Id.ToString()),
                        new Claim("TipoUsuario", "Responsavel")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.LembrarMe,
                        ExpiresUtc = model.LembrarMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Perfil");
                }

                ModelState.AddModelError("", "Email ou senha inválidos");
            }

            return View(model);
        }

        // GET: Login de Admin
        [HttpGet]
        [Route("Admin/Login")]
        public IActionResult AdminLogin()
        {
            // Se já estiver logado como admin, redireciona
            if (User.Identity.IsAuthenticated && User.HasClaim("TipoUsuario", "Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        // POST: Login de Admin
        [HttpPost]
        [Route("Admin/Login")]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar o admin no banco de dados
                var admin = _context.responsavel
                    .FirstOrDefault(r => r.Email == model.Email && r.Ativo);

                // Verificar se existe e se é o admin (você pode criar um campo específico depois)
                if (admin != null &&
                    model.Email == "admin@piodonto.com" &&
                    PasswordHelper.VerifyPassword(model.Senha, admin.Senha))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, admin.Nome),
                        new Claim(ClaimTypes.Email, admin.Email),
                        new Claim("ResponsavelId", admin.Id.ToString()),
                        new Claim("TipoUsuario", "Admin")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Dashboard", "Admin");
                }

                ModelState.AddModelError("", "Credenciais de administrador inválidas");
            }

            return View(model);
        }

        // Logout
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Área restrita - só para testar se está funcionando
        [HttpGet]
        [Route("AreaRestrita")]
        public IActionResult AreaRestrita()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}