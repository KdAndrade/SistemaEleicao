using SistemaEleicao.Console;
using Xunit;

namespace SistemaEleicao.Tests;

public class VotacaoTests
{
    [Fact]
    public void DeveContabilizarVotoNoCandidatoEAtualizarEleitor()
    {
        EleicaoManager eleicao = CriarEleicao();

        eleicao.Votar("12345678900", 10);

        Assert.Equal(1, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.True(Assert.Single(eleicao.ListarEleitores()).JaVotou);
        Assert.Equal(0, eleicao.VotosBrancos);
        Assert.Equal(0, eleicao.VotosNulos);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(99)]
    [InlineData(null)]
    public void NaoDevePermitirSegundoVotoNemAlterarContagens(int? segundoNumero)
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.Votar("12345678900", 10);

        Assert.Throws<InvalidOperationException>(() =>
            eleicao.Votar("12345678900", segundoNumero));

        Assert.Equal(1, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.Equal(0, eleicao.VotosBrancos);
        Assert.Equal(0, eleicao.VotosNulos);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveContabilizarNumeroInexistenteComoNulo(int numero)
    {
        EleicaoManager eleicao = CriarEleicao();

        eleicao.Votar("12345678900", numero);

        Assert.Equal(1, eleicao.VotosNulos);
        Assert.Equal(0, eleicao.VotosBrancos);
        Assert.Equal(0, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.True(Assert.Single(eleicao.ListarEleitores()).JaVotou);
        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("12345678900", 10));
        Assert.Equal(1, eleicao.VotosNulos);
    }

    [Fact]
    public void DeveContabilizarVotoBrancoEImpedirOutroVoto()
    {
        EleicaoManager eleicao = CriarEleicao();

        eleicao.Votar("12345678900", null);

        Assert.Equal(1, eleicao.VotosBrancos);
        Assert.Equal(0, eleicao.VotosNulos);
        Assert.Equal(0, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.True(Assert.Single(eleicao.ListarEleitores()).JaVotou);
        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("12345678900", 10));
        Assert.Equal(1, eleicao.VotosBrancos);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(99)]
    [InlineData(null)]
    public void NaoDeveAceitarEleitorNaoCadastrado(int? numero)
    {
        EleicaoManager eleicao = CriarEleicao();

        Assert.Throws<InvalidOperationException>(() => eleicao.Votar("inexistente", numero));

        Assert.Equal(0, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.Equal(0, eleicao.VotosBrancos);
        Assert.Equal(0, eleicao.VotosNulos);
        Assert.False(Assert.Single(eleicao.ListarEleitores()).JaVotou);
    }

    [Fact]
    public void DevePermitirQueEleitoresDiferentesVotemNoMesmoCandidato()
    {
        EleicaoManager eleicao = CriarEleicao();
        eleicao.CadastrarEleitor("98765432100");

        eleicao.Votar("12345678900", 10);
        eleicao.Votar("98765432100", 10);

        Assert.Equal(2, Assert.Single(eleicao.ListarCandidatos()).Votos);
        Assert.All(eleicao.ListarEleitores(), eleitor => Assert.True(eleitor.JaVotou));
    }

    private static EleicaoManager CriarEleicao()
    {
        EleicaoManager eleicao = new();
        eleicao.CadastrarCandidato("Kauan", 10);
        eleicao.CadastrarEleitor("12345678900");
        return eleicao;
    }
}
