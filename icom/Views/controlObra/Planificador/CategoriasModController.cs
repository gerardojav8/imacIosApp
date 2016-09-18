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
	public partial class CategoriasModController : UIViewController
	{
		public CategoriasModController() : base("CategoriasModController", null)
		{
		}

		public UIViewController viewcategorias { get; set; }
		public int idcategoria { get; set; }
		LoadingOverlay loadPop;
		HttpClient client;


		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtComentario.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentario.Layer.BorderWidth = (nfloat)2.0;
			txtComentario.Text = "";

			bajatecladoinputs();

			Dictionary<string, string> resp = await cargaDatosCategoria();
			if (resp != null) {
				loadPop.Hide();
				txtNombreCategoria.Text = resp["nombre"];
				txtComentario.Text = resp["comentarios"];
			}

			btnGuardar.TouchUpInside += modificaCategoria;
			btnEliminar.TouchUpInside += BorrarCategoria;
		}

		public async Task<Dictionary<string, string>> cargaDatosCategoria()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de la Categoria...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/getCategoriaById";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idcategoria", idcategoria.ToString());

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


			Dictionary<string, string> resp = new Dictionary<string, string>();
			resp.Add("nombre", jsonresponse["nombre"].ToString());
			resp.Add("comentarios", jsonresponse["comentario"].ToString());


			return resp;

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtComentario.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtComentario.InputAccessoryView = toolbar;


			txtNombreCategoria.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		async void modificaCategoria(object sender, EventArgs e)
		{
			if (txtNombreCategoria.Equals(""))
			{
				funciones.MessageBox("Error", "El nombre de la Categoria no puede ser vacio, verifiquelo por favor");
				return;
			}


			Boolean resp = await modCategoria();

			if (resp)
			{
				((CategoriasTareasController)viewcategorias).recargarListado();
				this.NavigationController.PopToViewController(viewcategorias, true);
			}
		}

		public async Task<Boolean> modCategoria()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Categoria...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/ModificaCategoria";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idcategoria", idcategoria.ToString());
			pet.Add("idusuario", Consts.idusuarioapp);
			pet.Add("nombre", txtNombreCategoria.Text);
			pet.Add("comentarios", txtComentario.Text);

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
			funciones.MessageBox("Aviso", "Se ha guardado la Categoria!!!");
			return true;

		}

		async void BorrarCategoria(object sender, EventArgs e)
		{
			int resp = await funciones.MessageBoxCancelOk("Aviso", "Esta seguro de borrar la Categoria");
			if (resp == 0)
			{
				return;
			}

			Boolean respborr = await borrCat();

			if (respborr)
			{
				((CategoriasTareasController)viewcategorias).recargarListado();
				this.NavigationController.PopToViewController(viewcategorias, true);
			}
		}

		public async Task<Boolean> borrCat()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Eliminando Categoria...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "controldeobras/EliminarCategoria";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idcategoria", idcategoria.ToString());

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
			funciones.MessageBox("Aviso", "Se elimino la Categoria");
			return true;

		}

		double ajuste = 160;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtComentario.IsFirstResponder)
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