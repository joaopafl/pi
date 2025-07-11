using Pi_Odonto.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pi_Odonto.Data;
using Pi_Odonto.Models;
using System;
using System.Linq;

namespace Pi_Odonto.Controllers
{
    public class ResponsavelController : Controller
    {
        private readonly AppDbContext _context;

        public ResponsavelController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.responsavel.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Responsavel responsavel)
        {
            if (ModelState.IsValid)
            {
                _context.responsavel.Add(responsavel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsavel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null) return NotFound();
            return View(responsavel);
        }

        [HttpPost]
        public IActionResult Edit(Responsavel responsavel)
        {
            if (ModelState.IsValid)
            {
                _context.responsavel.Update(responsavel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(responsavel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var responsavel = _context.responsavel.Find(id);
            if (responsavel == null) return NotFound();
            return View(responsavel);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var responsavel = _context.responsavel.Find(id);
            if (responsavel != null)
            {
                _context.responsavel.Remove(responsavel);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // === ÁREA PÚBLICA DE CADASTRO ===

        [HttpGet]
        [Route("Cadastro")]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [Route("Cadastro")]
        public IActionResult Cadastro(Responsavel responsavel)
        {
            if (ModelState.IsValid)
            {
                // Remove máscara do CPF e telefone antes de salvar
                responsavel.Cpf = responsavel.Cpf.Replace(".", "").Replace("-", "");
                responsavel.Telefone = responsavel.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                // Validação adicional de CPF
                if (!IsValidCPF(responsavel.Cpf))
                {
                    ModelState.AddModelError("Cpf", "CPF inválido");
                    return View(responsavel);
                }

                // Verificar se CPF já existe
                if (_context.responsavel.Any(r => r.Cpf == responsavel.Cpf))
                {
                    ModelState.AddModelError("Cpf", "CPF já cadastrado no sistema");
                    return View(responsavel);
                }

                // Verificar se email já existe
                if (_context.responsavel.Any(r => r.Email == responsavel.Email))
                {
                    ModelState.AddModelError("Email", "Email já cadastrado no sistema");
                    return View(responsavel);
                }

                // Criptografar a senha antes de salvar
                responsavel.Senha = PasswordHelper.HashPassword(responsavel.Senha);
                responsavel.DataCadastro = DateTime.Now;
                responsavel.Ativo = true;

                _context.responsavel.Add(responsavel);
                _context.SaveChanges();
                return RedirectToAction("Sucesso");
            }
            return View(responsavel);
        }

        [HttpGet]
        [Route("Cadastro/Sucesso")]
        public IActionResult Sucesso()
        {
            return View();
        }

        // === MÉTODO AUXILIAR ===

        private bool IsValidCPF(string cpf)
        {
            // Remove caracteres não numéricos
            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return int.Parse(cpf[10].ToString()) == digito2;
        }
    }
}