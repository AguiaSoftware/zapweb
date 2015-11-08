using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public enum UnidadeTipo
    {
        ZAP = 0,
        CENTRAL = 1,
        COS = 2,
        TODOS = 3
    }

    public class Unidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public UnidadeTipo Tipo { get; set; }
        public int CidadeId { get; set; }
        public string Hierarquia { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }

        [PetaPoco.Ignore] public List<Unidade> Unidades { get; set; }
        [PetaPoco.Ignore] public List<Usuario> Usuarios { get; set; }
        [PetaPoco.Ignore] public Cidade Cidade { get; set; }
        [PetaPoco.Ignore] public List<Arquivo> Anexos { get; set; }

        public string GetLevelHierarquia() {
            return this.Id + ".";
        }

        public string GetFullLevelHierarquia() {
            return this.Hierarquia + this.GetLevelHierarquia();
        }

        public int GetUnidadeIdPai() {
            var s = this.Hierarquia.Split('.');

            return int.Parse(s[s.Length - 2]);
        }

        public bool IsParent(int unidadeId) {
            var s = this.Hierarquia.Split('.');

            foreach (var id in s)
            {
                if (unidadeId.ToString() == id) return true;
            }

            return false;
        }
    }
}