using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZapWeb.Lib.Mvc;

namespace ZapWeb.Models
{
    public class NotificacaoRepositorio : RepositorioTemp
    {
        public void Add(Notificacao notificacao) {

            notificacao.Id = 0;
            notificacao.DeId = notificacao.De.Id;
            notificacao.ParaId = notificacao.Para.Id;

            this.Db.Insert(notificacao);
        }

        public List<Notificacao> Fetch(int usuarioId, Paging paging) {
            var sql = PetaPoco.Sql.Builder.Append("SELECT Notificacao.*, De.*, Para.*, Unidade.*")
                                          .Append("FROM Notificacao")
                                          .Append("INNER JOIN Usuario AS De ON De.Id = Notificacao.DeId")
                                          .Append("INNER JOIN Usuario AS Para ON Para.Id = Notificacao.ParaId")
                                          .Append("INNER JOIN Unidade ON Unidade.Id = De.UnidadeId")
                                          .Append("WHERE Notificacao.ParaId = @0", usuarioId)
                                          .Append("ORDER BY Notificacao.Data DESC")
                                          .Append("LIMIT @0, @1", paging.LimitDown, paging.LimitUp);

            return this.Db.Fetch<Notificacao, Usuario, Usuario, Unidade, Notificacao>((n, d, p, u) => {

                n.De = d;
                n.Para = p;
                n.De.Unidade = u;

                return n;
            }, sql).ToList();
        }

        public void MarcarLida(int notificacaoId) {
            this.Db.Execute("UPDATE Notificacao SET Lida = 1 WHERE Id = @0", notificacaoId);
        }

        public void MarcarLidaByUsuario(Usuario usuario)
        {
            this.Db.Execute("UPDATE Notificacao SET Lida = 1 WHERE ParaId = @0", usuario.Id);
        }
    }
}