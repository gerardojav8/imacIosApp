using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using CoreGraphics;
using icom.globales.ModalViewPicker;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using icom.globales;
using System.Text;



namespace icom
{
	public partial class solicitudMaquinaController : UIViewController
	{

		public static List<clsSolicitudesMaquinas> lstsolmaq = new List<clsSolicitudesMaquinas>();
		public static Boolean stacsec = false;

		LoadingOverlay loadPop;
		HttpClient client;

		UIActionSheet actResponsables;
		List<clsCmbUsuarios> lstResponsables;
		int idresponsable = -1;

		UIActionSheet actAreasObra;
		List<clsCmbAreasObra> lstAreasObra = new List<clsCmbAreasObra>();
		int idarea = -1;

		public UIViewController viewmaq
		{
			get;
			set;
		}



		public solicitudMaquinaController() : base("solicitudMaquinaController", null)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UIScreen.MainScreen.Bounds.Width == 414)			
			{
				scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(359, 1783);
			}
			else {
				scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(316, 1783);
			}


			lstRequerimientos.Layer.BorderColor = UIColor.Black.CGColor;
			lstRequerimientos.Layer.BorderWidth = (nfloat)2.0;
			icom.solicitudMaquinaController.lstsolmaq.Clear();
			lstRequerimientos.Source = new FuenteTablaRequerimientos();

			/*var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando Datos ...");
			View.Add(loadPop);

			String folio = await getFolio();

			if (folio == "")
			{
				this.NavigationController.PopToRootViewController(true);
			}


			txtRequerimiento.Text = folio;

			String fecha = await getFechaHoraActual();

			if (fecha == "")
			{
				this.NavigationController.PopToRootViewController(true);
			}

			txtSolicitud.Text = fecha;

			lstResponsables = new List<clsCmbUsuarios>();

			Boolean respus = await getUsuarios();
			if (!respus)
			{
				this.NavigationController.PopToRootViewController(true);
			}

			lstAreasObra = new List<clsCmbAreasObra>();

			Boolean respareas = await getAreasObra();
			if (!respareas)
			{
				this.NavigationController.PopToRootViewController(true);
			}

			loadPop.Hide();

			inicializaCombos();*/


			btnAgregar.TouchUpInside += delegate {

				if (txtCantidad.Text == "") {
					funciones.MessageBox("Error", "Debe de ingresar una cantidad");
					return;
				}

				int intcant;
				if (!Int32.TryParse(txtCantidad.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out intcant))
				{
					funciones.MessageBox("Error", "La cantidad ingresada debe de ser numercia");
					return;
				}

				if (txtEquiposolicitado.Text == "") { 
					funciones.MessageBox("Error", "Debe de seleccionar un equipo para agregar");
					return;
				}

				if (txtModelo.Text == "")
				{
					funciones.MessageBox("Error", "Debe de seleccionar un modelo para agregar");
					return;
				}

				if (txtmarca.Text == "")
				{
					funciones.MessageBox("Error", "Debe de seleccionar una marca para agregar");
					return;
				}



				clsSolicitudesMaquinas objsol = new clsSolicitudesMaquinas();
				objsol.cantidad = Int32.Parse(txtCantidad.Text);
				objsol.equipo = txtEquiposolicitado.Text;
				objsol.marca = txtmarca.Text;
				objsol.modelo = txtModelo.Text;

				icom.solicitudMaquinaController.lstsolmaq.Add(objsol);

				lstRequerimientos.ReloadData();

				limpiaseleccion();
			};

			btnlimpiar.TouchUpInside += delegate {
				icom.solicitudMaquinaController.lstsolmaq.Clear();
				lstRequerimientos.ReloadData();
				limpiaseleccion();
			};



			btnSolicitudFecha.TouchUpInside += DatePickerButtonTapped;

