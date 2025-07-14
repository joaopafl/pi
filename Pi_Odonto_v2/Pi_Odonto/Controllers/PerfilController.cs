using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pi_Odonto.Data;
using Pi_Odonto.Models;
using Pi_Odonto.ViewModels;
using Pi_Odonto.Helpers;

namespace Pi_Odonto.Controllers
{
    [Authorize] // Remove a policy específica - permite Admin e Responsável
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Se for admin, mostra dashboard diferente
            if (User.HasClaim("TipoUsuario", "Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            // Pega o ID do responsável logado
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            var responsavel = _context.Responsaveis
                .Include(r => r.Criancas)
                .FirstOrDefault(r => r.Id == responsavelId);

            return View(responsavel);
        }

        public IActionResult MinhasCriancas()
        {
            // Debug - verificar claims
            Console.WriteLine("=== CLAIMS DO USUÁRIO ===");
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            Console.WriteLine("========================");

            // Se for admin, mostra TODAS as crianças
            if (User.HasClaim("TipoUsuario", "Admin"))
            {
                Console.WriteLine("Usuário é ADMIN - mostrando todas as crianças");
                var todasCriancas = _context.Criancas
                    .Include(c => c.Responsavel)
                    .ToList();

                Console.WriteLine($"Total de crianças no sistema: {todasCriancas.Count}");
                foreach (var crianca in todasCriancas)
                {
                    Console.WriteLine($"- {crianca.Nome} (Responsável: {crianca.Responsavel?.Nome})");
                }

                return View(todasCriancas);
            }

            // Se for responsável, mostra só as dele
            var responsavelIdClaim = User.FindFirst("ResponsavelId");
            if (responsavelIdClaim == null)
            {
                Console.WriteLine("ERRO: Claim ResponsavelId não encontrada!");
                return RedirectToAction("Login", "Auth");
            }

            var responsavelId = int.Parse(responsavelIdClaim.Value);
            Console.WriteLine($"ID do responsável logado: {responsavelId}");

            var criancas = _context.Criancas
                .Where(c => c.IdResponsavel == responsavelId)
                .ToList();

            Console.WriteLine($"Crianças encontradas: {criancas.Count}");
            foreach (var crianca in criancas)
            {
                Console.WriteLine($"- {crianca.Nome} (ID: {crianca.Id})");
            }

            return View(criancas);
        }

        public IActionResult DetalhesCrianca(int id)
        {
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            var crianca = _context.Criancas
                .FirstOrDefault(c => c.Id == id && c.IdResponsavel == responsavelId);

            if (crianca == null)
                return NotFound();

            return View(crianca);
        }

        [HttpGet]
        public IActionResult EditarCrianca(int id)
        {
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            var crianca = _context.Criancas
                .FirstOrDefault(c => c.Id == id && c.IdResponsavel == responsavelId);

            if (crianca == null)
                return NotFound();

            return View(crianca);
        }

        [HttpPost]
        public IActionResult EditarCrianca(Crianca model)
        {
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            // Verifica se a criança pertence ao responsável logado
            var criancaExistente = _context.Criancas
                .FirstOrDefault(c => c.Id == model.Id && c.IdResponsavel == responsavelId);

            if (criancaExistente == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                criancaExistente.Nome = model.Nome;
                criancaExistente.Cpf = model.Cpf;
                criancaExistente.DataNascimento = model.DataNascimento;
                criancaExistente.Parentesco = model.Parentesco;

                _context.SaveChanges();
                return RedirectToAction("MinhasCriancas");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Editar()
        {
            // Pega o ID do responsável logado
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            var responsavel = _context.Responsaveis
                .Include(r => r.Criancas)
                .FirstOrDefault(r => r.Id == responsavelId);

            if (responsavel == null)
                return NotFound();

            var viewModel = new ResponsavelCriancaViewModel
            {
                Responsavel = responsavel,
                Criancas = responsavel.Criancas?.ToList() ?? new List<Crianca>(),
                OpcoesParentesco = new List<string>
                {
                    "Pai", "Mãe", "Avô", "Avó", "Tio", "Tia", "Padrasto", "Madrasta", "Tutor Legal"
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Editar(ResponsavelCriancaViewModel viewModel)
        {
            var responsavelId = int.Parse(User.FindFirst("ResponsavelId").Value);

            if (responsavelId != viewModel.Responsavel.Id)
                return Forbid();

            var responsavelAtual = _context.Responsaveis.Find(responsavelId);
            if (responsavelAtual == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Atualizar dados do responsável
                responsavelAtual.Nome = viewModel.Responsavel.Nome;
                responsavelAtual.Email = viewModel.Responsavel.Email;
                responsavelAtual.Telefone = viewModel.Responsavel.Telefone;
                responsavelAtual.Endereco = viewModel.Responsavel.Endereco;

                // Se forneceu nova senha
                if (!string.IsNullOrEmpty(viewModel.Responsavel.Senha))
                {
                    responsavelAtual.Senha = PasswordHelper.HashPassword(viewModel.Responsavel.Senha);
                }

                _context.SaveChanges();
                TempData["Sucesso"] = "Perfil atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            // Se deu erro, recarregar opções
            viewModel.OpcoesParentesco = new List<string>
            {
                "Pai", "Mãe", "Avô", "Avó", "Tio", "Tia", "Padrasto", "Madrasta", "Tutor Legal"
            };

            return View(viewModel);
        }
    }
}