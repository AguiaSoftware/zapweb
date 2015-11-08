using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ZapWeb.Lib.Mvc;

using Pillar.RealTime;
using Pillar.Util;

namespace ZapWeb.Models
{
    public class DespesaRules : BusinessLogic
    {
        public bool Add(Despesa despesa) {

            if (!Account.Current.Permissao.Has("ADD_DESPESA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            if (despesa.Fornecedor == null || despesa.Anexos == null || despesa.Items == null || despesa.Unidade == null ||
                despesa.Usuario == null)
            {
                return false;
            }

            if (despesa.Anexos.Count == 0 || despesa.Items.Count == 0)
            {
                return false;
            }
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidadeCurrent = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidadeCurrent.Tipo == UnidadeTipo.ZAP) {
                despesa.Status = DespesaStatus.AUTORIZADA;
            }

            var despesaRepositorio = new DespesaRepositorio();
            despesaRepositorio.Add(despesa);
            despesaRepositorio.UpdateItems(despesa);
            despesaRepositorio.UpdateAnexos(despesa);

            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Adicionada por " + Account.Current.Usuario.Nome,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            despesaHistoricoRepositorio.Add(historico);

            //bug: loop historico <-> despesa
            historico.Despesa = null;
            despesa.Historicos = new List<DespesaHistorico>();
            despesa.Historicos.Add(historico);

            return true;
        }

        public bool Update(Despesa despesa)
        {

            if (!Account.Current.Permissao.Has("UPDATE_DESPESA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            if (despesa.Fornecedor == null || despesa.Anexos == null || despesa.Items == null || despesa.Unidade == null ||
                despesa.Usuario == null)
            {
                return false;
            }

            if (despesa.Anexos.Count == 0 || despesa.Items.Count == 0)
            {
                return false;
            }

            var despesaRepositorio = new DespesaRepositorio();
            var despesaOld = despesaRepositorio.Fetch(despesa.Id);
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidadeCurrent = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if ((despesaOld.Status == DespesaStatus.ABERTA) ||
                (despesaOld.Status == DespesaStatus.NAO_PAGA) ||
                ((despesaOld.Status == DespesaStatus.REMETIDA || despesa.Status == DespesaStatus.NAO_AUTORIZADA) && unidadeCurrent.Id == despesa.Unidade.GetUnidadeIdPai()) ||
                (unidadeCurrent.Tipo == UnidadeTipo.ZAP))
            {
                despesaRepositorio.UpdateItems(despesa);
            }

            despesaRepositorio.Update(despesa);            
            despesaRepositorio.UpdateAnexos(despesa);

            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Atualizada por " + Account.Current.Usuario.Nome,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            despesaHistoricoRepositorio.Add(historico);

            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            return true;
        }

        public bool Remeter(Despesa despesa) {
            var despesaRepositorio = new DespesaRepositorio();

            if (despesa.Id != 0)
            {
                var despesaOld = despesaRepositorio.Fetch(despesa.Id);

                if (despesaOld.Status != DespesaStatus.ABERTA && despesaOld.Status != DespesaStatus.NAO_PAGA)
                {
                    this.MessageError = "DESPESA_REMETIDA";
                    return false;
                }
            }

            despesa.Status = DespesaStatus.REMETIDA;

            //adiciona se for nova
            if (despesa.Id == 0)
            {
                this.Add(despesa);
            }
            else {
                this.Update(despesa);
            }

            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Remetida por " + Account.Current.Usuario.Nome,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            
            despesaHistoricoRepositorio.Add(historico);
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            var notificacaoRules = new NotificacaoRules();
            notificacaoRules.SendToUnidade(new Notificacao()
            {
                Data = DateTime.Now,
                De = Account.Current.Usuario,
                Message = "Solicitação de pagamento",
                Icon = "fa fa-money",
                Href = "Despesa/Editar/" + despesa.Id
            }, unidade.GetUnidadeIdPai());

            return true;
        }

        public bool Pagar(Despesa despesa) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (Account.Current.Usuario.Unidade.Id == unidade.GetUnidadeIdPai()) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var despesaRepositorio = new DespesaRepositorio();
            despesa.Status = DespesaStatus.PAGA;
            despesaRepositorio.Update(despesa);

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Pago por " + Account.Current.Usuario.Nome,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };
            
            despesaHistoricoRepositorio.Add(historico);
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            var notificacaoRules = new NotificacaoRules();
            notificacaoRules.SendToUnidade(new Notificacao()
            {
                Data = DateTime.Now,
                De = Account.Current.Usuario,
                Message = "Solicitação de autorização",
                Icon = "fa fa-money",
                Href = "Despesa/Editar/" + despesa.Id
            }, unidade.GetUnidadeIdPai());

            return true;
        }

