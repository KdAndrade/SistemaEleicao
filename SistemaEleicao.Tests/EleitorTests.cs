using SistemaEleicao.Console;
using Xunit;

namespace SistemaEleicao.Tests;

public class EleitorTests
{
    [Fact]
    public void DeveCadastrarEleitor()
    {
        EleicaoManager eleicao = new();

        eleicao.CadastrarEleitor("12345678900");

        Eleitor eleitor = Assert.Single(eleicao.ListarEleitores());
        Assert.Equal("12345678900", eleitor.Documento);
    }

    [Fact]
    public void NaoDeveCadastrarEleitorDuplicado()
    {
        EleicaoManager eleicao = new();
        eleicao.CadastrarEleitor("12345678900");

        Assert.Throws<InvalidOperationException>(() =>
            eleicao.CadastrarEleitor("12345678900")
        );

        Assert.Single(eleicao.ListarEleitores());
    }

    [Fact]
    public void EleitorDeveIniciarSemTerVotado()
    {
        Eleitor eleitor = new("12345678900");

        Assert.False(eleitor.JaVotou);
    }

    [Fact]
    public void NaoDevePermitirVotarDuasVezes()
    {
        Eleitor eleitor = new("12345678900");
        eleitor.RegistrarVoto();

        Assert.True(eleitor.JaVotou);
        Assert.Throws<InvalidOperationException>(() =>
            eleitor.RegistrarVoto()
        );
        Assert.True(eleitor.JaVotou);
    }
}
