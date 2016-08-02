using System;
using System.Collections.Generic;
namespace icom
{
	public class clsDetalleEventoAgenda
	{
		public int mes { get; set; }
		public int dia { get; set; }
		public string titulo { get; set; }
		public string comentario { get; set; }
		public string lapso { get; set; }
		public List<String> usuarios { get; set; }
	}
}

