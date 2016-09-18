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
using System.Text;

namespace icom
{
	public partial class ModificarObraController : UIViewController
	{
		public UIViewController viewobras { get; set; }
		public int idobra { get; set; }
		LoadingOverlay loadPop;
		HttpClient client;

		public ModificarObraController() : base("ModificarObraController", null)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtDescripcionobra.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescripcionobra.Layer.BorderWidth = (nfloat)2.0;
			txtDescripcionobra.Text = "";



			clsCmbObras ob = await cargaDatosObra();

			if (ob != null) {
				loadPop.Hide();
				txtNombreObra.Text = ob.nombre;
				txtDescripcionobra.Text = ob.descripcion;
			}

			btnModificarObra.TouchUpInside += modificaObra;
			btnEliminarObra.TouchUpInside += BorrarObra;

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtDescripcionobra.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtDescripcionobra.InputAccessoryView = toolbar;


			txtNombreObra.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };



		}

		public async Task<clsCmbObras> cargaDatosObra()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de la Obra...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/getObraById";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idobra", idobra.ToString());

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
				return null;
			}

				
			

			var jsonresponse = JObject.Parse(responseString);

			var result = jsonresponse["result"];

			if (result != null)
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			clsCmbObras obj = new clsCmbObras();
			obj.idobra = Int32.Parse(jsonresponse["idobra"].ToString());
			obj.nombre = jsonresponse["nombre"].ToString();
			obj.descripcion = jsonresponse["descripcion"].ToString();


			return obj;

		}

		async void modificaObra(object sender, EventArgs e)
		{
			if (txtNombreObra.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la obra no puede ser vacio, verifiquelo por favor");
				return;
			}


			Boolean resp = await modObra();

			if (resp)
			{
				((ObrasController)viewobras).recargarListado();
				this.NavigationController.PopToViewController(viewobras, true);
			}
		}

		public async Task<Boolean> modObra()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Obra...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/ModificaObra";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idobra", idobra.ToString());
			pet.Add("nombre", txtNombreObra.Text);
			pet.Add("descripcion", txtDescripcionobra.Text);

			var json = JsonConvert.SerializeObject(pet);
			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
				return false;
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
			funciones.MessageBox("Aviso", "Se ha guardado la obra!!!");
			return true;

		}

		async void BorrarObra(object sender, EventArgs e)
		{
			int resp = await funciones.MessageBoxCancelOk("Aviso", "Esta seguro de borrar la obra");
			if(resp == 0)
			{				
				return;
			}

			Boolean respborr = await borrObra();

			if (respborr)
			{
				((ObrasController)viewobras).recargarListado();
				this.NavigationController.PopToViewController(viewobras, true);
			}
		}

		public async Task<Boolean> borrObra()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Eliminando Obra...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/EliminarObra";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idobra", idobra.ToString());

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

			var strresult = result.ToString();

			if (strresult.Equals("0") || strresult.Equals("2"))
			{
				loadPop.Hide();
				string error = jsonresponse["error"].ToString();
				funciones.MessageBox("Error", error);
				return false;
			}



			loadPop.Hide();
			funciones.MessageBox("Aviso", "Se elimino la obra");
			return true;

		}

		double ajuste = 150;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtDescripcionobra.IsFirstResponder)
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


