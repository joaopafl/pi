using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pi_Odonto.Data;
using Pi_Odonto.Models;
using Pi_Odonto.Helpers;
using System.Security.Claims;

namespace Pi_Odonto.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        // Verificar se é responsável
        private bool IsResponsavel()
        {
            return User.HasClaim("TipoUsuario", "Responsavel");
        }

        // Obter ID do responsável logado
        private int GetResponsavelId()
        {
            var responsavelIdClaim = User.FindFirst("ResponsavelId")?.Value;
            return int.TryParse(responsavelIdClaim, out int id) ? id : 0;
        }

        // GET: Perfil do responsável
        public IActionResult Index()
        {
            if (!IsResponsavel())
            {
                return RedirectToAction("Login", "Auth");
            }

            var responsavelId = GetResponsavelId();
            var responsavel = _context.responsavel.Find(responsavelId);

            if (responsavel == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(responsavel);
        }

        // GET: Editar perfil
        public IActionResult Editar()
        {
            if (!IsResponsavel())
            {
                return RedirectToAction("Login", "Auth");
            }

            var responsavelId = GetResponsavelId();
            var responsavel = _context.responsavel.Find(responsavelId);

            if (responsavel == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Limpar senha para não mostrar no formulário
            responsavel.Senha = "";

            // Usar a view Edit do ResponsavelController
            ViewBag.IsPerfilEdit = true; // Flag para identificar que é edição de perfil
            return View("~/Views/Responsavel/Edit.cshtml", responsavel);
        }

        // POST: Editar perfil
        [HttpPost]
        public IActionResult Editar(Responsavel responsavel)
        {
            if (!IsResponsavel())
            {
                return RedirectToAction("Login", "Auth");
            }

            var responsavelId = GetResponsavelId();

            // Verificar se está editando seus próprios dados
            if (responsavel.Id != responsavelId)
            {
                return Forbid();
            }

            // Buscar dados atuais do banco
            var responsavelAtual = _context.responsavel.Find(responsavelId);
            if (responsavelAtual == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Validar se email já existe (exceto o próprio)
            if (_context.responsavel.Any(r => r.Email == responsavel.Email && r.Id != responsavelId))
            {
                ModelState.AddModelError("Email", "Este email já está em uso");
            }

            // Remove validação de senha se estiver vazia (não está alterando)
            if (string.IsNullOrEmpty(responsavel.Senha))
            {
                ModelState.Remove("Senha");
                ModelState.Remove("ConfirmarSenha");
            }

            if (ModelState.IsValid)
            {
                // Remover máscara do CPF e telefone
                responsavel.Cpf = responsavel.Cpf.Replace(".", "").Replace("-", "");
                responsavel.Telefone = responsavel.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                // Atualizar dados
                responsavelAtual.Nome = responsavel.Nome;
                responsavelAtual.Email = responsavel.Email;
                responsavelAtual.Telefone = responsavel.Telefone;
                responsavelAtual.Endereco = responsavel.Endereco;
                responsavelAtual.Cpf = responsavel.Cpf;

                // Se forneceu nova senha, criptografar
                if (!string.IsNullOrEmpty(responsavel.Senha))
                {
                    responsavelAtual.Senha = PasswordHelper.HashPassword(responsavel.Senha);
                }

                _context.responsavel.Update(responsavelAtual);
                _context.SaveChanges();

                TempData["Sucesso"] = "Perfil atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            // Se houver erro, usar a mesma view
            ViewBag.IsPerfilEdit = true;
            return View("~/Views/Responsavel/Edit.cshtml", responsavel);
        }
    }
}