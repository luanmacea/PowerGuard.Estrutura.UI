using PowerGuard.Estrutura.Controller;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;
using PowerGuard.Estrutura.UI.Utils;

Usuario? usuarioLogado = null;

while (usuarioLogado == null)
{
    try
    {
        Console.WriteLine("=== LOGIN ===");
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Senha: ");
        string senha = Console.ReadLine() ?? "";

        var repo = new UsuarioRepository();
        usuarioLogado = repo.ValidarLogin(email, senha);

        if (usuarioLogado == null)
            Console.WriteLine("Credenciais inválidas. Tente novamente.\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Erro] Falha no login: {ex.Message}\n");
    }
}

Console.WriteLine($"\nBem-vindo, {usuarioLogado.Nome}!\n");

Console.WriteLine("=== PowerGuard - Monitoramento de Falhas ===\n");

bool continuar = true;

while (continuar)
{
    try
    {
        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Cadastrar Falha de Energia");
        Console.WriteLine("2 - Listar Falhas de Energia");
        Console.WriteLine("3 - Emitir Alerta");
        Console.WriteLine("4 - Listar Alertas");
        Console.WriteLine("5 - Registrar Simulação");
        Console.WriteLine("6 - Listar Simulações");
        Console.WriteLine("7 - Listar Logs de Eventos");
        Console.WriteLine("0 - Sair");

        Console.Write("\nOpção: ");
        string? opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                CadastrarFalha();
                break;
            case "2":
                ListarFalhas();
                break;
            case "3":
                EmitirAlerta();
                break;
            case "4":
                ListarAlertas();
                break;
            case "5":
                RegistrarSimulacao();
                break;
            case "6":
                ListarSimulacoes();
                break;
            case "7":
                ListarLogs();
                break;

            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida.\n");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[ERRO] {ex.Message}\n");
    }
}

Console.WriteLine("Encerrando o sistema...");

void CadastrarFalha()
{
    var logController = new LogEventoController();
    var controller = new FalhaEnergiaController();
    var falha = new FalhaEnergia();

    Console.Write("Localização (ex: 'São Paulo - Zona Sul'): ");
    falha.Localizacao = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(falha.Localizacao))
        throw new Exception("O campo 'Localização' é obrigatório.");

    Console.Write("Causa provável (ex: 'Queda de árvore') (opcional): ");
    falha.CausaProvavel = Console.ReadLine() ?? "";

    Console.Write("Tipo de evento (ex: 'Tempestade') (opcional): ");
    falha.TipoEvento = Console.ReadLine() ?? "";

    Console.Write("Gravidade (Alta/Média/Baixa): ");
    falha.Gravidade = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(falha.Gravidade))
        throw new Exception("O campo 'Gravidade' é obrigatório.");

    falha.DataHora = DateTime.Now;

    controller.Cadastrar(falha);
    Console.WriteLine("Falha cadastrada com sucesso!\n");
    logController.Registrar(new LogEvento
    {
        Acao = "Falha de energia cadastrada",
        DataHora = DateTime.Now,
        UsuarioId = usuarioLogado.Id
    });

}


void ListarFalhas()
{
    var controller = new FalhaEnergiaController();
    var falhas = controller.Listar();

    Console.WriteLine("\n=== Falhas Registradas ===");
    foreach (var f in falhas)
    {
        Console.WriteLine($"ID: {f.Id} | Local: {f.Localizacao} | Evento: {f.TipoEvento} | Gravidade: {f.Gravidade} | Data: {f.DataHora}");
    }
    Console.WriteLine();
}

