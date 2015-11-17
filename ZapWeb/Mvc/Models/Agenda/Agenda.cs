﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZapWeb.Models
{
    public class Agenda
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public int UnidadeId { get; set; }
        public int UsuarioId { get; set; }

        [PetaPoco.Ignore] public Unidade Unidade { get; set; }
        [PetaPoco.Ignore] public Usuario Usuario { get; set; }
    }
}