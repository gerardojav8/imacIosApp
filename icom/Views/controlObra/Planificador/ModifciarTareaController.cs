using System;

using UIKit;
using Foundation;
using CoreGraphics;
using icom.globales;
using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using icom.globales.ModalViewPicker;
namespace icom
{
	public partial class ModifciarTareaController : UIViewController
	{
		public ModifciarTareaController() : base("ModifciarTareaController", null)
		{
		}
		public UIViewController viewtareas { get; set; }
		public int idtarea { get; set; }
		LoadingOverlay loadPop;
		HttpClient client;

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);


			txtNotas.Layer.BorderColor = UIColor.Black.CGColor;
			txtNotas.Layer.BorderWidth = (nfloat)2.0;
			txtNotas.Text = "";

			swTodoDia.On = false;
			swTodoDia.ValueChanged += delegate
			{
				if (swTodoDia.On)
				{
					if (!txtInicio.Text.Equals(""))
					{
						txtFinal.Text = txtInicio.Text.Substring(0, 10) + " 23:59:59";
						txtInicio.Text = txtInicio.Text.Substring(0, 10) + " 00:00:00";
					}
					else {
						txtFinal.Text = "";
					}
					btnFinal.Enabled = false;
				}
				else {

					txtInicio.Text = "";
					txtFinal.Text = "";
					btnFinal.Enabled = true;
				}
			};

			btnGuardar.TouchUpInside += modificaTarea;
			btnEliminar.TouchUpInside += BorraTarea;
			btnInicio.TouchUpInside += DatePickerFechaInicio;
			btnFinal.TouchUpInside += DateTimePikerFechafin;

			bajatecladoinputs();

			await cargaDatosTarea();
		}

		public async Task<Boolean> cargaDatosTarea()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de la tarea...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/getTareaById";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idtarea", idtarea.ToString());

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
			}

			var jsonresponse = JObject.Parse(responseString);

			var result = jsonresponse["result"];

			if (result != null)
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			txtObra.Text = jsonresponse["nombreobra"].ToString();
			txtCategoria.Text = jsonresponse["nombrecategoria"].ToString();
			txtTitulo.Text = jsonresponse["titulo"].ToString();

			int td = Int32.Parse(jsonresponse["todoDia"].ToString());
			if (td == 1)
			{
				swTodoDia.On = true;
			}
			else {
				swTodoDia.On = false;
			}

			txtInicio.Text = jsonresponse["inicio"].ToString();
			txtFinal.Text = jsonresponse["fin"].ToString();
			txtPorcentaje.Text = jsonresponse["porcentaje"].ToString();
			txtNotas.Text = jsonresponse["notas"].ToString();

			loadPop.Hide();
			return true;

		}

		async void modificaTarea(object sender, EventArgs e)
		{
			if (txtTitulo.Text.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la Tarea no puede ser vacio, verifiquelo por favor");
				return;
			}

			if (txtInicio.Text.Equals("") || txtFinal.Text.Equals(""))
			{
				funciones.MessageBox("Error", "Debe de seleccionar una fecha inicial y una fecha final, verifiquelo por favor");
				return;
			}

			if (txtPorcentaje.Text.Equals(""))
			{
				funciones.MessageBox("Error", "Debe agregar un porcentaje, verifiquelo por favor");
				return;
			}


			Boolean resp = await modTarea();

			if (resp)
			{
				((PlanificadorController)viewtareas).recargarListado();
				this.NavigationController.PopToViewController(viewtareas, true);
			}
		}

		public async Task<Boolean> modTarea()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Tarea...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/ModificarTarea";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idtarea", idtarea.ToString());
			pet.Add("titulo", txtTitulo.Text);
			int td = 0;
			if (swTodoDia.On)
			{
				td = 1;
			}
			pet.Add("todoDia", td.ToString());
			pet.Add("inicio", txtInicio.Text);
			pet.Add("fin", txtFinal.Text);
			pet.Add("porcentaje", txtPorcentaje.Text);
			pet.Add("notas", txtNotas.Text);

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
			}

			var jsonresponse = JObject.Parse(responseString);

			var result = jsonresponse["result"];


			if (result == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "Error al guardar los datos, intentelo nuevamente");
				return false;
			}

			if (result.ToString().Equals("0"))
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			loadPop.Hide();
			funciones.MessageBox("Aviso", "Se ha guardado la Tarea!!!");
			return true;

		}

		async void BorraTarea(object sender, EventArgs e)
		{
			int resp = await funciones.MessageBoxCancelOk("Aviso", "Esta seguro de borrar la Tarea");
			if (resp == 0)
			{
				return;
			}

			Boolean respborr = await borrTarea();

			if (respborr)
			{
				((PlanificadorController)viewtareas).recargarListado();
				this.NavigationController.PopToViewController(viewtareas, true);
			}
		}

		public async Task<Boolean> borrTarea()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Eliminando Tarea...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/eliminarTarea";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idtarea", idtarea.ToString());

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
			}

			var jsonresponse = JObject.Parse(responseString);

			var result = jsonresponse["result"];


			if (result == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "Error al guardar los datos, intentelo nuevamente");
				return false;
			}

			if (result.ToString().Equals("0") || result.ToString().Equals("2"))
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}



			loadPop.Hide();
			funciones.MessageBox("Aviso", "Se elimino la tarea");
			return true;

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

			if (!swTodoDia.On)
			{
				modalPicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };
					var dateFormatterHora = new NSDateFormatter() { DateFormat = "HH:mm:ss" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;
					dateFormatterHora.Locale = locale;

					txtInicio.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " " + dateFormatterHora.ToString(modalPicker.DatePicker.Date);
				};
			}
			else {
				modalPicker.DatePicker.Mode = UIDatePickerMode.Date;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;

					txtInicio.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " 00:00:00";
					txtFinal.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " 23:59:59";
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

				txtFinal.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " " + dateFormatterHora.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtNotas.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtNotas.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtPorcentaje.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtPorcentaje.InputAccessoryView = toolbar;


			txtTitulo.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		double ajuste = 7;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtNotas.IsFirstResponder || txtPorcentaje.IsFirstResponder)
			{
				if (txtPorcentaje.IsFirstResponder)
				{
					ajuste = 50;
				}
				else {
					ajuste = 7;
				}

				var r = UIKeyboard.FrameBeginFromNotification(notif);

				var keyboardHeight = r.Height;
				if (!blntecladoarriba)
				{
					var desface = (View.Frame.Y - keyboardHeight) + ajuste;
					CGRect newrect = new CGRect(View.Frame.X,
												desface,
												View.Frame.Width,
												View.Frame.Height);

					View.Frame = newrect;
					blntecladoarriba = true;
				}
				else {
					var rr = UIKeyboard.FrameEndFromNotification(notif);
					var hact = View.Frame.Y * -1;
					var hnew = rr.Height;
					var dif = hact - hnew;
					var desface = (View.Frame.Y + dif) + ajuste;
					CGRect newrect = new CGRect(View.Frame.X,
												desface,
												View.Frame.Width,
												View.Frame.Height);

					View.Frame = newrect;


				}
			}

		}

		private void TecladoAbajo(NSNotification notif)
		{
			if (blntecladoarriba)
			{
				var r = UIKeyboard.FrameBeginFromNotification(notif);
				var keyboardHeight = r.Height;
				var desface = View.Frame.Y + keyboardHeight - ajuste;
				CGRect newrect = new CGRect(View.Frame.X,
											desface,
											View.Frame.Width,
											View.Frame.Height);

				View.Frame = newrect;
				blntecladoarriba = false;
			}

		}

	}
}


