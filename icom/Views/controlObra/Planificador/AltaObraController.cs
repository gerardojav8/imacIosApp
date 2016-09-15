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


namespace icom
{
	public partial class AltaObraController : UIViewController
	{
		public UIViewController viewobras { get; set; } 
		LoadingOverlay loadPop;
		HttpClient client;

		public AltaObraController() : base("AltaObraController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtdescripcion.Layer.BorderColor = UIColor.Black.CGColor;
			txtdescripcion.Layer.BorderWidth = (nfloat)2.0;
			txtdescripcion.Text = "";

			bajatecladoinputs();
		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtdescripcion.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtdescripcion.InputAccessoryView = toolbar;


			txtnombreobra.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };

			btnGuardarObra.TouchUpInside += guardarObra;


		}

		async void guardarObra(object sender, EventArgs e)
		{
			if (txtnombreobra.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la obra no puede ser vacio, verifiquelo por favor");
				return;
			}


			Boolean resp = await saveObra();

			if (resp)
			{
				((MaquinasController)viewobras).recargarListado();
				this.NavigationController.PopToViewController(viewobras, true);
			}
		}

		public async Task<Boolean> saveObra()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Obra...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/NuevaObra";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("nombre", txtnombreobra.Text);
			pet.Add("descripcion", txtdescripcion.Text);

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

			if (result.Equals("0")) { 
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}


			funciones.MessageBox("Aviso", "Se ha guardado la obra!!!");
			return true;

		}


		double ajuste = 150;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtdescripcion.IsFirstResponder)
			{				

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


