using System;

using UIKit;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using System.Threading.Tasks;
using System.Net.Http;
using icom.globales;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;


namespace icom
{
	public partial class ReporteServicio : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;

		public static List<String> lstref = new List<String>();
		public static Boolean stacsec = false;

		public String strNoSerie { get; set; }


		public ReporteServicio () : base ("ReporteServicio", null)
		{
			
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			scrViewRepServicios.ContentSize = new CoreGraphics.CGSize(359, 1783);
			txtDescFalla.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescFalla.Layer.BorderWidth = (nfloat) 2.0;
			txtDescFalla.Text = "";

			txtObs.Layer.BorderColor = UIColor.Black.CGColor;
			txtObs.Layer.BorderWidth = (nfloat) 2.0;
			txtObs.Text = "";

			tblRefacciones.Layer.BorderColor = UIColor.Black.CGColor;
			tblRefacciones.Layer.BorderWidth = (nfloat) 2.0;
			icom.ReporteServicio.lstref.Clear();
			tblRefacciones.Source = new FuenteTablaRefacciones();



			btnaddref.TouchUpInside += delegate {
				lstref.Add(txtaddref.Text);
				tblRefacciones.ReloadData();
				txtaddref.Text = "";
			};

			btnLimpiarRefacciones.TouchUpInside += delegate {
				icom.ReporteServicio.lstref.Clear();
				tblRefacciones.ReloadData();
			};


			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando Datos ...");
			View.Add(loadPop);
			clsReporteOpConsulta objresp = await getReporteOperador();
			if (objresp == null)
			{
				this.NavigationController.PopToRootViewController(true);
			}
			else {
				loadPop.Hide();
				txtFolio.Text = objresp.folio;
				txtEquipo.Text = objresp.equipo;
				txtnoserie.Text = objresp.noserie;
				txtkmho.Text = objresp.kmho;
				txtmodelo.Text = objresp.modelo;
				txtDescFalla.Text = objresp.descripcion;


				btnGuardar.Hidden = true;

			}


		}

		public async Task<clsReporteOpConsulta> getReporteOperador()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "reportes/getReporteOperador";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> obj = new Dictionary<string, string>();
			obj.Add("noserie", strNoSerie);
			var json = JsonConvert.SerializeObject(obj);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return null;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}
			clsReporteOpConsulta objresp = new clsReporteOpConsulta();

			objresp.folio = jsonresponse["folio"].ToString();
			objresp.noserie = jsonresponse["noserie"].ToString();
			objresp.fechahora = jsonresponse["fechahora"].ToString();
			objresp.equipo = jsonresponse["equipo"].ToString();
			objresp.kmho = jsonresponse["kmho"].ToString();
			objresp.modelo = jsonresponse["modelo"].ToString();
			objresp.reporto = jsonresponse["reporto"].ToString();
			objresp.tipofalla = jsonresponse["tipofalla"].ToString();
			objresp.atiende = jsonresponse["atiende"].ToString();
			objresp.descripcion = jsonresponse["descripcion"].ToString();


			return objresp;
		}
	}

	public class FuenteTablaRefacciones : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(idPersonaje) as CustomrefaccionesCell;
			if (cell == null)
			{
				cell = new CustomrefaccionesCell((NSString)idPersonaje);
			}

			String refa = icom.ReporteServicio.lstref.ElementAt(indexPath.Row);

			cell.UpdateCell(refa);


			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return icom.ReporteServicio.lstref.Count;
		}
	}

	public class CustomrefaccionesCell : UITableViewCell
	{
		UILabel Refaccion;

		public CustomrefaccionesCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			icom.ReporteServicio.stacsec = !icom.ReporteServicio.stacsec;
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			if (icom.ReporteServicio.stacsec)
			{
				ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);
			}
			else {
				ContentView.BackgroundColor = UIColor.White;
			}



			Refaccion = new UILabel()
			{
				Font = UIFont.FromName("Arial", 15f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};



			ContentView.AddSubviews(new UIView[] { Refaccion });

		}
		public void UpdateCell(string refaccion)
		{
			Refaccion.Text = refaccion;

		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Refaccion.Frame = new CGRect(20, 10, 200, 20);

		}

	}
}


