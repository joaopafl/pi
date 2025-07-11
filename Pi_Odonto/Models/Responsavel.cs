using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pi_Odonto.Models
{
    [Table("responsavel")]
    public class Responsavel
    {
        [Key]
        [Column("id_resp")]
        public int Id { get; set; }

        [Column("nome_resp")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Column("cpf_resp")]
        [Required(ErrorMessage = "CPF é obrigatório")]
        public string Cpf { get; set; } = string.Empty;

        [Column("tel_resp")]
        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Telefone { get; set; } = string.Empty;

        [Column("email_resp")]
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Column("endereco_resp")]
        [Required(ErrorMessage = "Endereço é obrigatório")]
        public string Endereco { get; set; } = string.Empty;

        [Column("senha_resp")]
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        // Propriedade que não vai para o banco - só para validação
        [NotMapped]
        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "Senhas não conferem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }

    // Model separado para login
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;

        public bool LembrarMe { get; set; }
    }

    // Model para admin
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = string.Empty;
    }
}