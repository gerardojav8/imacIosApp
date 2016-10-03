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
using System.Drawing;



namespace icom
{
	public partial class solicitudMaquinaController : UIViewController
	{

		List<clsSolicitudesMaquinas> lstsolmaq = new List<clsSolicitudesMaquinas>();

		LoadingOverlay loadPop;
		HttpClient client;

		UIActionSheet actResponsables;
		List<clsCmbUsuarios> lstResponsables;
		int idresponsable = -1;

		UIActionSheet actAreasObra;
		List<clsCmbObras> lstAreasObra = new List<clsCmbObras>();
		int idarea = -1;

		public UIViewController viewmaq
		{
			get;
			set;
		}



		public solicitudMaquinaController() : base("solicitudMaquinaController", null)
		{
			
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UIScreen.MainScreen.Bounds.Width == 414)			
			{
				scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(359, 1783);
			}
			else {
				scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(316, 1783);
			}

			btnAgregar.Layer.CornerRadius = 10;
			btnAgregar.ClipsToBounds = true;

			btnlimpiar.Layer.CornerRadius = 10;
			btnlimpiar.ClipsToBounds = true;

			btnSolicitar.Layer.CornerRadius = 10;
			btnSolicitar.ClipsToBounds = true;

			lstRequerimientos.Layer.BorderColor = UIColor.Black.CGColor;
			lstRequerimientos.Layer.BorderWidth = (nfloat)2.0;
			lstsolmaq.Clear();
			lstRequerimientos.Source = new FuenteTablaRequerimientos(lstsolmaq);

			var bounds = UIScreen.MainScreen.Bounds;
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

			lstAreasObra = new List<clsCmbObras>();

			Boolean respareas = await getAreasObra();
			if (!respareas)
			{
				this.NavigationController.PopToRootViewController(true);
			}

			loadPop.Hide();

			inicializaCombos();

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

				lstsolmaq.Add(objsol);

				lstRequerimientos.ReloadData();

				limpiaseleccion();
			};

			btnlimpiar.TouchUpInside += delegate {
				lstsolmaq.Clear();
				lstRequerimientos.ReloadData();
				limpiaseleccion();
			};



			btnSolicitudFecha.TouchUpInside += DatePickerButtonTapped;

			btnSolicitar.TouchUpInside += guardarSolicitud;

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtCantidad.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtCantidad.InputAccessoryView = toolbar;

			txtEquiposolicitado.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtmarca.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtModelo.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };

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
			objsolicitud.idobra = idarea.ToString();
			objsolicitud.idresponsable = idresponsable.ToString();
			objsolicitud.requerimientos = lstsolmaq;

			var json = JsonConvert.SerializeObject(objsolicitud);


			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return "";
			}

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
			string url = Consts.ulrserv + "obras/getObras";
			var uri = new Uri(string.Format(url));

			var json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return false;
			}

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
				clsCmbObras objarea = getobjAreaObra(ob);
				lstAreasObra.Add(objarea);
			}

			return true;
		}

		public clsCmbObras getobjAreaObra(Object varjson)
		{			
			clsCmbObras objob = new clsCmbObras();
			JObject json = (JObject)varjson;

			objob.idobra = Int32.Parse(json["idobra"].ToString());
			objob.nombre = json["nombre"].ToString();

			return objob;
		}

		public async Task<Boolean> getUsuarios()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "usuarios/getCmbUsuarios";
			var uri = new Uri(string.Format(url));

			var json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return false;
			}

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

			var json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return null;
			}

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

			var json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return null;
			}

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
			foreach (clsCmbObras ob in lstAreasObra)
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
					clsCmbObras ob = lstAreasObra.ElementAt((int)e.ButtonIndex);
					txtAreaObra.Text = ob.nombre;
					idarea = ob.idobra;
				}
				else {
					txtAreaObra.Text = "";
					idarea = -1;
				}
			};

		}
	}


}


