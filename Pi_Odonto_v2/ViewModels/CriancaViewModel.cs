// ViewModels/CriancaViewModel.cs
public class CriancaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Parentesco { get; set; }
    public int IdResponsavel { get; set; } // FK para associar

    public List<string> OpcoesParentesco { get; set; } = new List<string>
    {
        "Pai", "Mãe", "Avô", "Avó", "Tio", "Tia", "Padrasto", "Madrasta", "Tutor Legal"
    };
}