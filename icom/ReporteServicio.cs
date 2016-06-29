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

		UIActionSheet actShRealizo;
		private List<clsCmbUsuarios> lstusuarios;
		int idrealizo = -1;

		UIActionSheet actShTipoFalla;
		private List<clsTipoFallas> lsttipofallas;
		int idtipofalla = -1;

		UIActionSheet actShTipoMnto;
		private List<clsTipoMnto> lsttipomnto;
		int idtipomnto = -1;

		public static List<String> lstref = new List<String>();
		public static Boolean stacsec = false;



		public String strNoSerie { get; set; }
		public UIViewController viewmaq { get; set; }


		public ReporteServicio () : base ("ReporteServicio", null)
		{
			
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			scrViewRepServicios.ContentSize = new CoreGraphics.CGSize(359, 1783);
			txtDescFalla.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescFalla.Layer.BorderWidth = (nfloat)2.0;
			txtDescFalla.Text = "";
			txtDescFalla.Editable = false;

			txtObs.Layer.BorderColor = UIColor.Black.CGColor;
			txtObs.Layer.BorderWidth = (nfloat)2.0;
			txtObs.Text = "";

			tblRefacciones.Layer.BorderColor = UIColor.Black.CGColor;
			tblRefacciones.Layer.BorderWidth = (nfloat)2.0;
			icom.ReporteServicio.lstref.Clear();
			tblRefacciones.Source = new FuenteTablaRefacciones();

			lstusuarios = new List<clsCmbUsuarios>();
			lsttipofallas = new List<clsTipoFallas>();
			lsttipomnto = new List<clsTipoMnto>();

			btnaddref.TouchUpInside += delegate
			{
				lstref.Add(txtaddref.Text);
				tblRefacciones.ReloadData();
				txtaddref.Text = "";
			};

			btnLimpiarRefacciones.TouchUpInside += delegate
			{
				icom.ReporteServicio.lstref.Clear();
				tblRefacciones.ReloadData();
			};

			btnGuardar.TouchUpInside += guardarReporte;


			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando Datos ...");
			View.Add(loadPop);

			int resptienerep = await tieneRepServicio();

			if (resptienerep == 0) {

				funciones.MessageBox("Aviso", "La maquina no tiene Reporte de operador, debe de ingresar primero un reporte de operador para capturar el reporte de servicio");
				this.NavigationController.PopToViewController(viewmaq, true);
				return;
			}
			
			
			clsReporteOpConsulta objresp = await getReporteOperador();
			if (objresp == null)
			{
				this.NavigationController.PopToRootViewController(true);
				return;
			}

			txtFolio.Text = objresp.folio;
			txtEquipo.Text = objresp.equipo;
			txtnoserie.Text = objresp.noserie;
			txtkmho.Text = objresp.kmho;
			txtmodelo.Text = objresp.modelo;
			txtDescFalla.Text = objresp.descripcion;
			txtTipoFalla.Text = objresp.tipofalla;
			idtipofalla = Int32.Parse(objresp.idtipofalla);

			Boolean respus = await getUsuarios();
			if (!respus)
			{
				this.NavigationController.PopToViewController(viewmaq, true);
				return;
			}

			Boolean resptip = await getTipoFallas();
			if (!resptip)
			{
				this.NavigationController.PopToViewController(viewmaq, true);
				return;
			}

			Boolean resptipmnto = await getTipoMantenimientos();
			if (!resptipmnto)
			{
				this.NavigationController.PopToViewController(viewmaq, true);
				return;
			}

			inicializaCombos();
			loadPop.Hide();

		}

		public async Task<int> tieneRepServicio()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/tieneReporte";
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
				return -1;
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
				return -1;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return -1;
			}

			int resp = Int32.Parse(jsonresponse["tieneReporte"].ToString());
			return resp;
		}

		async void guardarReporte(object sender, EventArgs e)
		{

			if (txttiemporep.Text == "") { 
				funciones.MessageBox("Error", "Debe de ingresar un tiempo de reparacion, para guardar el reporte");
				return;
			}

			if (idrealizo == -1)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un usuario de realizo, para guardar el reporte");
				return;
			}

			if (idtipomnto == -1)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un tipo de mantenimiento para guardar el reporte");
				return;
			}

			if (idtipofalla == -1)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un tipo de falla para guardar el reporte");
				return;
			}

			if (txtkmho.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar una cantidad para Km/Horometro");
				return;
			}

			Decimal kmho;
			if (!Decimal.TryParse(txtkmho.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out kmho))
			{
				funciones.MessageBox("Error", "La cantidad ingresada para el Km/Horometro debe de ser decimal");
				return;
			}

			Boolean respsavserv = await saveRepServ();

			if (respsavserv)
			{
				((MaquinasController)viewmaq).recargarListado();

				this.NavigationController.PopToViewController(viewmaq, true);
			}
		}

		public async Task<Boolean> saveRepServ()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Reporte...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "reportes/GuardarReporteServicio";
			var uri = new Uri(string.Format(url));

			clsGuardaReporteServ objreporte = new clsGuardaReporteServ();

			objreporte.folio = txtFolio.Text;
			String strkmho = txtkmho.Text;
			strkmho = strkmho.Replace(",", ".");
			objreporte.kmho = strkmho;
			objreporte.idrealizo = idrealizo.ToString();
			objreporte.tiemporeparacion = txttiemporep.Text;
			objreporte.idtipomnto = idtipomnto.ToString();
			objreporte.idtipofalla = idtipofalla.ToString();
			objreporte.observaciones = txtObs.Text;

			int intRetraso = 0;

			if (segRetraso.SelectedSegment == 0) {
				intRetraso = 1;
			}

			objreporte.retraso = intRetraso.ToString();

			List<clsRefaccionesReporteServicio> lstrefserv = new List<clsRefaccionesReporteServicio>();

			foreach (String strref in lstref) {
				clsRefaccionesReporteServicio objrefser = new clsRefaccionesReporteServicio();
				objrefser.nombre_refaccion = strref;
				lstrefserv.Add(objrefser);
			}
			
			objreporte.refacciones = lstrefserv;


			var json = JsonConvert.SerializeObject(objreporte);

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
				return false;
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return false;
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
				return false;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}


			return true;

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
			objresp.kmho = jsonresponse["kmho"].ToString().Replace(",", ".");
			objresp.modelo = jsonresponse["modelo"].ToString();
			objresp.reporto = jsonresponse["reporto"].ToString();
			objresp.tipofalla = jsonresponse["tipofalla"].ToString();
			objresp.idtipofalla = jsonresponse["idtipofalla"].ToString();
			objresp.atiende = jsonresponse["atiende"].ToString();
			objresp.descripcion = jsonresponse["descripcion"].ToString();


			return objresp;
		}

		public async Task<Boolean> getUsuarios()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "usuarios/getCmbUsuarios";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
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
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer los usuarios del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var us in jrarray)
			{
				clsCmbUsuarios objus = getobjUsuario(us);
				lstusuarios.Add(objus);
			}

			return true;
		}

		public clsCmbUsuarios getobjUsuario(Object varjson)
		{
			clsCmbUsuarios objus = new clsCmbUsuarios();
			JObject json = (JObject)varjson;

			objus.idusuario = Int32.Parse(json["idusuario"].ToString());
			objus.nombre = json["nombre"].ToString();
			objus.apepaterno = json["apepaterno"].ToString();
			objus.apematerno = json["apematerno"].ToString();

			return objus;
		}

		public async Task<Boolean> getTipoFallas()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "reportes/getTipoFallas";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
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
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer los tipos de falla del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var tip in jrarray)
			{
				clsTipoFallas objtip = getTipoFalla(tip);
				lsttipofallas.Add(objtip);
			}

			return true;
		}

		public clsTipoFallas getTipoFalla(Object varjson)
		{
			clsTipoFallas objtip = new clsTipoFallas();
			JObject json = (JObject)varjson;

			objtip.idtipofalla = Int32.Parse(json["idtipofalla"].ToString());
			objtip.nombre = json["nombre"].ToString();
			objtip.descripcion = json["descripcion"].ToString();


			return objtip;
		}

		public async Task<Boolean> getTipoMantenimientos()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "reportes/getTipoMntos";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
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
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer los tipos de mantenimiento del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var tip in jrarray)
			{
				clsTipoMnto objtip = getTipoMnto(tip);
				lsttipomnto.Add(objtip);
			}

			return true;
		}

		public clsTipoMnto getTipoMnto(Object varjson)
		{
			clsTipoMnto objtip = new clsTipoMnto();
			JObject json = (JObject)varjson;

			objtip.idtipomnto = Int32.Parse(json["idtipomnto"].ToString());
			objtip.nombre = json["nombre"].ToString();
			objtip.descripcion = json["descripcion"].ToString();


			return objtip;
		}

		public void inicializaCombos()
		{

			//--------Combo Reporto---------------------
			actShRealizo = new UIActionSheet("Seleccionar");
			foreach (clsCmbUsuarios us in lstusuarios)
			{
				String nombre = us.nombre + " " + us.apepaterno + " " + us.apematerno;
				actShRealizo.Add(nombre);
			}
			actShRealizo.Add("Cancelar");

			actShRealizo.Style = UIActionSheetStyle.BlackTranslucent;
			actShRealizo.CancelButtonIndex = lstusuarios.Count;

			btnrealizo.TouchUpInside += delegate
			{
				actShRealizo.ShowInView(this.ContentViewRepServicios);
			};

			actShRealizo.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstusuarios.Count)
				{
					clsCmbUsuarios us = lstusuarios.ElementAt((int)e.ButtonIndex);
					txtrealizo.Text = us.nombre + " " + us.apepaterno + " " + us.apematerno;
					idrealizo = us.idusuario;
				}
				else {
					txtrealizo.Text = "";
					idrealizo = -1;
				}
			};

			//--------Combo tipofalla---------------------

			actShTipoFalla = new UIActionSheet("Seleccionar");

			foreach (clsTipoFallas tip in lsttipofallas)
			{
				String falla = tip.nombre;
				actShTipoFalla.Add(falla);
			}
			actShTipoFalla.Add("Cancelar");


			actShTipoFalla.Style = UIActionSheetStyle.BlackTranslucent;
			actShTipoFalla.CancelButtonIndex = lsttipofallas.Count;

			btntipofalla.TouchUpInside += delegate
			{
				actShTipoFalla.ShowInView(this.ContentViewRepServicios);
			};

			actShTipoFalla.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lsttipofallas.Count)
				{
					clsTipoFallas tip = lsttipofallas.ElementAt((int)e.ButtonIndex);
					txtTipoFalla.Text = tip.nombre;
					idtipofalla = tip.idtipofalla;
				}
				else {
					txtTipoFalla.Text = "";
					idtipofalla = -1;
				}
			};

			//--------Combo tipo mantenimiento---------------------

			actShTipoMnto = new UIActionSheet("Seleccionar");

			foreach (clsTipoMnto tip in lsttipomnto)
			{
				String mantenimiento = tip.nombre;
				actShTipoMnto.Add(mantenimiento);
			}
			actShTipoMnto.Add("Cancelar");


			actShTipoMnto.Style = UIActionSheetStyle.BlackTranslucent;
			actShTipoMnto.CancelButtonIndex = lsttipomnto.Count;

			btntipomnto.TouchUpInside += delegate
			{
				actShTipoMnto.ShowInView(this.ContentViewRepServicios);
			};

			actShTipoMnto.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lsttipomnto.Count)
				{
					clsTipoMnto tip = lsttipomnto.ElementAt((int)e.ButtonIndex);
					txtTipoMnto.Text = tip.nombre;
					idtipomnto = tip.idtipomnto;
				}
				else {
					txtTipoMnto.Text = "";
					idtipomnto = -1;
				}
			};
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


