using System;

using UIKit;
using Foundation;
using icom.globales.ModalViewPicker;
using System.Collections.Generic;
using icom.globales;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;


namespace icom
{
	public partial class ExportarTareasController : UIViewController
	{
		public ExportarTareasController() : base("ExportarTareasController", null)
		{
		}

		LoadingOverlay loadPop;
		HttpClient client;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnDesde.TouchUpInside += DatePickerFechaInicio;
			btnHasta.TouchUpInside += DatePickerFechafin;
			btnAceptar.TouchUpInside += creaPDF;
		}

		private async void creaPDF(object sender, EventArgs e) { 


			if (txtDesde.Text.Equals(""))
			{
				funciones.MessageBox("Error", "Debe de ingresar una fecha de inicio");
				return;
			}

			if (txtHasta.Text.Equals(""))
			{
				funciones.MessageBox("Error", "Debe de ingresar una fecha de Final");
				return;
			}

			DateTime dtfechaini = DateTime.Parse(txtDesde.Text);
			DateTime dtfechafin = DateTime.Parse(txtHasta.Text);

			int respcomp = DateTime.Compare(dtfechafin, dtfechaini);

			if (respcomp < 0)
			{
				funciones.MessageBox("Error", "la fecha de fin  debe de ser mayor o igual a la fecha de inicio");
				return;
			}

			String strpdf = await creaPDFServer();

			if (!strpdf.Equals(""))
			{
				

				string nombrefile = "tareas" + Consts.idusuarioapp + ".pdf";
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

		public async Task<String> creaPDFServer()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Exportando PDF...");
			View.Add(loadPop);

			client = new HttpClient();
			//client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/exportaPDF";
			var uri = new Uri(string.Format(url));


			Dictionary<string, string> pet = new Dictionary<string, string>();
			pet.Add("fechaini", txtDesde.Text);
			pet.Add("fechafin", txtHasta.Text);
			pet.Add("idusuario", Consts.idusuarioapp);

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
				return "";
			}

			if (responseString.Equals("-2"))
			{
				loadPop.Hide();
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

		async void DatePickerFechaInicio(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};


				modalPicker.DatePicker.Mode = UIDatePickerMode.Date;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };					

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;

					txtDesde.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
				};

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void DatePickerFechafin(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };


				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatterFecha.Locale = locale;


				txtHasta.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}
	}
}


