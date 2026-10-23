using SistemaEleicao.Console;
using Terminal = System.Console;

EleicaoManager eleicao = new();
Terminal.OutputEncoding = System.Text.Encoding.UTF8;

while (true)
{
    Terminal.WriteLine("\n=== Sistema de Eleição ===");
    Terminal.WriteLine("1 - Cadastrar candidato");
    Terminal.WriteLine("2 - Cadastrar eleitor");
    Terminal.WriteLine("3 - Listar candidatos");
    Terminal.WriteLine("4 - Votar");
    Terminal.WriteLine("5 - Apurar votos");
    Terminal.WriteLine("0 - Sair");
    Terminal.Write("Opção: ");
    string? opcao = Terminal.ReadLine();

    if (opcao is null || opcao.Trim() == "0")
    {
        Terminal.WriteLine("Sistema encerrado. Os dados desta sessão não serão salvos.");
        break;
    }

    try
    {
        switch (opcao.Trim())
        {
            case "1":
                string nome = LerTexto("Nome do candidato: ");
                string entradaNumero = LerTexto("Número do candidato: ");
                if (!int.TryParse(entradaNumero, out int numero))
                {
                    Terminal.WriteLine("Digite um número inteiro para o candidato.");
                    break;
                }
                eleicao.CadastrarCandidato(nome, numero);
                Terminal.WriteLine("Candidato cadastrado com sucesso.");
                break;
            case "2":
                eleicao.CadastrarEleitor(LerTexto("Título/CPF simulado: "));
                Terminal.WriteLine("Eleitor cadastrado com sucesso.");
                break;
            case "3":
                ListarCandidatos(eleicao);
                break;
            case "4":
                RegistrarVoto(eleicao);
                break;
            case "5":
                ExibirApuracao(eleicao.ApurarVotos());
                break;
            default:
                Terminal.WriteLine("Opção inválida. Escolha uma opção do menu.");
                break;
        }
    }
    catch (ArgumentException erro)
    {
        Terminal.WriteLine($"Erro: {erro.Message}");
    }
    catch (InvalidOperationException erro)
    {
        Terminal.WriteLine($"Erro: {erro.Message}");
    }
    catch (EndOfStreamException)
    {
        Terminal.WriteLine("\nEntrada encerrada. Os dados desta sessão não serão salvos.");
        break;
    }
}

static string LerTexto(string mensagem)
{
    Terminal.Write(mensagem);
    string? entrada = Terminal.ReadLine();
    if (entrada is null)
    {
        throw new EndOfStreamException();
    }
    return entrada.Trim();
}

static void ListarCandidatos(EleicaoManager eleicao)
{
    if (eleicao.ListarCandidatos().Count == 0)
    {
        Terminal.WriteLine("Nenhum candidato cadastrado.");
        return;
    }

    foreach (Candidato candidato in eleicao.ListarCandidatos().OrderBy(c => c.Numero))
    {
        Terminal.WriteLine($"{candidato.Numero} - {candidato.Nome}");
    }
}

static void RegistrarVoto(EleicaoManager eleicao)
{
    string documento = LerTexto("Título/CPF simulado do eleitor: ");
    ListarCandidatos(eleicao);
    string entrada = LerTexto("Número do candidato, B para branco ou N para nulo: ");
    int? numero;
    string tipo;

    if (entrada.Equals("B", StringComparison.OrdinalIgnoreCase))
    {
        numero = null;
        tipo = "branco";
    }
    else if (entrada.Equals("N", StringComparison.OrdinalIgnoreCase))
    {
        numero = -1;
        tipo = "nulo";
    }
    else if (int.TryParse(entrada, out int numeroDigitado))
    {
        numero = numeroDigitado;
        tipo = eleicao.ListarCandidatos().Any(c => c.Numero == numeroDigitado)
            ? "no candidato escolhido"
            : "nulo (número de candidato inexistente)";
    }
    else
    {
        Terminal.WriteLine("Entrada inválida. Use um número inteiro, B ou N. Nenhum voto foi registrado.");
        return;
    }

    eleicao.Votar(documento, numero);
    Terminal.WriteLine($"Voto {tipo} registrado com sucesso.");
}

static void ExibirApuracao(ResultadoEleicao resultado)
{
    Terminal.WriteLine("\n=== Apuração de votos ===");
    if (resultado.Candidatos.Count == 0)
    {
        Terminal.WriteLine("Nenhum candidato cadastrado.");
    }
    foreach (ResultadoCandidato candidato in resultado.Candidatos)
    {
        Terminal.WriteLine($"{candidato.Numero} - {candidato.Nome}: {candidato.Votos} voto(s)");
    }
    Terminal.WriteLine($"Votos válidos: {resultado.VotosValidos}");
    Terminal.WriteLine($"Votos brancos: {resultado.VotosBrancos}");
    Terminal.WriteLine($"Votos nulos: {resultado.VotosNulos}");
    Terminal.WriteLine($"Total de votos: {resultado.TotalVotos}");

    if (resultado.Vencedores.Count == 0)
    {
        Terminal.WriteLine("Sem vencedor: nenhum voto válido registrado.");
    }
    else if (resultado.Vencedores.Count == 1)
    {
        ResultadoCandidato vencedor = resultado.Vencedores[0];
        Terminal.WriteLine($"Vencedor: {vencedor.Nome} (número {vencedor.Numero}), com {vencedor.Votos} voto(s).");
    }
    else
    {
        string nomes = string.Join(", ", resultado.Vencedores.Select(c => $"{c.Nome} ({c.Numero})"));
        Terminal.WriteLine($"Empate entre: {nomes}, com {resultado.Vencedores[0].Votos} voto(s) cada.");
    }
}
