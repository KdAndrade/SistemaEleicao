using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaEleicao.Console;

public class EleicaoManager
{
    private readonly List<Candidato> candidatos = new();

    public void CadastrarCandidato(string nome, int numero)
    {
        bool numeroExiste = candidatos.Any(c => c.Numero == numero);

        if (numeroExiste)
        {
            throw new InvalidOperationException(
                "Já existe um candidato cadastrado com esse número."
            );
        }

        Candidato candidato = new Candidato(nome, numero);

        candidatos.Add(candidato);
    }

    public IReadOnlyList<Candidato> ListarCandidatos()
    {
        return candidatos.AsReadOnly();
    }

    private readonly List<Eleitor> eleitores = new();

    public void CadastrarEleitor(string documento)
    {
        bool eleitorExiste = eleitores.Any(e => e.Documento == documento);

        if (eleitorExiste)
        {
            throw new InvalidOperationException(
                "Já existe um eleitor com esse documento."
            );
        }

        Eleitor eleitor = new Eleitor(documento);
        eleitores.Add(eleitor);
    }

    public IReadOnlyList<Eleitor> ListarEleitores()
    {
        return eleitores.AsReadOnly();
    }

    public int VotosBrancos { get; private set; }
    public int VotosNulos { get; private set; }

    public void Votar(string documento, int? numeroCandidato)
    {
        Eleitor? eleitor = eleitores.SingleOrDefault(e => e.Documento == documento);

        if (eleitor is null)
        {
            throw new InvalidOperationException("Eleitor não cadastrado.");
        }

        if (eleitor.JaVotou)
        {
            throw new InvalidOperationException("Este eleitor já votou.");
        }

        if (numeroCandidato is null)
        {
            VotosBrancos++;
        }
        else
        {
            Candidato? candidato = candidatos.SingleOrDefault(c => c.Numero == numeroCandidato);

            if (candidato is null)
            {
                VotosNulos++;
            }
            else
            {
                candidato.AdicionarVoto();
            }
        }

        eleitor.RegistrarVoto();
    }
}