void EmitirAlerta()
{
    var controller = new AlertaController();
    var alerta = new Alerta();

    Console.Write("Mensagem (ex: 'Falha registrada em SP'): ");
    alerta.Mensagem = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(alerta.Mensagem))
        throw new Exception("O campo 'Mensagem' é obrigatório.");

    Console.Write("Informe o email ou nome do usuário responsável: ");
    string inputUsuario = Console.ReadLine() ?? "";
    Usuario usuarioAlvo = BancoUtils.BuscarUsuarioPorNomeOuEmail(inputUsuario);
    alerta.UsuarioId = usuarioAlvo.Id;

    var falhas = new FalhaEnergiaRepository().ListarTodos().OrderBy(f => f.Id).ToList();
    Console.WriteLine("\nFalhas registradas disponíveis:");
    foreach (var f in falhas)
    {
        Console.WriteLine($"ID: {f.Id} | Local: {f.Localizacao} | Tipo: {f.TipoEvento} | Gravidade: {f.Gravidade}");
    }

    Console.Write("\nInforme o ID da falha associada (ou deixe vazio para ignorar): ");
    string? inputFalha = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(inputFalha))
    {
        if (!int.TryParse(inputFalha, out int idFalha))
            throw new Exception("ID de falha inválido.");

        var falhaEncontrada = BancoUtils.BuscarFalhaPorId(idFalha);
        alerta.FalhaEnergiaId = falhaEncontrada.Id;
    }

    alerta.DataHora = DateTime.Now;

    controller.Emitir(alerta);
    Console.WriteLine("✅ Alerta emitido com sucesso!\n");

    var logController = new LogEventoController();
    logController.Registrar(new LogEvento
    {
        Acao = "Alerta emitido",
        DataHora = DateTime.Now,
        UsuarioId = usuarioLogado.Id
    });
}






void ListarAlertas()
{
    var alertaController = new AlertaController();
    var usuarioRepo = new UsuarioRepository();
    var falhaRepo = new FalhaEnergiaRepository();

    var alertas = alertaController.Listar();
    var usuarios = usuarioRepo.ListarTodos();
    var falhas = falhaRepo.ListarTodos();

    Console.WriteLine("\n=== Alertas Emitidos ===");

    foreach (var alerta in alertas)
    {
        string nomeUsuario = usuarios
            .FirstOrDefault(u => u.Id == alerta.UsuarioId)?.Nome ?? "Desconhecido";

        string infoFalha = "Nenhuma";
        if (alerta.FalhaEnergiaId.HasValue)
        {
            var falha = falhas.FirstOrDefault(f => f.Id == alerta.FalhaEnergiaId.Value);
            if (falha != null)
                infoFalha = $"{falha.Localizacao} - {falha.TipoEvento}";
        }

        Console.WriteLine($"ID: {alerta.Id} | Usuário: {nomeUsuario} | Falha: {infoFalha} | Mensagem: {alerta.Mensagem} | Data: {alerta.DataHora}");
    }

    Console.WriteLine();
}


void RegistrarSimulacao()
{
    var logController = new LogEventoController();
    var controller = new SimulacaoController();
    var sim = new Simulacao();

    Console.Write("Tipo de Simulação (ex: 'Blackout geral'): ");
    sim.TipoSimulacao = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(sim.TipoSimulacao))
        throw new Exception("O campo 'Tipo de Simulação' é obrigatório.");

    Console.Write("Descrição (ex: 'Simulação de falha total no centro') (opcional): ");
    sim.Descricao = Console.ReadLine() ?? "";

    sim.DataHora = DateTime.Now;
    sim.Concluida = false;

    controller.IniciarSimulacao(sim);
    Console.WriteLine("Simulação registrada com sucesso!\n");
    logController.Registrar(new LogEvento
    {
        Acao = "Simulação registrada",
        DataHora = DateTime.Now,
        UsuarioId = usuarioLogado.Id
    });

}


void ListarSimulacoes()
{
    var controller = new SimulacaoController();
    var sims = controller.Listar();

    Console.WriteLine("\n=== Simulações ===");
    foreach (var s in sims)
    {
        Console.WriteLine($"ID: {s.Id} | Tipo: {s.TipoSimulacao} | Concluída: {(s.Concluida ? "Sim" : "Não")} | Data: {s.DataHora}");
    }
    Console.WriteLine();
}
void ListarLogs()
{
    var logController = new LogEventoController();
    var usuarioRepo = new UsuarioRepository();

    var logs = logController.Listar().OrderBy(l => l.Id).ToList();
    var usuarios = usuarioRepo.ListarTodos();

    Console.WriteLine("\n=== LOGS DO SISTEMA ===");

    foreach (var log in logs)
    {
        string nomeUsuario = usuarios
            .FirstOrDefault(u => u.Id == log.UsuarioId)?.Nome ?? "Desconhecido";

        Console.WriteLine($"ID: {log.Id} | Usuário: {nomeUsuario} | Ação: {log.Acao} | Data: {log.DataHora}");
    }

    Console.WriteLine();
}

