using Pillar.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class NotificacaoRules
    {
        public void SendToUnidade(Notificacao notificacao, int unidadeId) {
            var realTimeRepositorio = new RealTimeRepositorio();
            var accountRepositorio = new AccountRepositorio();
            var accounts = accountRepositorio.FetchByUnidadeId(unidadeId);
            var notificacaoRepositorio = new NotificacaoRepositorio();
            var notificacaoUsuarioRepositorio = new NotificacaoUsuarioRepositorio();

            foreach (var account in accounts)
            {
                notificacao.Para = account.Usuario;
                notificacaoRepositorio.Add(notificacao);
                notificacaoUsuarioRepositorio.Increment(notificacao.Para);

                foreach (var session in account.Sessions)
                {
                    var realTime = realTimeRepositorio.Fetch(session.Presence);
                    if (realTime != null)
                    {
                        Pillar.RealTime.RealTime.SendMessage(realTime.ConnectionId, new Pillar.RealTime.Protocol()
                        {
                            Event = "new::notificacao",
                            Data = notificacao
                        });
                    }

                }
            }
        }

        public void MarcarLida(int Id) {
            var notificacaoRepositorio = new NotificacaoRepositorio();

            notificacaoRepositorio.MarcarLida(Id);
        }

        public void MarcarLida()
        {
            var notificacaoUsuarioRepositorio = new NotificacaoUsuarioRepositorio();

            notificacaoUsuarioRepositorio.MarcarLida(Account.Current.Usuario);
        }

        public NotificacaoUsuario Get() {
            var notificacaoUsuarioRepositorio = new NotificacaoUsuarioRepositorio();

            return notificacaoUsuarioRepositorio.Fetch(Account.Current.Usuario);
        }

        public List<Notificacao> All(Paging paging) {
            var notificacaoRepositorio = new NotificacaoRepositorio();

            return notificacaoRepositorio.Fetch(Account.Current.UsuarioId, paging);
        }

        public void TodasLida()
        {
            var notificacaoRepositorio = new NotificacaoRepositorio();

            notificacaoRepositorio.MarcarLidaByUsuario(Account.Current.Usuario);
        }
    }
}