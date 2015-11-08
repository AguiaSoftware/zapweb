﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class DespesaHistorico
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }        
        public int DespesaId { get; set; }
        public int UsuarioId { get; set; }

        [PetaPoco.Ignore] public Usuario Usuario { get; set; }
        [PetaPoco.Ignore] public Despesa Despesa { get; set; }
    }
}