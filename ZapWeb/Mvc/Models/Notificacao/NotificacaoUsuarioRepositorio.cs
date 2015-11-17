using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class NotificacaoUsuarioRepositorio : RepositorioTemp
    {

        public void Add(NotificacaoUsuario notificacaoUsuario) {
            this.Db.Insert(notificacaoUsuario);
        }

        public void Increment(Usuario usuario) {

            this.Db.Execute("UPDATE NotificacaoUsuario SET Total = Total + 1 WHERE UsuarioId = @0", usuario.Id);
        }

        public void MarcarLida(Usuario usuario) {
            this.Db.Execute("UPDATE NotificacaoUsuario SET Total = 0 WHERE UsuarioId = @0", usuario.Id);
        }

        public NotificacaoUsuario Fetch(Usuario usuario) {
            return this.Db.SingleOrDefault<NotificacaoUsuario>("SELECT NotificacaoUsuario.* FROM NotificacaoUsuario WHERE UsuarioId = @0", usuario.Id);
        }
    }
}