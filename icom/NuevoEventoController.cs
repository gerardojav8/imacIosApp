using System;

using UIKit;
using Foundation;
using icom.globales.ModalViewPicker;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using icom.globales;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public partial class NuevoEventoController : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;
		public static Boolean stacsec = false;

		List<clsCmbUsuarios> lstasistentescombo;
		public static List<String> lstasistentes = new List<string>();
		List<int> lstidasistentes = new List<int>();
		UIActionSheet actShAsistentes;
		int idasistentesel = -1;

		public UIViewController viewagenda { get; set; }


		public NuevoEventoController() : base("NuevoEventoController", null)
		{
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UIScreen.MainScreen.Bounds.Width == 414)
			{
				scrNuevoEvento.ContentSize = new CoreGraphics.CGSize(355, 1200);
			}
			else {
				scrNuevoEvento.ContentSize = new CoreGraphics.CGSize(316, 1200);

			}
			swTodoeldia.On = false;
			swTodoeldia.ValueChanged += delegate
			{
				if (swTodoeldia.On)
				{
					txthorainicio.Text = "00:00:00";
					txtHoraFin.Text = "23:59:59";
					if (!txtfechaevento.Text.Equals(""))
					{
						txtFechaFin.Text = txtfechaevento.Text;
					}
					else {
						txtFechaFin.Text = "";
					}
					btnFechafin.Enabled = false;
				}
				else {
					txthorainicio.Text = "";
					txtHoraFin.Text = "";
					txtFechaFin.Text = "";
					btnFechafin.Enabled = true;
				}
			};


			btnfecha.TouchUpInside += DatePickerFechaEvento;
			btnFechafin.TouchUpInside += DateTimePikerFechafin;

			txtComentario.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentario.Layer.BorderWidth = (nfloat)2.0;
			txtComentario.Text = "";

			tblAsistentes.Layer.BorderColor = UIColor.Black.CGColor;
			tblAsistentes.Layer.BorderWidth = (nfloat)2.0;
			icom.NuevoEventoController.lstasistentes.Clear();
			tblAsistentes.Source = new FuenteTablaAsistentes();



			txtTitulo.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};

			UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
			{
				txtComentario.EndEditing(true);
			});

			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace), doneButton
			};

			this.txtComentario.InputAccessoryView = toolbar;
			lstasistentescombo = new List<clsCmbUsuarios>();

			btnBuscarUsuarios.TouchUpInside += buscaUsuarios;

			btnAgregarAsistentes.TouchUpInside += delegate {
				if (idasistentesel > -1)
				{
					lstasistentes.Add(txtAsistentes.Text);
					lstidasistentes.Add(idasistentesel);
					tblAsistentes.ReloadData();
					txtAsistentes.Text = "";
					idasistentesel = -1;
				}
				else {
					funciones.MessageBox("Mensaje", "Debe de seleccionar un usuario para agregar");
				}
			};

			btnEliminarAsistentes.TouchUpInside += delegate {
				icom.NuevoEventoController.lstasistentes.Clear();
				lstidasistentes.Clear();
				tblAsistentes.ReloadData();
			};

			btnaceptar.TouchUpInside += guardaEvento;

		}

		async void guardaEvento(object sender, EventArgs e) {

			if (txtTitulo.Text.Equals("")) {
				funciones.MessageBox("Error", "Debe de ingresar un titulo al evento");
				return;
			}

			if (txtfechaevento.Text.Equals("") || txthorainicio.Text.Equals("")) { 						

				funciones.MessageBox("Error", "Debe de ingresar una fecha y hora de inicio al evento");
				return;
			}

			if (txtFechaFin.Text.Equals("") || txtHoraFin.Text.Equals(""))
			{

				funciones.MessageBox("Error", "Debe de ingresar una fecha y hora de fin al evento");
				return;
			}

			if (lstidasistentes.Count <= 0) { 
				funciones.MessageBox("Error", "Debe de agregar al menos a un asistente");
				return;
			}

			Boolean resp = await saveEve();

			if (resp) { 				
				this.NavigationController.PopToViewController(viewagenda, true);
			}
		}

		public async Task<Boolean> saveEve()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Evento...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/guardaEventoAgenda";
			var uri = new Uri(string.Format(url));

			clsGuardaNuevoEvento objev = new clsGuardaNuevoEvento();

			objev.titulo = txtTitulo.Text;
			objev.fechaini = txtfechaevento.Text;
			objev.fechafin = txtFechaFin.Text;
			objev.horaini = txthorainicio.Text;
			objev.horafin = txtHoraFin.Text;
			objev.notas = txtComentario.Text;

			if (swTodoeldia.On)
			{
				objev.diacompleto = "1";
			}
			else { 
				objev.diacompleto = "0";
			}

			if (swNotificarInvitados.On)
			{
				objev.notificaasistentes = "1";
			}
			else {
				objev.notificaasistentes = "0";
			}

			List<Dictionary<String, String>> lstasis = new List<Dictionary<string, string>>();

			foreach (int id in lstidasistentes) {
				Dictionary<String, String> asis = new Dictionary<string, string>();
				asis.Add("idusuario", id.ToString());
				lstasis.Add(asis);
			}

			objev.asistentes = lstasis;

			var json = JsonConvert.SerializeObject(objev);

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

			jtokenerror = jsonresponse["result"].ToString();


			if (jtokenerror.Equals("1"))
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			funciones.MessageBox("Aviso", "Se ha guardado el evento!!");
			return true;

		}

		async void buscaUsuarios(object sender, EventArgs e)
		{
			lstasistentescombo.Clear();
			txtAsistentes.EndEditing(true);
			if (txtAsistentes.Text.Length < 1)
			{
				funciones.MessageBox("Error", "Debe de ingresar al menos cuatro caracteres para realizar la busqueda de usuarios");
				return;
			}



			Boolean respSearch = await searchUsers();

			if (respSearch)
			{
				llenacmbAsistentes();
				loadPop.Hide();
				actShAsistentes.ShowInView(this.ContentNuevoEvento);

			}
			else {
				txtAsistentes.Text = "";
				idasistentesel = -1;
			}

		}

		public async Task<Boolean> searchUsers()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando usuarios...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "usuarios/getUsuariosSearch";
			var uri = new Uri(string.Format(url));

			Dictionary<String, String> param = new Dictionary<String, String>();

			param.Add("nombre", txtAsistentes.Text);
			var json = JsonConvert.SerializeObject(param);

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

				var jtokenresult = jsonresponse["result"].ToString();
				if (jtokenresult.Equals("0")) { 
					funciones.MessageBox("Error", "No se han encontrado coincidencias");
					return false;
				}

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
				lstasistentescombo.Add(objus);
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

		public void llenacmbAsistentes()
		{

			actShAsistentes = new UIActionSheet("Seleccionar");
			foreach (clsCmbUsuarios us in lstasistentescombo)
			{
				String nombre = us.nombre + " " + us.apepaterno + " " + us.apematerno;
				actShAsistentes.Add(nombre);
			}
			actShAsistentes.Add("Cancelar");

			actShAsistentes.Style = UIActionSheetStyle.BlackTranslucent;
			actShAsistentes.CancelButtonIndex = lstasistentescombo.Count;


			actShAsistentes.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstasistentescombo.Count)
				{
					clsCmbUsuarios us = lstasistentescombo.ElementAt((int)e.ButtonIndex);
					txtAsistentes.Text = us.nombre + " " + us.apepaterno + " " + us.apematerno;
					idasistentesel = us.idusuario;
				}
				else {
					txtAsistentes.Text = "";
					idasistentesel = -1;
				}
			};
		}


		async void DatePickerFechaEvento(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			if (!swTodoeldia.On)
			{
				modalPicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };
					var dateFormatterHora = new NSDateFormatter() { DateFormat = "HH:mm:ss" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;
					dateFormatterHora.Locale = locale;

					txtfechaevento.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
					txthorainicio.Text = dateFormatterHora.ToString(modalPicker.DatePicker.Date);
				};
			}
			else { 
				modalPicker.DatePicker.Mode = UIDatePickerMode.Date;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;

					txtfechaevento.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
					txtFechaFin.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
				};
			}

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void DateTimePikerFechafin(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };
				var dateFormatterHora = new NSDateFormatter() { DateFormat = "HH:mm:ss" };

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatterFecha.Locale = locale;
				dateFormatterHora.Locale = locale;

				txtFechaFin.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
				txtHoraFin.Text = dateFormatterHora.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

	}

	public class FuenteTablaAsistentes : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return icom.NuevoEventoController.lstasistentes.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(idPersonaje) as CustomAsistentesCell;
			if (cell == null)
			{
				cell = new CustomAsistentesCell((NSString)idPersonaje);
			}

			String asis = icom.NuevoEventoController.lstasistentes.ElementAt(indexPath.Row);

			cell.UpdateCell(asis);


			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

	}

	public class CustomAsistentesCell : UITableViewCell
	{
		UILabel lblasistente;

		public CustomAsistentesCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			icom.NuevoEventoController.stacsec = !icom.NuevoEventoController.stacsec;
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			if (icom.NuevoEventoController.stacsec)
			{
				ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);
			}
			else {
				ContentView.BackgroundColor = UIColor.White;
			}


			lblasistente = new UILabel()
			{
				Font = UIFont.FromName("Arial", 15f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};



			ContentView.AddSubviews(new UIView[] { lblasistente });

		}
		public void UpdateCell(string refaccion)
		{
			lblasistente.Text = refaccion;



		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			lblasistente.Frame = new CGRect(20, 10, 700, 20);

		}

	}
}


