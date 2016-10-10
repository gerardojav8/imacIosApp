using System;

using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using icom.Entidades;
using System.Text;
using Newtonsoft.Json;
using SlideDownMenu;
using System.Collections.Generic;
namespace icom
{
	public partial class Principal : UIViewController
	{
		public Principal () : base ("Principal", null)
		{
		}
		HttpClient client;
		LoadingOverlay loadPop;
		Boolean blnmenuactivo = false;
		SlideMenu sldmnu;


		public string strusuario{ get; set; }

		public string strpass{ get; set; }

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var item0 = new MenuItem("Cerrar Sesion", UIImage.FromBundle("Images/a0.png"), (menuItem) =>
			{				
				funciones.SalirSesion(this);
			});
			item0.Tag = 0;


			var item1 = new MenuItem("Cambiar Password", UIImage.FromBundle("Images/a0.png"), (menuItem) =>
			{
				cambioPass viewcam = new cambioPass();

				this.NavigationController.PushViewController(viewcam, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();

				sldmnu.ToggleMenu();
				sldmnu.Hidden = true;
				blnmenuactivo = false;

			});
			item1.Tag = 1;

			sldmnu = new SlideMenu(new List<MenuItem> { item0, item1 }, new CoreGraphics.CGPoint(0, 110));
			sldmnu.AutosizesSubviews = true;

			this.View.AddSubview(sldmnu);

			btnCerrarSesion.TouchUpInside += delegate
			{
				if (!blnmenuactivo)
				{
					sldmnu.Hidden = false;
					sldmnu.ToggleMenu();
					blnmenuactivo = true;
				}
				else { 
					sldmnu.ToggleMenu();
					sldmnu.Hidden = true;
					blnmenuactivo = false;
				}
			};



			btnMaquinaria.TouchUpInside += delegate {
				

				MaquinasController viewmaq = new MaquinasController();

				this.NavigationController.PushViewController(viewmaq, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnProduccion.TouchUpInside += delegate {
				FiltroProduccionController viewprod = new FiltroProduccionController();
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


			/*if (strusuario.ToLower().Equals("fermin"))
			{
				Consts.idusuarioapp = "1";
				Consts.nombreusuarioapp = "Fermin Mojica Araujo";
				Consts.inicialesusuarioapp = "FM";
			}
			else if (strusuario.ToLower().Equals("evelyne")) { 
				Consts.idusuarioapp = "2";
				Consts.nombreusuarioapp = "Evelyne";
				Consts.inicialesusuarioapp = "E";
			}else if (strusuario.ToLower().Equals("enith"))
			{
				Consts.idusuarioapp = "3";
				Consts.nombreusuarioapp = "Enith";
				Consts.inicialesusuarioapp = "EN";
			}else
			{
				Consts.idusuarioapp = "4";
				Consts.nombreusuarioapp = "Gerardo Javier Gamez Vazquez";
				Consts.inicialesusuarioapp = "GG";
			}*/



			Boolean resp = await TraeUsuario();

			if (!resp)
			{
				this.NavigationController.PopToRootViewController(true);
			}
			else {
				loadPop.Hide();
			}


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

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2")) {
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

			string nombre = jsonresponse["nombre"].ToString();
			string apepaterno = jsonresponse["apepaterno"].ToString();
			string apematerno = jsonresponse["apematerno"].ToString();
			btnCerrarSesion.SetTitle(nombre + " " + apepaterno + " " + apematerno, UIControlState.Normal);
			Consts.idusuarioapp = jsonresponse["idusuario"].ToString();
			Consts.nombreusuarioapp = nombre + " " + apepaterno + " " + apematerno;
			Consts.inicialesusuarioapp = nombre.Substring(0, 1) + apepaterno.Substring(0, 1);

			return true;
		}



		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