        public bool NaoPagar(Despesa despesa)
        {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (Account.Current.Usuario.Unidade.Id == unidade.GetUnidadeIdPai())
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var despesaRepositorio = new DespesaRepositorio();
            despesa.Status = DespesaStatus.NAO_PAGA;
            despesaRepositorio.Update(despesa);

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Não Pago por " + Account.Current.Usuario.Nome + "<br/>" + despesa.Justificativa,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            despesaHistoricoRepositorio.Add(historico);
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            var unidadeDespesa = unidadeRepositorio.Fetch(despesa.UnidadeId);
            var notificacaoRules = new NotificacaoRules();
            notificacaoRules.SendToUnidade(new Notificacao()
            {
                Data = DateTime.Now,
                De = Account.Current.Usuario,
                Message = "Negado pedido de pagamento",
                Icon = "fa fa-thumbs-o-down",
                Href = "Despesa/Editar/" + despesa.Id
            }, unidadeDespesa.Id);

            return true;
        }

        public bool Autorizar(Despesa despesa) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP) {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var despesaRepositorio = new DespesaRepositorio();
            despesa.Status = DespesaStatus.AUTORIZADA;
            despesaRepositorio.Update(despesa);

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Autorizada por " + Account.Current.Usuario.Nome,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            despesaHistoricoRepositorio.Add(historico);
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            return true;
        }

        public bool NaoAutorizar(Despesa despesa)
        {
            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            if (unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return false;
            }

            var despesaRepositorio = new DespesaRepositorio();
            despesa.Status = DespesaStatus.NAO_AUTORIZADA;
            despesaRepositorio.Update(despesa);

            var despesaHistoricoRepositorio = new DespesaHistoricoRepositorio();
            var historico = new DespesaHistorico()
            {
                Data = DateTime.Now,
                Descricao = "Não autorizada por " + Account.Current.Usuario.Nome + "<br/>" + despesa.Justificativa,
                Usuario = Account.Current.Usuario,
                Despesa = despesa
            };

            despesaHistoricoRepositorio.Add(historico);
            despesa.Historicos = despesaHistoricoRepositorio.Fetch(despesa);

            var unidadeDespesa = unidadeRepositorio.Fetch(despesa.UnidadeId);            
            var notificacaoRules = new NotificacaoRules();
            notificacaoRules.SendToUnidade(new Notificacao()
            {
                Data = DateTime.Now,
                De = Account.Current.Usuario,
                Message = "Negado pedido de autorização",
                Icon = "fa fa-thumbs-o-down",
                Href = "Despesa/Editar/" + despesa.Id
            }, unidadeDespesa.GetUnidadeIdPai());

            return true;
        }

        public Despesa Get(int Id) {
            var despesaRepositorio = new DespesaRepositorio();

            if (!Account.Current.Permissao.Has("UPDATE_DESPESA"))
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            var unidadeRepositorio = new UnidadeRepositorio();
            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            var despesa = despesaRepositorio.Fetch(Id);
            if (despesa.Unidade.Id != unidade.Id &&
                !despesa.Unidade.IsParent(Account.Current.Usuario.Unidade.Id) &&
                unidade.Tipo != UnidadeTipo.ZAP)
            {
                this.MessageError = "USUARIO_SEM_PERMISSAO";
                return null;
            }

            return despesa;
        }

        public List<Despesa> Pesquisar(DespesaPesquisa parametroPesquisa, Paging paging) {
            var unidadeRepositorio = new UnidadeRepositorio();
            var despesaRepositorio = new DespesaRepositorio();

            var unidade = unidadeRepositorio.Fetch(Account.Current.Usuario.Unidade.Id);

            return despesaRepositorio.Fetch(parametroPesquisa, unidade, paging);
        }

    }
}