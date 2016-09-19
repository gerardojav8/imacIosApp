using System;

using UIKit;
using Alliance.Charts;
using System.Collections.Generic;
using Foundation;
using System.IO;
using CoreGraphics;
using icom.globales;
using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;


namespace icom
{
	public partial class GraficasTareasController : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;
		public AllianceChart grafica;
		public List<clsClasificacion> lstClas { get; set; }
		public GraficasTareasController() : base("GraficasTareasController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			/*lstClas = new List<clsClasificacion>();

			clsClasificacion obj1 = new clsClasificacion
			{
				nombre = "Clasificacion 1",
				porcentaje = 50.0f,
				color = UIColor.Blue
			};

			clsClasificacion obj2 = new clsClasificacion
			{
				nombre = "Clasificacion 2",
				porcentaje = 80.0f,
				color = UIColor.Gray
			};

			clsClasificacion obj3 = new clsClasificacion
			{
				nombre = "Clasificacion 3",
				porcentaje = 24.0f,
				color = UIColor.Green
			};

			clsClasificacion obj4 = new clsClasificacion
			{
				nombre = "Clasificacion 4",
				porcentaje = 50.0f,
				color = UIColor.Orange
			};

			clsClasificacion obj5 = new clsClasificacion
			{
				nombre = "Clasificacion 5",
				porcentaje = 15.0f,
				color = UIColor.Red
			};

			lstClas.Add(obj1);
			lstClas.Add(obj2);
			lstClas.Add(obj3);
			lstClas.Add(obj4);
			lstClas.Add(obj5);*/

			bool unamayoracero = false;
			foreach (clsClasificacion clas in lstClas) {
				if (clas.porcentaje > 0) {
					unamayoracero = true;
					break;
				}
			}

			if (unamayoracero)
			{
				tblClasificaciones.Source = new FuenteTablaGraficas(this, lstClas);

				grafica = new AllianceChart(Chart.Bar, vwGrafica);
				creargrafica();
			}

			btnExportarGrafica.TouchUpInside += delegate {
				crearPDF();
			};


		}

		private async void crearPDF() { 
			String pathimagen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "grafica.png");

			UIGraphics.BeginImageContext(vwGrafica.Frame.Size);

			try
			{
				using (var context = UIGraphics.GetCurrentContext())
				{
					vwGrafica.Layer.RenderInContext(context);
					using (var image = UIGraphics.GetImageFromCurrentImageContext())
					{


						NSData imgData = image.AsPNG();

						NSError err = null;

						if (!imgData.Save(pathimagen, false, out err))
						{
							funciones.MessageBox("Error", err.LocalizedDescription);
							return;
						}

					}
				}
			}
			finally
			{
				UIGraphics.EndImageContext();
			}

			if (!File.Exists(pathimagen))
			{
				funciones.MessageBox("Error", "No existe la imagen de la grafica, verifiquelo por favor");
				return;
			}

			String strimagenbase64 = funciones.getBase64Image(pathimagen);
			String strpdf = await creaPDFServer(strimagenbase64);


			if (!strpdf.Equals(""))
			{
				string nombrefile = "grafica" + Consts.idusuarioapp + ".pdf";
				String pathtemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nombrefile);
				if (File.Exists(pathtemp))
				{
					File.Delete(pathtemp);
				}

				//Convertir en archivo y guardar
				Byte[] bytesfile = Convert.FromBase64String(strpdf);
				File.WriteAllBytes(pathtemp, bytesfile);

				PreviewDocsController previewDocs = new PreviewDocsController();
				previewDocs.tituloDocumento = nombrefile;
				previewDocs.urlDocumento = pathtemp;
				this.NavigationController.PushViewController(previewDocs, true);
			}
		}

		public async Task<String> creaPDFServer(String str64)
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de Obra y Categoria...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/exportaGraficaPDF";
			var uri = new Uri(string.Format(url));


			clsPeticionGrafica objpetgra = new clsPeticionGrafica();

			objpetgra.imagen = str64;
			objpetgra.idusuario = Consts.idusuarioapp;
			List<Dictionary<string, string>> clasificaciones = new List<Dictionary<string, string>>();
			foreach (clsClasificacion cl in lstClas) { 
				Dictionary<string, string> clas = new Dictionary<string, string>();

				clas.Add("color", cl.strcolor);
				clas.Add("clasificacion", cl.nombre);
				clas.Add("porcentaje", cl.porcentaje.ToString());

				clasificaciones.Add(clas);
			}

			objpetgra.clasificaciones = clasificaciones;




			var json = JsonConvert.SerializeObject(objpetgra);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
				return "";
			}




			var jsonresponse = JObject.Parse(responseString);

			var result = jsonresponse["result"];

			if (result != null)
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return "";
			}

			loadPop.Hide();

			return jsonresponse["pdf"].ToString();

		}

		private void creargrafica() {


			List<ChartComponent> Componentes = new List<ChartComponent>();
			grafica.BarChart.SetupBarViewShape(BarShape.Rounded);
			grafica.BarChart.SetupBarViewStyle(BarDisplayStyle.BarStyleMatte);
			grafica.BarChart.SetupBarViewShadow(BarShadow.Light);
			grafica.BarChart.barWidth = 30;

			foreach (clsClasificacion item in lstClas) { 
				ChartComponent chrtcomp = new ChartComponent
				{
					Name = "",
					value = (float)item.porcentaje,
					color = item.color
				};
				Componentes.Add(chrtcomp);
			}

			grafica.LoadChart(Componentes, Chart.Bar, vwGrafica);
		}
	}
}


