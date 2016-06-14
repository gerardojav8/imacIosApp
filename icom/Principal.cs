using System;

using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using icom.Entidades;
using System.Text;
using Newtonsoft.Json;
namespace icom
{
	public partial class Principal : UIViewController
	{
		public Principal () : base ("Principal", null)
		{
		}

		HttpClient client;

		public string strusuario{ get; set; }

		public string strpass{ get; set; }

		public string token { get; set;}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnMaquinaria.TouchUpInside += delegate {
				//Maquinaria viewmaq = new Maquinaria();
				//viewmaq.Title = "Maquinaria";

				MaquinasController viewmaq = new MaquinasController();

				this.NavigationController.PushViewController(viewmaq, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnProduccion.TouchUpInside += delegate {
				Produccion viewprod = new Produccion();
				viewprod.Title = "Produccion";


				this.NavigationController.PushViewController(viewprod, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnCtrlObra.TouchUpInside += delegate {
				CtrlObra viewctrlobra = new CtrlObra();
				viewctrlobra.Title = "Control de Obra";


				this.NavigationController.PushViewController(viewctrlobra, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnInformacion.TouchUpInside += delegate {
				Informacion viewinfo = new Informacion();
				viewinfo.Title = "Informacion";


				this.NavigationController.PushViewController(viewinfo, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnCerrarSesion.TouchUpInside += delegate {
				this.NavigationController.PopToRootViewController(true);
			};

			Boolean resp = await TraeUsuario();

			if (!resp)
			{
				this.NavigationController.PopToRootViewController(true);
			}



		}

		public async Task<Boolean> TraeUsuario()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "usuarios/getUsuarioByuserAndpass";
			var uri = new Uri(string.Format(Consts.urltoken));

			LoginUsuario objlog = new LoginUsuario();
			objlog.usuario = strusuario;
			objlog.pass = strpass;
			var json = JsonConvert.SerializeObject(objlog);

			var content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Headers.Clear();
			try
			{
				//request.Headers.Add("Content-Type", "application/json");
				request.Headers.Add("Authorization", "Bearer " + token);
			}
			catch (Exception e)
			{
				funciones.MessageBox("Error", e.ToString());
			}


			request.Content = content;

			HttpResponseMessage response = null;

			try
			{
				response = await client.SendAsync(request);
			}
			catch (Exception e)
			{
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return false;
			}

			if (response == null)
			{
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			responseString = responseString.Replace("\\", "");
			responseString = responseString.Substring(1, responseString.Length-2);
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			string nombre = jsonresponse["nombre"].ToString();
			string apepaterno = jsonresponse["apepaterno"].ToString();
			string apematerno = jsonresponse["apematerno"].ToString();
			lblUsuario.Text = nombre + " " + apepaterno + " " + apematerno;
			return true;
		}



		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


