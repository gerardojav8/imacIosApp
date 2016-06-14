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
	}
}

