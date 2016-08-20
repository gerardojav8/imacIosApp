using System;

using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Drawing;

namespace icom
{
	public partial class ReporteOperador : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;
		List<clsCmbUsuarios> lstusuarios;
		List<clsTipoFallas> lsttipofallas;

		UIActionSheet actShReporto;
		UIActionSheet actShTipoFalla;
		UIActionSheet actShAtiende;


		int idreporto = 0;
		int idatiende = 0;
		int idfalla = 0;

		public UIViewController viewmaq
		{
			get;
			set;
		}


		public String strNoeconomico{ get; set;}

		public String strNoSerie{ get; set; }

		public String strModelo { get; set; }

		public ReporteOperador () : base ("ReporteOperador", null)
		{
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (UIScreen.MainScreen.Bounds.Width == 414)
			{
				ScrView.ContentSize = new CoreGraphics.CGSize(355, 1200);
			}
			else { 
				ScrView.ContentSize = new CoreGraphics.CGSize(316, 1200);
			}
			txtDescripcion.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescripcion.Layer.BorderWidth = (nfloat) 2.0;
			txtDescripcion.Text = "";

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando Datos ...");
			View.Add(loadPop);


			int tieneRep = await tieneRepServicio();

			if (tieneRep == -1) { 
				this.NavigationController.PopToViewController(viewmaq, true);
			}

			if (tieneRep == 1)
			{
				clsReporteOpConsulta objresp = await getReporteOperador();
				if (objresp == null)
				{
					this.NavigationController.PopToRootViewController(true);
				}
				else { 
					loadPop.Hide();

					txtFolio.Text = objresp.folio;
					txtnoserie.Text = objresp.noserie;
					txtfechahora.Text = objresp.fechahora;
					txtequipo.Text = objresp.equipo;
					txtkmho.Text = objresp.kmho;
					txtmodelo.Text = objresp.modelo;
					txtreporto.Text = objresp.reporto;
					txtfipofalla.Text = objresp.tipofalla;
					txtatiende.Text = objresp.atiende;
					txtDescripcion.Text = objresp.descripcion;

					btnGuardar.Hidden = true;

				}




			}
			else 
			{

				String folio = await getFolio();

				if (folio == "")
				{
					this.NavigationController.PopToRootViewController(true);
				}


				txtFolio.Text = folio;

				String fecha = await getFechaHoraActual();

				if (fecha == "")
				{
					this.NavigationController.PopToRootViewController(true);
				}

				txtfechahora.Text = fecha;

				lstusuarios = new List<clsCmbUsuarios>();
				lsttipofallas = new List<clsTipoFallas>();

				Boolean respus = await getUsuarios();
				if (!respus)
				{
					this.NavigationController.PopToRootViewController(true);
				}

				Boolean resptip = await getTipoFallas();
				if (!resptip)
				{
					this.NavigationController.PopToRootViewController(true);
				}
				else {
					loadPop.Hide();
				}


				inicializaCombos();


				inicializadatos();
				btnGuardar.Hidden = false;
			
			}

			btnGuardar.TouchUpInside += guardarReporte;

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {txtkmho.EndEditing(true);});
			toolbar.Items = new UIBarButtonItem[] {new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace), doneButton};
			this.txtkmho.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtDescripcion.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtDescripcion.InputAccessoryView = toolbar;


		}

		async void guardarReporte(object sender, EventArgs e)
		{
			if (idfalla == 0) {
				funciones.MessageBox("Error", "Debe de seleccionar un tipo de falla para guardar el reporte");
				return;
			}

			if (idatiende == 0)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un usuario de atiende para guardar el reporte");
				return;
			}

			if (idreporto == 0)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un usuario de reporto para guardar el reporte");
				return;
			}

			if (txtkmho.Text == "") 
			{
				funciones.MessageBox("Error", "Debe de ingresar una cantidad para Km/Horometro");
				return;
			}

			Decimal kmho;
			if (!Decimal.TryParse(txtkmho.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"),out kmho)) { 
				funciones.MessageBox("Error", "La cantidad ingresada para el Km/Horometro debe de ser decimal");
				return;
			}

			String folio = await saveRep();

			if (folio != "") {
				((MaquinasController)viewmaq).recargarListado();
				this.NavigationController.PopToViewController(viewmaq, true);
			}
		}

		public async Task<String> saveRep() { 
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Reporte...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "reportes/GuardarReporteOperador";
			var uri = new Uri(string.Format(url));

			clsGuardaReporteOp objreporte = new clsGuardaReporteOp();

			objreporte.noserie = txtnoserie.Text;
			String strkmho = txtkmho.Text;
			strkmho = strkmho.Replace(",",".");
			objreporte.kmho = strkmho;
			objreporte.modelo = txtmodelo.Text;
			objreporte.idreporto = idreporto.ToString();
			objreporte.idtipofalla = idfalla.ToString();
			objreporte.idatiende = idatiende.ToString();
			objreporte.descripcion = txtDescripcion.Text;

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
				return "";
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return "";
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
				return "";
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return "";
			}

			String folio = jsonresponse["folio"].ToString();
			funciones.MessageBox("Aviso", "Se ha guardado el reporte con folio: " + folio);
			return folio;

		}

		public async Task<String> getFolio()
		{
			

			client = new HttpClient();
			string url = Consts.ulrserv + "reportes/getFolioReporte";
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
				return "";
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
				return "";
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return "";
			}

			String folio = jsonresponse["folio"].ToString();
			return folio;
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
			objresp.atiende = jsonresponse["atiende"].ToString();
			objresp.descripcion = jsonresponse["descripcion"].ToString();


			return objresp;
		}


		public async Task<String> getFechaHoraActual()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "common/getFechaHoraActual";
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
				return "";
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
				return "";
			}

			String fecha = jsonresponse["Fecha"].ToString();
			String hora = jsonresponse["hora"].ToString();
			return fecha + " " + hora;
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



		public void inicializadatos()
		{
			txtequipo.Text = strNoeconomico;
			txtmodelo.Text = strModelo;
			txtnoserie.Text = strNoSerie;


		}

		public void inicializaCombos(){

			//--------Combo Reporto---------------------
			actShReporto = new UIActionSheet ("Seleccionar");
			foreach (clsCmbUsuarios us in lstusuarios)
			{
				String nombre = us.nombre + " " + us.apepaterno + " " + us.apematerno;
				actShReporto.Add(nombre);
			}
			actShReporto.Add("Cancelar");

			actShReporto.Style = UIActionSheetStyle.BlackTranslucent;
			actShReporto.CancelButtonIndex = lstusuarios.Count;

			btnReporto.TouchUpInside += delegate {
				actShReporto.ShowInView (this.ContentView);
			};

			actShReporto.Clicked += delegate(object sender, UIButtonEventArgs e) {
				if (e.ButtonIndex != lstusuarios.Count)
				{
					clsCmbUsuarios us = lstusuarios.ElementAt((int)e.ButtonIndex);
					txtreporto.Text = us.nombre + " " + us.apepaterno + " " + us.apematerno;
					idreporto = us.idusuario;
				}
				else {
					txtreporto.Text = "";
					idreporto = 0;
				}
			};

			//-----------Combo atiende-------------------

			actShAtiende = new UIActionSheet("Seleccionar");
			foreach (clsCmbUsuarios us in lstusuarios)
			{
				String nombre = us.nombre + " " + us.apepaterno + " " + us.apematerno;
				actShAtiende.Add(nombre);
			}
			actShAtiende.Add("Cancelar");
			actShAtiende.Style = UIActionSheetStyle.BlackTranslucent;
			actShAtiende.CancelButtonIndex = lstusuarios.Count;

			btnAtiende.TouchUpInside += delegate
			{
				actShAtiende.ShowInView(this.ContentView);
			};

			actShAtiende.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstusuarios.Count)
				{
					clsCmbUsuarios us = lstusuarios.ElementAt((int)e.ButtonIndex);
					txtatiende.Text = us.nombre + " " + us.apepaterno + " " + us.apematerno;
					idatiende = us.idusuario;
				}
				else {
					txtatiende.Text = "";
					idatiende = 0;
				}
			};

			//--------Combo tipofalla---------------------

			actShTipoFalla = new UIActionSheet ("Seleccionar");

			foreach (clsTipoFallas tip in lsttipofallas)
			{
				String falla = tip.nombre;
				actShTipoFalla.Add(falla);
			}
			actShTipoFalla.Add("Cancelar");


			actShTipoFalla.Style = UIActionSheetStyle.BlackTranslucent;
			actShTipoFalla.CancelButtonIndex = lsttipofallas.Count;

			btnTipoFalla.TouchUpInside += delegate {
				actShTipoFalla.ShowInView (this.ContentView);
			};

			actShTipoFalla.Clicked += delegate(object sender, UIButtonEventArgs e) {
				if (e.ButtonIndex != lsttipofallas.Count)
				{
					clsTipoFallas tip = lsttipofallas.ElementAt((int)e.ButtonIndex);
					txtfipofalla.Text = tip.nombre;
					idfalla = tip.idtipofalla;
				}
				else {
					txtfipofalla.Text = "";
					idfalla = 0;
				}
			};

		}


		private void MessageBox(string titulo, string mensaje){
			using(UIAlertView Alerta = new UIAlertView()){
				Alerta.Title = titulo;
				Alerta.Message = mensaje;
				Alerta.AddButton ("Enterado");
				Alerta.Show ();
			};
		}
	}
}


