using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaEleicao.Console;
using Xunit;

namespace SistemaEleicao.Tests;

public class CandidatoTests
{
    [Fact]
    public void DeveCadastrarCandidato()
    {
        // Arrange
        EleicaoManager eleicao = new();

        // Act
        eleicao.CadastrarCandidato("Kauan", 10);

        // Assert
        Assert.Single(eleicao.ListarCandidatos());
    }

    [Fact]
    public void NaoDeveCadastrarCandidatosComMesmoNumero()
    {
        // Arrange
        EleicaoManager eleicao = new();
        eleicao.CadastrarCandidato("Kauan", 10);

        // Act e Assert
        Assert.Throws<InvalidOperationException>(() =>
            eleicao.CadastrarCandidato("João", 10)
        );
    }

    [Fact]
    public void NaoDeveCadastrarCandidatoSemNome()
    {
        EleicaoManager eleicao = new();

        Assert.Throws<ArgumentException>(() =>
            eleicao.CadastrarCandidato("", 20)
        );
    }

    [Fact]
    public void NaoDeveCadastrarCandidatoComNumeroNegativo()
    {
        EleicaoManager eleicao = new();

        Assert.Throws<ArgumentException>(() =>
            eleicao.CadastrarCandidato("Maria", -1)
        );
    }
}