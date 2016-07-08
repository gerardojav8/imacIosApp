using System;
using System.Collections.Generic;
namespace icom
{
	public class clsFichaMaquina
	{
		public string noeco { get; set;}
		public string descripcion { get; set; }
		public string marca { get; set; }
		public string modelo { get; set; }
		public string serie { get; set; }
		public int idobraactual { get; set; }
		public string obraactual { get; set; }
		public string imagen { get; set; }
		public int equipoaux { get; set; }
		public string marcaaux { get; set; }
		public string modeloaux { get; set; }
		public string serieaux { get; set; }
		public string idequipoaux { get; set; }
		public List<clsEstadosFisicosMaquinas> estadosfisicos { get; set;}
		public List<clsFiltrosMaquinas> filtros { get; set;}

		public clsFichaMaquina()
		{
		}
	}
}

