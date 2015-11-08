using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public enum AccountType
    {
        SUPER = 0,
        DEFAULT = 1
    }

    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }        
        public bool Ativa { get; set; }        
        public int UsuarioId { get; set; }
        public int GrupoPermissaoId { get; set; }
        public AccountType Tipo { get; set; }

        [PetaPoco.Ignore] public List<Session> Sessions { get; set; }
        [PetaPoco.Ignore] public string Presence { get; set; }
        [PetaPoco.Ignore] public static Account Current { get; set; }
        [PetaPoco.Ignore] public Usuario Usuario { get; set; }
        [PetaPoco.Ignore] public GrupoPermissao Permissao { get; set; }

        [PetaPoco.Ignore] public bool isAuthenticate { 
            get {

                if (Presence == null) return false;

                return Presence.Length == 0 ? false : true;
            } 
        }
    }
}