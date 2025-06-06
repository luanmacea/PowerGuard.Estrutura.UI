using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.UI.Utils
{
    public static class BancoUtils
    {
        public static Usuario BuscarUsuarioPorNomeOuEmail(string input)
        {
            var usuarioRepo = new UsuarioRepository();
            var usuarios = usuarioRepo.ListarTodos();

            var usuario = usuarios.FirstOrDefault(u =>
                u.Email.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                u.Nome.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
                throw new Exception("Usuário não encontrado. Verifique o nome ou email e tente novamente.");

            return usuario;
        }

        public static FalhaEnergia BuscarFalhaPorId(int idFalha)
        {
            var falhaRepo = new FalhaEnergiaRepository();
            var falhas = falhaRepo.ListarTodos();

            var falha = falhas.FirstOrDefault(f => f.Id == idFalha);

            if (falha == null)
                throw new Exception($"Falha de energia com ID {idFalha} não encontrada.");

            return falha;
        }
    }
}

