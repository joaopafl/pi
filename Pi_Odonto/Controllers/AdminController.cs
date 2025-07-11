using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pi_Odonto.Data;
using Pi_Odonto.Models;
using Pi_Odonto.Helpers;

namespace Pi_Odonto.Controllers
{
    [Authorize]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Verificar se é admin
        private bool IsAdmin()
        {
            return User.HasClaim("TipoUsuario", "Admin");
        }

        // GET: Dashboard Admin
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            // Estatísticas para o dashboard
            var totalResponsaveis = _context.responsavel.Count();
            var responsaveisAtivos = _context.responsavel.Count(r => r.Ativo);
            var cadastrosHoje = _context.responsavel.Count(r => r.DataCadastro.Date == DateTime.Today);
            var cadastrosEsteMes = _context.responsavel.Count(r => r.DataCadastro.Month == DateTime.Now.Month && r.DataCadastro.Year == DateTime.Now.Year);

            ViewBag.TotalResponsaveis = totalResponsaveis;
            ViewBag.ResponsaveisAtivos = responsaveisAtivos;
            ViewBag.CadastrosHoje = cadastrosHoje;
            ViewBag.CadastrosEsteMes = cadastrosEsteMes;

            // Últimos cadastros
            var ultimosCadastros = _context.responsavel
                .OrderByDescending(r => r.DataCadastro)
                .Take(5)
                .ToList();

            return View(ultimosCadastros);
        }

        // GET: Lista de Responsáveis (Admin)
        [Route("Responsaveis")]
        public IActionResult Responsaveis(string busca = "", bool? ativo = null)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var query = _context.responsavel.AsQueryable();

            // Filtro de busca
            if (!string.IsNullOrEmpty(busca))
            {
                query = query.Where(r =>
                    r.Nome.Contains(busca) ||
                    r.Email.Contains(busca) ||
                    r.Cpf.Contains(busca) ||
                    r.Telefone.Contains(busca));
            }

            // Filtro de status
            if (ativo.HasValue)
            {
                query = query.Where(r => r.Ativo == ativo.Value);
            }

            var responsaveis = query
                .OrderByDescending(r => r.DataCadastro)
                .ToList();

            ViewBag.Busca = busca;
            ViewBag.Ativo = ativo;

            return View(responsaveis);
        }

        // GET: Visualizar Responsável
        [Route("Responsaveis/Detalhes/{id}")]
        public IActionResult DetalhesResponsavel(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            return View(responsavel);
        }

        // GET: Editar Responsável (Admin)
        [Route("Responsaveis/Editar/{id}")]
        public IActionResult EditarResponsavel(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            // Limpar senha para não mostrar
            responsavel.Senha = "";
            return View(responsavel);
        }

        // POST: Editar Responsável (Admin)
        [HttpPost]
        [Route("Responsaveis/Editar/{id}")]
        public IActionResult EditarResponsavel(int id, Responsavel responsavel)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            if (id != responsavel.Id)
            {
                return BadRequest();
            }

            var responsavelAtual = _context.responsavel.Find(id);
            if (responsavelAtual == null)
            {
                return NotFound();
            }

            // Validar email único
            if (_context.responsavel.Any(r => r.Email == responsavel.Email && r.Id != id))
            {
                ModelState.AddModelError("Email", "Este email já está em uso");
            }

            // Remove validação de senha se estiver vazia
            if (string.IsNullOrEmpty(responsavel.Senha))
            {
                ModelState.Remove("Senha");
                ModelState.Remove("ConfirmarSenha");
            }

            if (ModelState.IsValid)
            {
                // Remover máscara
                responsavel.Cpf = responsavel.Cpf.Replace(".", "").Replace("-", "");
                responsavel.Telefone = responsavel.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                // Atualizar dados
                responsavelAtual.Nome = responsavel.Nome;
                responsavelAtual.Email = responsavel.Email;
                responsavelAtual.Telefone = responsavel.Telefone;
                responsavelAtual.Endereco = responsavel.Endereco;
                responsavelAtual.Cpf = responsavel.Cpf;
                responsavelAtual.Ativo = responsavel.Ativo;

                // Se forneceu nova senha
                if (!string.IsNullOrEmpty(responsavel.Senha))
                {
                    responsavelAtual.Senha = PasswordHelper.HashPassword(responsavel.Senha);
                }

                _context.responsavel.Update(responsavelAtual);
                _context.SaveChanges();

                TempData["Sucesso"] = "Responsável atualizado com sucesso!";
                return RedirectToAction("Responsaveis");
            }

            return View(responsavel);
        }

        // POST: Desativar/Ativar Responsável
        [HttpPost]
        [Route("Responsaveis/ToggleStatus/{id}")]
        public IActionResult ToggleStatus(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            responsavel.Ativo = !responsavel.Ativo;
            _context.responsavel.Update(responsavel);
            _context.SaveChanges();

            TempData["Sucesso"] = $"Responsável {(responsavel.Ativo ? "ativado" : "desativado")} com sucesso!";
            return RedirectToAction("Responsaveis");
        }

        // POST: Excluir Responsável (Admin)
        [HttpPost]
        [Route("Responsaveis/Excluir/{id}")]
        public IActionResult ExcluirResponsavel(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null)
            {
                return NotFound();
            }

            _context.responsavel.Remove(responsavel);
            _context.SaveChanges();

            TempData["Sucesso"] = "Responsável excluído com sucesso!";
            return RedirectToAction("Responsaveis");
        }

        // GET: Relatórios
        [Route("Relatorios")]
        public IActionResult Relatorios()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            // Dados para relatórios
            var totalResponsaveis = _context.responsavel.Count();
            var responsaveisAtivos = _context.responsavel.Count(r => r.Ativo);
            var responsaveisInativos = _context.responsavel.Count(r => !r.Ativo);

            // Cadastros por mês (últimos 6 meses)
            var cadastrosPorMes = new List<object>();
            for (int i = 5; i >= 0; i--)
            {
                var data = DateTime.Now.AddMonths(-i);
                var count = _context.responsavel.Count(r =>
                    r.DataCadastro.Month == data.Month &&
                    r.DataCadastro.Year == data.Year);

                cadastrosPorMes.Add(new
                {
                    Mes = data.ToString("MMM/yyyy"),
                    Count = count
                });
            }

            ViewBag.TotalResponsaveis = totalResponsaveis;
            ViewBag.ResponsaveisAtivos = responsaveisAtivos;
            ViewBag.ResponsaveisInativos = responsaveisInativos;
            ViewBag.CadastrosPorMes = cadastrosPorMes;

            return View();
        }
    }
}