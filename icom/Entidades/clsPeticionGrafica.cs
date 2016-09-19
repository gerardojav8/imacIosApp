using System;
using System.Collections.Generic;
namespace icom
{
	public class clsPeticionGrafica
	{
		public string imagen {get;set;}	
		public string idusuario { get; set; }
		public List<Dictionary<string, string>> clasificaciones { get; set; }
	}
}
