using System;
using UIKit;
using Foundation;
using icom.globales;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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

		public static Task<int> MessageBoxCancelOk(string titulo, string mensaje)
		{
			var tcs = new TaskCompletionSource<int>();
			var alert = new UIAlertView
			{
				Title = titulo,
				Message = mensaje
			};

			alert.AddButton("Cancelar");
			alert.AddButton("OK");
			alert.Show();
			alert.Clicked += (s, e) => tcs.TrySetResult((int)e.ButtonIndex);
			return tcs.Task;

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

		public static NSDate ConvertDateTimeToNSDate(DateTime date)
		{
			DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
				new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - newDate).TotalSeconds);
		}

		public static void SalirSesion(UIViewController controllerFrom) {
			var logcont = (Login)Consts.logincontroller;
			logcont.limpiacampos();

			Consts.token = "";
			Consts.idusuarioapp = "";
			Consts.nombreusuarioapp = "";
			Consts.inicialesusuarioapp = "";

			controllerFrom.NavigationController.PopToViewController(logcont, true);

		}

		public static Byte[] getBytesFromImage(String path) { 
		
			UIImage img = UIImage.FromFile(path);
			Byte[] imageBytes;
			using (NSData imagenData = img.AsJPEG())
			{
				imageBytes = new Byte[imagenData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imagenData.Bytes, imageBytes, 0, Convert.ToInt32(imagenData.Length));
			}

			return imageBytes;

		}

		public static String getBase64Image(String path) {
			Byte[] imageBytes = getBytesFromImage(path);
			return Convert.ToBase64String(imageBytes);
		}

		public static async Task<String> llamadaRest(HttpClient client, Uri uri, LoadingOverlay loadPop, String json, string token) {

			if (token.Equals("")) return "-1";
				
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);

			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return "-2";
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return "-2";
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();

			var jsonresponse = JObject.Parse(responseString);
			var jtokenerror = jsonresponse["Message"];
			if (jtokenerror != null)
			{
				String msg = jtokenerror.ToString();
				if (msg.ToLower().Contains("se ha denegado")) {
					return "-1";
				}
			}
			return responseString;
		}

		public static int getNumeroAleatorioSinRepetir(int desde, int hasta, List<int> numeros) {

			Random rn = new Random();
			int num = 0;

			do
			{
				num = rn.Next(desde, hasta);
			} while (numeros.Any(x => x == num));

			return num;
		}

	}
}

