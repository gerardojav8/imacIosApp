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

namespace icom
{
	public partial class cambioPass : UIViewController
	{
		public cambioPass() : base("cambioPass", null)
		{
		}

		LoadingOverlay loadPop;
		HttpClient client;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			btnAceptar.TouchUpInside += cambiaPass;

			bajatecladoinputs();

		}

		async void cambiaPass(object sender, EventArgs e)
		{
			if (txtpassactual.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar el password actual");
				return;
			}
			
			if (txtnuevopass.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar un password nuevo");
				return;
			}

			if (txtconfirmpass.Text == "")
			{
				funciones.MessageBox("Error", "Debe de confirmar el password nuevo");
				return;
			}

			if (!txtnuevopass.Text.Equals(txtconfirmpass.Text))
			{
				funciones.MessageBox("Error", "El nuevo password no coincide con la confirmacion, verifiquelo por favor");
				return;
			}

			Boolean resp = await changepass();

			if (resp)
			{
				funciones.SalirSesion(this);
			}
		}

		private void bajatecladoinputs()
		{
			
			txtnuevopass.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtpassactual.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtconfirmpass.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		public async Task<Boolean> changepass()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cambiando password...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "common/cambiarPass";
			var uri = new Uri(string.Format(url));

			Dictionary<String, String> pet = new Dictionary<String, String>();
			pet.Add("idusuario", Consts.idusuarioapp);
			pet.Add("newpass", txtnuevopass.Text);
			pet.Add("oldpass", txtpassactual.Text);

			var json = JsonConvert.SerializeObject(pet);

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return false;
			}

			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			loadPop.Hide();
			funciones.MessageBox("Aviso", "Se ha cambiado tu password");
			return true;
		}
	}
}

