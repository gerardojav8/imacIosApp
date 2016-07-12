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
		LoadingOverlay loadPop;

		public string strusuario{ get; set; }

		public string strpass{ get; set; }


		public override void ViewDidLoad ()
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

			/*Boolean resp = await TraeUsuario();

			if (!resp)
			{
				this.NavigationController.PopToRootViewController(true);
			}
			else {
				loadPop.Hide();
			}*/




		}

		public async Task<Boolean> TraeUsuario()
		{

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de usuario...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "usuarios/getUsuarioByuserAndpass";
			var uri = new Uri(string.Format(url));

			LoginUsuario objlog = new LoginUsuario();
			objlog.usuario = strusuario;
			objlog.pass = strpass;
			var json = JsonConvert.SerializeObject(objlog);

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