			btnSolicitar.TouchUpInside += guardarSolicitud;

		}

		async void guardarSolicitud(object sender, EventArgs e)
		{
			if (txtRequeridaPara.Text == "")
			{
				funciones.MessageBox("Error", "Debe de seleccionar una fecha de requerimiento");
				return;
			}

			if (idresponsable == -1)
			{
				funciones.MessageBox("Error", "Debe de seleccionar un usuario responsable de la solicitud");
				return;
			}

			if (idarea == -1)
			{
				funciones.MessageBox("Error", "Debe de seleccionar una area de obra");
				return;
			}

			if (lstsolmaq.Count() < 1) {
				funciones.MessageBox("Error", "Debe de ingresar algun requerimiento para la solicitud");
				return;
			}

			String respsave = await saveRep();

			if (respsave == "")
			{
				((MaquinasController)viewmaq).recargarListado();
				this.NavigationController.PopToViewController(viewmaq, true);
			}
		}

		public async Task<String> saveRep()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Solicitud...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "maquinas/GuardarSolicitudMaquinaria";
			var uri = new Uri(string.Format(url));

			clsGuardaSolicitudMaquinaria objsolicitud = new clsGuardaSolicitudMaquinaria();

			objsolicitud.requeridopara = txtRequeridaPara.Text;
			objsolicitud.idareaobra = idarea.ToString();
			objsolicitud.idresponsable = idresponsable.ToString();
			objsolicitud.requerimientos = lstsolmaq;

			var json = JsonConvert.SerializeObject(objsolicitud);

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

			funciones.MessageBox("Aviso", "Se ha guardado la solicitud!!");
			return "";

		}

		public async Task<Boolean> getAreasObra()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "obras/getAreasObras";
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

			foreach (var ob in jrarray)
			{
				clsCmbAreasObra objarea = getobjAreaObra(ob);
				lstAreasObra.Add(objarea);
			}

			return true;
		}

		public clsCmbAreasObra getobjAreaObra(Object varjson)
		{
			clsCmbAreasObra objob = new clsCmbAreasObra();
			JObject json = (JObject)varjson;

			objob.idarea = Int32.Parse(json["idareaobra"].ToString());
			objob.nombre = json["nombre"].ToString();

			return objob;
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
				lstResponsables.Add(objus);
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

		public async Task<String> getFolio()
		{

			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getFolioSolicitud";
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


		async void DatePickerButtonTapped(object sender, EventArgs e)
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
				var dateFormatter = new NSDateFormatter()
				{
					DateFormat = "yyyy-MM-dd"
				};

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatter.Locale = locale;
				txtRequeridaPara.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		public void limpiaseleccion() { 

			txtCantidad.Text = "";
			txtmarca.Text = "";
			txtModelo.Text = "";
			txtEquiposolicitado.Text = "";
		}

		public void inicializaCombos()
		{			
			//--------Combo Responsables---------------------
			actResponsables = new UIActionSheet("Seleccionar");
			foreach (clsCmbUsuarios us in lstResponsables)
			{				
				actResponsables.Add(us.nombre);
			}
			actResponsables.Add("Cancelar");

			actResponsables.Style = UIActionSheetStyle.BlackTranslucent;
			actResponsables.CancelButtonIndex = lstResponsables.Count;

			btnResponsable.TouchUpInside += delegate
			{
				actResponsables.ShowInView(this.contentViewSolicitudMaquina);
			};

			actResponsables.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstResponsables.Count)
				{
					clsCmbUsuarios us = lstResponsables.ElementAt((int)e.ButtonIndex);
					txtResponsable.Text = us.nombre;
					idresponsable = us.idusuario;
				}
				else {
					txtResponsable.Text = "";
					idresponsable = -1;
				}
			};

			//--------Combo Areas Obra---------------------
			actAreasObra = new UIActionSheet("Seleccionar");
			foreach (clsCmbAreasObra ob in lstAreasObra)
			{
				actAreasObra.Add(ob.nombre);
			}
			actAreasObra.Add("Cancelar");

			actAreasObra.Style = UIActionSheetStyle.BlackTranslucent;
			actAreasObra.CancelButtonIndex = lstAreasObra.Count;

			btnAreaObra.TouchUpInside += delegate
			{
				actAreasObra.ShowInView(this.contentViewSolicitudMaquina);
			};

			actAreasObra.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstAreasObra.Count)
				{
					clsCmbAreasObra ob = lstAreasObra.ElementAt((int)e.ButtonIndex);
					txtAreaObra.Text = ob.nombre;
					idarea = ob.idarea;
				}
				else {
					txtAreaObra.Text = "";
					idarea = -1;
				}
			};

		}
	}

	public class FuenteTablaRequerimientos : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(idPersonaje) as CustomSolMaqCell;
			if (cell == null)
			{
				cell = new CustomSolMaqCell((NSString)idPersonaje);
			}

			clsSolicitudesMaquinas sol = icom.solicitudMaquinaController.lstsolmaq.ElementAt(indexPath.Row);

			cell.UpdateCell(sol.cantidad.ToString(), sol.equipo, sol.marca, sol.modelo);


			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return icom.solicitudMaquinaController.lstsolmaq.Count;
		}

	}

	public class CustomSolMaqCell : UITableViewCell
	{
		UILabel cantidadlabel, equipolabel, marcalabel, modelolabel;

		public CustomSolMaqCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			icom.solicitudMaquinaController.stacsec = !icom.solicitudMaquinaController.stacsec;
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			if (icom.solicitudMaquinaController.stacsec)
			{
				ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);
			}
			else {
				ContentView.BackgroundColor = UIColor.White;
			}



			cantidadlabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			equipolabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};
			marcalabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			modelolabel = new UILabel()				
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			ContentView.AddSubviews(new UIView[] { cantidadlabel, equipolabel, marcalabel, modelolabel });

		}
		public void UpdateCell(string cantidad, string equipo, String marca, String modelo)
		{
			cantidadlabel.Text = cantidad;
			equipolabel.Text = equipo;
			marcalabel.Text = marca;
			modelolabel.Text = modelo;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			cantidadlabel.Frame = new CGRect( 20, 10, 30, 20);
			equipolabel.Frame = new CGRect(55 ,  10, 100, 20);
			marcalabel.Frame = new CGRect(170, 10, 70, 20);
			modelolabel.Frame = new CGRect(250, 10, 70, 20);
		}

	}
}


