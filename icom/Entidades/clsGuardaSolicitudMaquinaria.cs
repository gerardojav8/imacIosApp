using System;
using System.Collections.Generic;
namespace icom
{
	public class clsGuardaSolicitudMaquinaria
	{
		public String requeridopara { get; set; }
		public String idobra { get; set; }
		public String idresponsable { get; set; }
		public List<clsSolicitudesMaquinas> requerimientos {get; set;}
		public clsGuardaSolicitudMaquinaria()
		{
		}
	}
}

