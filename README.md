# Sistema de Eleição

Aplicação de console desenvolvida em C# para a atividade de Garantia da Qualidade de Software. Permite cadastrar candidatos e eleitores, votar e consultar a apuração, com testes unitários em xUnit e histórico em Conventional Commits.

## Requisitos

- .NET SDK 10.0, pois os dois projetos utilizam `net10.0`.
- Git para consultar o histórico e publicar o repositório.
- Acesso à internet na primeira restauração das dependências do xUnit.

Confira a instalação:

```sh
dotnet --version
```

## Como executar

Abra um terminal na pasta `SistemaEleicao`, que contém `SistemaEleicao.slnx`:

```sh
dotnet restore SistemaEleicao.slnx
dotnet run --project SistemaEleicao.Console/SistemaEleicao.Console.csproj
```

O arquivo `.slnx` é a solução usada neste projeto.

## Menu

| Opção | Ação |
| --- | --- |
| 1 | Cadastrar candidato por nome e número inteiro positivo |
| 2 | Cadastrar eleitor por Título/CPF simulado |
| 3 | Listar os candidatos cadastrados |
| 4 | Registrar voto de um eleitor cadastrado |
| 5 | Exibir a apuração dos votos registrados até o momento |
| 0 | Encerrar o programa |

### Exemplo de uso

1. Escolha `1` e cadastre `Kauan` com o número `10`.
2. Escolha `2` e cadastre o documento simulado `12345678900`.
3. Escolha `4`, informe o documento e digite `10`.
4. Escolha `5`: o candidato terá um voto e aparecerá como vencedor.
5. Uma nova tentativa de votação com o mesmo documento será recusada.

Na votação, digite um número inteiro de candidato, `B` para branco ou `N` para nulo. Números sem candidato cadastrado também contam como nulos. Letras diferentes de `B` e `N` são recusadas sem registrar voto, permitindo corrigir a entrada.

## Regras de negócio

- Nome de candidato e documento de eleitor são obrigatórios.
- Números de candidatos devem ser positivos e não podem se repetir.
- Documentos de eleitores não podem se repetir.
- O documento é simulado: não há verificação de quantidade de dígitos, formato ou dígitos verificadores de CPF/Título.
- Cada eleitor cadastrado pode registrar somente um voto, inclusive branco ou nulo.
- No gerenciador, `Votar(documento, numero)` registra um voto; `null` representa branco e um número inexistente representa nulo. O menu usa `-1` para a opção explícita de voto nulo, pois candidatos só aceitam números positivos.
- Eleitor inexistente ou que já votou provoca erro sem alterar as contagens.
- O estado `JaVotou` é atualizado depois da contabilização do voto.
- A apuração mostra votos por candidato, válidos, brancos, nulos e total geral.
- O vencedor é o candidato com mais votos válidos. Empates são exibidos com todos os candidatos na liderança, sem desempate automático. Sem votos válidos, não há vencedor.
- A apuração é uma consulta e não encerra a eleição. É possível continuar cadastrando e votando durante a sessão.
- Os dados ficam somente em memória e são perdidos ao encerrar o programa. Não há banco de dados nem integração com sistemas eleitorais reais.

## Testes

Execute na pasta da solução:

```sh
dotnet test
```

A suíte contém 26 casos de teste cobrindo cadastro de candidatos e eleitores, documentos duplicados, estado de votação, eleitor inexistente, voto duplicado, votos válidos/brancos/nulos, totais, vencedor, empate e resultados anteriores à entrada de novos votos.

Caso o ambiente restrinja os processos paralelos de compilação, use:

```sh
dotnet test SistemaEleicao.slnx -m:1 -p:UseSharedCompilation=false
```

O executor de testes precisa permitir a comunicação local entre seus processos. Uma falha de conexão com `testhost` deve ser investigada como restrição do ambiente antes de ser interpretada como falha de uma regra de negócio.

## Organização

```text
SistemaEleicao.slnx
SistemaEleicao.Console/
  Candidato.cs
  Eleitor.cs
  EleicaoManager.cs
  ResultadoCandidato.cs
  ResultadoEleicao.cs
  Program.cs
SistemaEleicao.Tests/
  CandidatoTests.cs
  EleitorTests.cs
  VotacaoTests.cs
  ApuracaoTests.cs
```

`EleicaoManager` coordena cadastros, votos e apuração. `ResultadoEleicao` guarda uma cópia dos resultados no momento da consulta, sem mudar quando novos votos forem registrados. `Program.cs` contém o menu e o tratamento das entradas do terminal.

As pastas `bin`, `obj` e `TestResults` são geradas pelas ferramentas e ignoradas pelo Git.

## Histórico e entrega

Os commits seguem Conventional Commits, com os tipos `feat`, `test`, `refactor`, `docs` e `chore`. Para consultar:

```sh
git log --oneline
git status
```

Repositório: [KdAndrade/SistemaEleicao](https://github.com/KdAndrade/SistemaEleicao).

Para obter uma cópia em outro computador:

```sh
git clone https://github.com/KdAndrade/SistemaEleicao.git
cd SistemaEleicao
```

Para enviar novos commits da branch `main`:

```sh
git push origin main
```

## Licença

Este projeto utiliza a licença MIT, disponível no arquivo `LICENSE`.
