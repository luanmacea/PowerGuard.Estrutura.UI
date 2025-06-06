using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerGuard.Estrutura.Model;
using PowerGuard.Estrutura.Repository;

namespace PowerGuard.Estrutura.Controller
{
    public class UsuarioController
    {
        private readonly UsuarioRepository _repo = new();

        public void Cadastrar(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
                throw new Exception("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new Exception("Email é obrigatório.");

            _repo.Inserir(usuario);
        }

        public List<Usuario> Listar() => _repo.ListarTodos();

        public void Atualizar(Usuario usuario) => _repo.Atualizar(usuario);

        public void Remover(int id) => _repo.Remover(id);
    }
}
