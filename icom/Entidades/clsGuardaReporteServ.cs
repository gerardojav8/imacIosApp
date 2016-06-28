using System;
using System.Collections.Generic;
namespace icom
{
	public class clsGuardaReporteServ
	{
		public String folio { get; set; }
		public String kmho { get; set; }
		public String idrealizo { get; set; }
		public String tiemporeparacion { get; set; }
		public String retraso { get; set; }
		public String idtipomnto { get; set; }
		public String idtipofalla { get; set; }
		public String observaciones { get; set; }
		public List<clsRefaccionesReporteServicio> refacciones { get; set; }


		public clsGuardaReporteServ()
		{
		}
	}
}

