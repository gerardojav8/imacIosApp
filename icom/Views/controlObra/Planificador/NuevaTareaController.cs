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
	public partial class NuevaTareaController : UIViewController
	{
		public NuevaTareaController() : base("NuevaTareaController", null)
		{
		}


		public UIViewController viewobras { get; set; }
		public int idcategoria { get; set; }
		LoadingOverlay loadPop;
		HttpClient client;

		public override void ViewDidLoad()
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

			btnGuardar.TouchUpInside += guardarTarea;

			btnInicio.TouchUpInside += DatePickerFechaInicio;
			btnFinal.TouchUpInside += DateTimePikerFechafin;

			bajatecladoinputs();
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

		async void guardarTarea(object sender, EventArgs e)
		{
			if (txtTitulo.Text.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la Tarea no puede ser vacio, verifiquelo por favor");
				return;
			}

			if (txtInicio.Text.Equals("") || txtFinal.Text.Equals("")) { 
				funciones.MessageBox("Error", "Debe de seleccionar una fecha inicial y una fecha final, verifiquelo por favor");
				return;
			}

			if (txtPorcentaje.Text.Equals("")) { 
				funciones.MessageBox("Error", "Debe agregar un porcentaje, verifiquelo por favor");
				return;
			}

			Boolean resp = await saveTarea();

			if (resp)
			{
				((PlanificadorController)viewobras).recargarListado();
				this.NavigationController.PopToViewController(viewobras, true);
			}
		}

		public async Task<Boolean> saveTarea()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Tarea...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/NuevaTarea";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idcategoria", idcategoria.ToString());
			pet.Add("titulo", txtTitulo.Text);
			int td = 0;
			if (swTodoDia.On){
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

			var result = jsonresponse["result"].ToString();


			if (result == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "Error al guardar los datos, intentelo nuevamente");
				return false;
			}

			if (result.Equals("0"))
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			funciones.MessageBox("Aviso", "Se ha guardado la Tarea!!!");
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


