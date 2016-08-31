using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using System.Text;
using Newtonsoft.Json;
using System.Json;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public partial class MaquinasController : UIViewController
	{
		public MaquinasController() : base("MaquinasController", null)
		{
		}

		LoadingOverlay loadPop;
		HttpClient client;
		private List<clsListadoMaquinas> lstMaqServ;


		public async override void ViewDidLoad()
		{

			base.ViewDidLoad();

			lstMaqServ = new List<clsListadoMaquinas>();
			lstMaquinas.Source = new FuenteTablaMaquinas(this, lstMaqServ);

			/*Boolean resp = await getAllMaquinas();

			if (resp)
			{
				loadPop.Hide();
				lstMaquinas.ReloadData();
			}*/

			clsListadoMaquinas obj1 = new clsListadoMaquinas();
			obj1.noserie = "1234568";
			obj1.noeconomico = 1234;
			obj1.marca = "Mercedes venz";
			obj1.modelo = 1234;
			obj1.IdTipoMaquina = 1;
			obj1.tieneReporte = 1;


			clsListadoMaquinas obj2 = new clsListadoMaquinas();
			obj2.noserie = "45678";
			obj2.noeconomico = 6789;
			obj2.marca = "Toyota";
			obj2.modelo = 3654;
			obj2.IdTipoMaquina = 2;
			obj2.tieneReporte = 0;


			clsListadoMaquinas obj3 = new clsListadoMaquinas();
			obj3.noserie = "987654";
			obj3.noeconomico = 9871;
			obj3.marca = "Volvo";
			obj3.modelo = 8798;
			obj3.IdTipoMaquina = 3;
			obj3.tieneReporte = 1;


			lstMaqServ.Add(obj1);
			lstMaqServ.Add(obj2);
			lstMaqServ.Add(obj3);

			btnAgregar.TouchUpInside += delegate {
				solicitudMaquinaController viewsolmaq = new solicitudMaquinaController();
				viewsolmaq.Title = "Solicitud de Maquinaria";
				viewsolmaq.viewmaq = this;

				this.NavigationController.PushViewController(viewsolmaq, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnSearch.TouchUpInside += delegate
			{				
				txtSearch.EndEditing(true);
				buscarMaquinas();
			};

			bajatecladoinputs();

		}

		private void bajatecladoinputs() { 

			txtSearch.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};
		}

		public async void recargarListado() {
			
			lstMaqServ = new List<clsListadoMaquinas>();
			lstMaquinas.Source = new FuenteTablaMaquinas(this, lstMaqServ);
			Boolean resp = await getAllMaquinas();

			if (resp)
			{
				loadPop.Hide();
				lstMaquinas.ReloadData();
			}
		}

		public async void buscarMaquinas() {
			if (txtSearch.Text.Equals(""))
			{
				recargarListado();
			}
			else {
				lstMaqServ = new List<clsListadoMaquinas>();
				Boolean resp = await getMaquinasBusqueda(txtSearch.Text);
				if (resp) {
					loadPop.Hide();
					lstMaquinas.ReloadData();
				}
			}
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public async Task<Boolean> getAllMaquinas()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Maquinas ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getListadoMaquinas";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI "+ e.HResult );
				return false;
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI ");
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer maquinas del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}




			foreach(var maquina in jrarray)
			{
				clsListadoMaquinas objm = getobjMaquina(maquina);
				lstMaqServ.Add(objm);
			}


			return true;
		}

		public async Task<Boolean> getMaquinasBusqueda(String strbusqueda)
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Maquinas ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getListadoMaquinasBusqueda";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> obj = new Dictionary<string, string>();
			obj.Add("busqueda", strbusqueda);
			var json = JsonConvert.SerializeObject(obj);

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
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI ");
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();

				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer maquinas del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}




			foreach (var maquina in jrarray)
			{
				clsListadoMaquinas objm = getobjMaquina(maquina);
				lstMaqServ.Add(objm);
			}


			return true;
		}

		public clsListadoMaquinas getobjMaquina(Object varjson)
		{
			clsListadoMaquinas objmaq = new clsListadoMaquinas();
			JObject json = (JObject)varjson;

			objmaq.noserie = json["noserie"].ToString();
			objmaq.noeconomico = Int32.Parse(json["noeconomico"].ToString());
			objmaq.marca = json["marca"].ToString();
			objmaq.modelo = Int32.Parse(json["modelo"].ToString());
			objmaq.IdTipoMaquina = Int32.Parse(json["idtipomaquina"].ToString());
			objmaq.tieneReporte = Int32.Parse(json["tieneReporte"].ToString());


			return objmaq;
		}
	}


}


