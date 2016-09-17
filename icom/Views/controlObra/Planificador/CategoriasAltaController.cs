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
	public partial class CategoriasAltaController : UIViewController
	{
		public CategoriasAltaController() : base("CategoriasAltaController", null)
		{
		}


		public UIViewController viewcategorias { get; set; }
		public int idobra{ get; set; }
		LoadingOverlay loadPop;
		HttpClient client;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtComentarios.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentarios.Layer.BorderWidth = (nfloat)2.0;
			txtComentarios.Text = "";

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtComentarios.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtComentarios.InputAccessoryView = toolbar;


			txtNombreCategoria.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		async void guardaCategoria(object sender, EventArgs e)
		{
			if (txtNombreCategoria.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la categoria no puede ser vacio, verifiquelo por favor");
				return;
			}


			Boolean resp = await saveCategoria();

			if (resp)
			{
				((CategoriasTareasController)viewcategorias).recargarListado();
				this.NavigationController.PopToViewController(viewcategorias, true);
			}
		}

		public async Task<Boolean> saveCategoria()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Categoria...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/NuevaCategoria";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idobra", idobra.ToString());
			pet.Add("idusuario", Consts.idusuarioapp);
			pet.Add("nombre", txtNombreCategoria.Text);
			pet.Add("comentarios", txtComentarios.Text);

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


			funciones.MessageBox("Aviso", "Se ha guardado la categoria!!!");
			return true;

		}

		double ajuste = 160;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtComentarios.IsFirstResponder)
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


