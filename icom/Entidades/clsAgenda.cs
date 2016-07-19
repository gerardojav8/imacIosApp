using System;
using System.Collections.Generic;
namespace icom
{
	public class clsAgenda
	{
		public int mes { get; set; }
		public string comentario { get; set; }
		public List<clsEventoAgenda> lstEventos { get; set;}

		public clsAgenda() {
			lstEventos = new List<clsEventoAgenda>();
		}
	}
}

