using System.ComponentModel.DataAnnotations;

namespace Projeto_MVC_Produtos.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Tipo { get; set; }
        
    } 
}
