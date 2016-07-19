using System;
using UIKit;

namespace icom
{
	public static class funciones
	{
		public static void MessageBox(string titulo, string mensaje)
		{
			using (UIAlertView Alerta = new UIAlertView())
			{
				Alerta.Title = titulo;
				Alerta.Message = mensaje;
				Alerta.AddButton("Enterado");
				Alerta.Show();
			};
		}

		public static string getNombreMes(int nomes) {
			switch (nomes) {
				case 1: return "Enero"; 
				case 2: return "Febrero"; 
				case 3: return "Marzo"; 
				case 4: return "Abril"; 
				case 5: return "Mayo"; 
				case 6: return "Junio"; 
				case 7: return "Julio"; 
				case 8: return "Agosto"; 
				case 9: return "Septiembre";
				case 10: return "Octubre";
				case 11: return "Novimebre";
				case 12: return "Diciembre";
				default: return "";
			}
		}

		public static UIColor getColorMes(int nomes) { 
			switch (nomes)
			{
				case 1: return UIColor.FromRGB(82, 121, 174);
				case 2: return UIColor.FromRGB(104, 156, 151);
				case 3: return UIColor.FromRGB(183, 176, 118);
				case 4: return UIColor.FromRGB(255, 149, 151);
				case 5: return UIColor.FromRGB(82, 121, 174);
				case 6: return UIColor.FromRGB(104, 156, 151);
				case 7: return UIColor.FromRGB(183, 176, 118);
				case 8: return UIColor.FromRGB(255, 149, 151);
				case 9: return UIColor.FromRGB(82, 121, 174);
				case 10: return UIColor.FromRGB(104, 156, 151);
				case 11: return UIColor.FromRGB(183, 176, 118);
				case 12: return UIColor.FromRGB(255, 149, 151);
				default: return UIColor.FromRGB(82, 121, 174);
			}
		}
	}
}

