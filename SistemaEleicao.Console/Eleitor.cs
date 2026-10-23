namespace SistemaEleicao.Console;

public class Eleitor
{
    public string Documento { get; }
    public bool JaVotou { get; private set; }

    public Eleitor(string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
        {
            throw new ArgumentException("O documento é obrigatório.");
        }

        Documento = documento;
        JaVotou = false;
    }

    public void RegistrarVoto()
    {
        if (JaVotou)
        {
            throw new InvalidOperationException("Este eleitor já votou.");
        }

        JaVotou = true;
    }
}
