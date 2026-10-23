namespace SistemaEleicao.Console;

public class ResultadoEleicao
{
    public IReadOnlyList<ResultadoCandidato> Candidatos { get; }
    public IReadOnlyList<ResultadoCandidato> Vencedores { get; }
    public int VotosBrancos { get; }
    public int VotosNulos { get; }
    public int VotosValidos => Candidatos.Sum(c => c.Votos);
    public int TotalVotos => VotosValidos + VotosBrancos + VotosNulos;

    internal ResultadoEleicao(IEnumerable<Candidato> candidatos, int votosBrancos, int votosNulos)
    {
        Candidatos = candidatos
            .Select(c => new ResultadoCandidato(c.Nome, c.Numero, c.Votos))
            .OrderBy(c => c.Numero)
            .ToList()
            .AsReadOnly();
        VotosBrancos = votosBrancos;
        VotosNulos = votosNulos;

        int maiorVotacao = Candidatos.Select(c => c.Votos).DefaultIfEmpty(0).Max();
        Vencedores = Candidatos
            .Where(c => c.Votos > 0 && c.Votos == maiorVotacao)
            .ToList()
            .AsReadOnly();
    }
}
