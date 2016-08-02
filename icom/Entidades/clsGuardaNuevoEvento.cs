using System;
using System.Collections.Generic;
namespace icom
{
	public class clsGuardaNuevoEvento
	{
		public string titulo { get; set;}
		public string fechaini { get; set; }
		public string fechafin { get; set; }
		public string horaini { get; set; }
		public string horafin { get; set; }
		public string notas { get; set; }
		public string diacompleto { get; set; }
		public string notificaasistentes { get; set; }
		public List<Dictionary<String, String>> asistentes { get; set; }

	}
}

