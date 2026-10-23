using SistemaEleicao.Console;
using Xunit;

namespace SistemaEleicao.Tests;

public class ApuracaoTests
{
    [Fact]
    public void EleicaoVaziaNaoDeveTerVencedor()
    {
        ResultadoEleicao resultado = new EleicaoManager().ApurarVotos();

        Assert.Empty(resultado.Candidatos);
        Assert.Empty(resultado.Vencedores);
        Assert.Equal(0, resultado.TotalVotos);
    }

    [Fact]
    public void CandidatosSemVotosNaoDevemSerDeclaradosVencedores()
    {
        EleicaoManager eleicao = new();
        eleicao.CadastrarCandidato("Kauan", 10);
        eleicao.CadastrarCandidato("Maria", 20);

        ResultadoEleicao resultado = eleicao.ApurarVotos();

        Assert.Equal(2, resultado.Candidatos.Count);
        Assert.Empty(resultado.Vencedores);
        Assert.Equal(0, resultado.VotosValidos);
    }

    [Fact]
    public void DeveApurarTotaisEIdentificarCandidatoMaisVotado()
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.Votar("eleitor1", 10);
        eleicao.Votar("eleitor2", 10);
        eleicao.Votar("eleitor3", 20);
        eleicao.Votar("eleitor4", null);
        eleicao.Votar("eleitor5", 99);

        ResultadoEleicao resultado = eleicao.ApurarVotos();

        Assert.Equal(2, resultado.Candidatos.Single(c => c.Numero == 10).Votos);
        Assert.Equal(1, resultado.Candidatos.Single(c => c.Numero == 20).Votos);
        Assert.Equal(3, resultado.VotosValidos);
        Assert.Equal(1, resultado.VotosBrancos);
        Assert.Equal(1, resultado.VotosNulos);
        Assert.Equal(5, resultado.TotalVotos);
        Assert.Equal(10, Assert.Single(resultado.Vencedores).Numero);
    }

    [Fact]
    public void DeveInformarTodosOsCandidatosEmpatadosNaLideranca()
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.Votar("eleitor1", 10);
        eleicao.Votar("eleitor2", 20);

        ResultadoEleicao resultado = eleicao.ApurarVotos();

        Assert.Equal(new[] { 10, 20 }, resultado.Vencedores.Select(c => c.Numero));
    }

    [Fact]
    public void ApenasBrancosENulosNaoDevemElegerCandidato()
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.Votar("eleitor1", null);
        eleicao.Votar("eleitor2", 99);

        ResultadoEleicao resultado = eleicao.ApurarVotos();

        Assert.Empty(resultado.Vencedores);
        Assert.Equal(0, resultado.VotosValidos);
        Assert.Equal(2, resultado.TotalVotos);
    }

    [Fact]
    public void ResultadoAnteriorNaoDeveMudarQuandoNovosVotosForemRegistrados()
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.Votar("eleitor1", 10);
        ResultadoEleicao anterior = eleicao.ApurarVotos();

        eleicao.Votar("eleitor2", 20);
        eleicao.Votar("eleitor3", null);
        eleicao.Votar("eleitor4", 99);

        Assert.Equal(1, anterior.TotalVotos);
        Assert.Equal(0, anterior.Candidatos.Single(c => c.Numero == 20).Votos);
        Assert.Equal(10, Assert.Single(anterior.Vencedores).Numero);
        Assert.Equal(4, eleicao.ApurarVotos().TotalVotos);
    }

    private static EleicaoManager CriarEleicao()
    {
        EleicaoManager eleicao = new();
        eleicao.CadastrarCandidato("Kauan", 10);
        eleicao.CadastrarCandidato("Maria", 20);
        for (int i = 1; i <= 5; i++)
        {
            eleicao.CadastrarEleitor($"eleitor{i}");
        }
        return eleicao;
    }
}
