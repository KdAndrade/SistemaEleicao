namespace SistemaEleicao.Console;

public class Candidato
{
    public string Nome { get; }
    public int Numero { get; }
    public int Votos { get; private set; }

    public Candidato(string nome, int numero)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do candidato é obrigatório.");

        if (numero <= 0)
            throw new ArgumentException("O número do candidato deve ser positivo.");

        Nome = nome;
        Numero = numero;
        Votos = 0;
    }

    public void AdicionarVoto()
    {
        Votos++;
    }
}
