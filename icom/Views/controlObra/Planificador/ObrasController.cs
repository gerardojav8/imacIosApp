using System;
using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using Foundation;
using icom.globales;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace icom
{
	public partial class ObrasController : UIViewController
	{
		public ObrasController() : base("ObrasController", null)
		{
		}

		LoadingOverlay loadPop;
		HttpClient client;

		private List<clsListadoObra> lstObras;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstObras = new List<clsListadoObra>();
			tblObras.Source = new FuenteTablaObras(this, lstObras);

			clsListadoObra obj1 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 1",
				porcentajeavance = 25,
				noclasificacioens = 3
			};

			clsListadoObra obj2 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 2",
				porcentajeavance = 98,
				noclasificacioens = 3
			};

			clsListadoObra obj3 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 3",
				porcentajeavance = 75,
				noclasificacioens = 3
			};

			clsListadoObra obj4 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 4",
				porcentajeavance = 50,
				noclasificacioens = 3
			};

			lstObras.Add(obj1);
			lstObras.Add(obj2);
			lstObras.Add(obj3);
			lstObras.Add(obj4);

			/*Boolean resp = await GetObras();
			if (resp)
			{
				loadPop.Hide();
				tblObras.ReloadData();
			}*/



			btnModificarObra.TouchUpInside += delegate {
				NSIndexPath indexPath = tblObras.IndexPathForSelectedRow;
				if (indexPath != null){
					ModificarObraController viewmodobra = new ModificarObraController();


					viewmodobra.Title = "Modifica Obra";
					this.NavigationController.PushViewController(viewmodobra, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
					UIView.CommitAnimations();
				}else {
					funciones.MessageBox("Aviso", "Ninguna celda seleccionada");
				}

				tblObras.DeselectRow(indexPath, true);
			};

			btnNuevaObra.TouchUpInside += delegate {
				AltaObraController viewaobras = new AltaObraController();


				viewaobras.Title = "Nueva Obra";
				this.NavigationController.PushViewController(viewaobras, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnBusquedaObra.TouchUpInside += buscarObras;

		}

		async void buscarObras(object sender, EventArgs e)
		{

			Boolean resp;
			if (txtBusquedaObra.Equals(""))
				resp = await GetObras();
			else
				resp = await buscaObras();

			if (resp)
			{
				recargarListado();
			}
		}

		public async Task<Boolean> GetObras()
		{

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando datos de obras...");
			View.Add(loadPop);
			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);
			string url = Consts.ulrserv + "controldeobras/getObrasListado";
			var uri = new Uri(string.Format(url));
			String json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1")) { 
				funciones.SalirSesion(this);
			}

			JArray jrarray;
			try
			{
				var objectjson = JObject.Parse(responseString);

				var result = objectjson["result"];
				if (result != null) { 					
					loadPop.Hide();
					if (result.ToString().Equals("0")) { funciones.MessageBox("Aviso", "No existen obras actualmente"); } 
					else { funciones.MessageBox("Error", objectjson["error"].ToString()); }
					return false;	
				}

				jrarray = JArray.Parse(objectjson["obras"].ToString());
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);
				string mensaje = "al transformar el arreglo" + e.HResult;
				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}
				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var obra in jrarray)
			{
				clsListadoObra objob = getObjObra(obra);
				lstObras.Add(objob);
			}
			return true;
		}



		public clsListadoObra getObjObra(Object varjson)
		{
			
			JObject json = (JObject)varjson;
			clsListadoObra objob = new clsListadoObra
			{
				idobra = Int32.Parse(json["idobra"].ToString()),
				nombre = json["nombre"].ToString(),
				porcentajeavance = Double.Parse(json["porcentaje"].ToString()),
				noclasificacioens = Int32.Parse(json["noclasificaciones"].ToString())
			};

			return objob;
		}

		public async void recargarListado()
		{

			lstObras = new List<clsListadoObra>();
			tblObras.Source = new FuenteTablaObras(this, lstObras);
			Boolean resp = await GetObras();

			if (resp)
			{
				loadPop.Hide();
				tblObras.ReloadData();
			}
		}

		public async Task<Boolean> buscaObras()
		{

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando obras...");
			View.Add(loadPop);
			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);
			string url = Consts.ulrserv + "controldeobras/BuscaObras";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> pet = new Dictionary<string, string>();
			pet.Add("strBusqueda", txtBusquedaObra.Text);
			var json = JsonConvert.SerializeObject(pet);

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);


			if (responseString.Equals("-1"))
			{
				funciones.SalirSesion(this);
			}

			JArray jrarray;
			try
			{
				var objectjson = JObject.Parse(responseString);

				var result = objectjson["result"];
				if (result != null)
				{
					loadPop.Hide();
					if (result.ToString().Equals("0")) { funciones.MessageBox("Aviso", "No existen obras actualmente"); }
					else { funciones.MessageBox("Error", objectjson["error"].ToString()); }
					return false;
				}

				jrarray = JArray.Parse(objectjson["obras"].ToString());
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);
				string mensaje = "al transformar el arreglo" + e.HResult;
				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}
				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var obra in jrarray)
			{
				clsListadoObra objob = getObjObra(obra);
				lstObras.Add(objob);
			}
			return true;
		}
	}
}


