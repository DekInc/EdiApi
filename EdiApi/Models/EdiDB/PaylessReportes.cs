﻿using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessReportes
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public string PeriodoF { get; set; }
        public string FechaGen { get; set; }
        public string Tipo { get; set; }
    }
}
