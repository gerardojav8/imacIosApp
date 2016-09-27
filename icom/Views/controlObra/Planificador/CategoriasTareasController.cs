using System;

using System.Collections.Generic;
using UIKit;
using System.Threading.Tasks;
using Foundation;
using icom.globales;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace icom
{
	public partial class CategoriasTareasController : UIViewController
	{
		public int idobra { get; set; }
		public string nombreobra { get; set; }
		LoadingOverlay loadPop;
		HttpClient client;

		List<clsClasificacion> lstClas;
		List<int> arrnumusados;

		public CategoriasTareasController() : base("CategoriasTareasController", null)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstClas = new List<clsClasificacion>();

			/*clsClasificacion obj1 = new clsClasificacion
			{
				idclasificacion = 1,
				nombre = "Clasificacion 1",
				porcentaje = 50.0f,
				color = UIColor.Blue
			};

			clsClasificacion obj2 = new clsClasificacion
			{
				idclasificacion = 2,
				nombre = "Clasificacion 2",
				porcentaje = 80.0f,
				color = UIColor.Gray
			};

			clsClasificacion obj3 = new clsClasificacion
			{
				idclasificacion = 3,
				nombre = "Clasificacion 3",
				porcentaje = 24.0f,
				color = UIColor.Green
			};

			clsClasificacion obj4 = new clsClasificacion
			{
				idclasificacion = 4,
				nombre = "Clasificacion 4",
				porcentaje = 50.0f,
				color = UIColor.Orange
			};

			clsClasificacion obj5 = new clsClasificacion
			{
				idclasificacion = 5,
				nombre = "Clasificacion 5",
				porcentaje = 15.0f,
				color = UIColor.Red
			};



			lstClas.Add(obj1);
			lstClas.Add(obj2);
			lstClas.Add(obj3);
			lstClas.Add(obj4);
			lstClas.Add(obj5);*/

			tblCategorias.Source = new FuenteTablaClasificaciones(this, lstClas);

			Boolean resp = await GetCategorias();
			if (resp) { 
				
				tblCategorias.ReloadData();
			}

			tblCategorias.SeparatorColor = UIColor.Gray;
			tblCategorias.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;


			btnNuevaCategoria.TouchUpInside += delegate {
				CategoriasAltaController viewca = new CategoriasAltaController();
				viewca.idobra = idobra;
				viewca.viewcategorias = this;
				viewca.Title = "Nueva Clasificacion";
				this.NavigationController.PushViewController(viewca, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnModificarCategoria.TouchUpInside += delegate {
				NSIndexPath indexPath = tblCategorias.IndexPathForSelectedRow;
				if (indexPath != null)
				{
					CategoriasModController viewcm = new CategoriasModController();
					viewcm.idcategoria = lstClas.ElementAt(indexPath.Row).idclasificacion;
					viewcm.viewcategorias = this;
					viewcm.Title = "Modificar Clasificacion";
					this.NavigationController.PushViewController(viewcm, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.NavigationController.View, true);
					UIView.CommitAnimations();
				}
				else {
					funciones.MessageBox("Aviso", "Ninguna celda seleccionada");
				}

				tblCategorias.DeselectRow(indexPath, true);
			};

			btnGrafica.TouchUpInside += delegate {
				GraficasTareasController viewg = new GraficasTareasController();
				viewg.lstClas = lstClas;
				viewg.nombreobra = this.nombreobra;

				viewg.Title = "Graficas";
				this.NavigationController.PushViewController(viewg, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();	
			};

			btnBusquedaCategoria.TouchUpInside += buscarCategorias;

			bajatecladoinputs();
		}

		private void bajatecladoinputs()
		{

			txtbusquedaCategoria.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
		}

		async void buscarCategorias(object sender, EventArgs e)
		{
			lstClas = new List<clsClasificacion>();
			Boolean resp;
			if (txtbusquedaCategoria.Equals(""))
				resp = await GetCategorias();
			else
				resp = await searchCategorias();

			if (resp)
			{
				loadPop.Hide();
				tblCategorias.Source = new FuenteTablaClasificaciones(this, lstClas);
				tblCategorias.ReloadData();
			}
			else { 
				tblCategorias.Source = new FuenteTablaClasificaciones(this, lstClas);
				tblCategorias.ReloadData();
			}
		}

		public async Task<Boolean> GetCategorias()
		{

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Cargando Categorias...");
			View.Add(loadPop);
			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);
			string url = Consts.ulrserv + "controldeobras/getCategoriasListado";
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

			JArray jrarray;
			try
			{
				var objectjson = JObject.Parse(responseString);

				var result = objectjson["result"];
				if (result != null)
				{
					loadPop.Hide();
					if (result.ToString().Equals("0")) { funciones.MessageBox("Aviso", "No existen categorias actualmente"); }
					else { funciones.MessageBox("Error", objectjson["error"].ToString()); }
					return false;
				}

				jrarray = JArray.Parse(objectjson["categorias"].ToString());
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

			arrnumusados = new List<int>();

			foreach (var obra in jrarray)
			{
				clsClasificacion objob = getObjClas(obra);
				lstClas.Add(objob);
			}
			loadPop.Hide();
			return true;
		}

		public async Task<Boolean> searchCategorias()
		{

			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Categorias...");
			View.Add(loadPop);
			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);
			string url = Consts.ulrserv + "controldeobras/BuscarCategorias";
			var uri = new Uri(string.Format(url));
			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("idobra", idobra.ToString());
			pet.Add("strBusqueda", txtbusquedaCategoria.Text);

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
					if (result.ToString().Equals("0")) { funciones.MessageBox("Aviso", "Sin categorias para mostrar"); }
					else { funciones.MessageBox("Error", objectjson["error"].ToString()); }
					return false;
				}

				jrarray = JArray.Parse(objectjson["categorias"].ToString());
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

			arrnumusados = new List<int>();

			foreach (var obra in jrarray)
			{
				clsClasificacion objob = getObjClas(obra);
				lstClas.Add(objob);
			}
			loadPop.Hide();
			return true;
		}


		private clsClasificacion getObjClas(Object varjson)
		{
			int indicecolor = funciones.getNumeroAleatorioSinRepetir(0,9, arrnumusados);

			JObject json = (JObject)varjson;
			clsClasificacion objob = new clsClasificacion
			{
				idclasificacion = Int32.Parse(json["idcategoria"].ToString()),
				nombre = json["nombre"].ToString(),
				porcentaje = float.Parse(json["porcentaje"].ToString()),
				notareas = Int32.Parse(json["notareas"].ToString()),
				color = Consts.colores[indicecolor],
				strcolor = Consts.strcolores[indicecolor]
			};

			arrnumusados.Add(indicecolor);
			return objob;
		}

		public async void recargarListado()
		{

			lstClas = new List<clsClasificacion>();
			tblCategorias.Source = new FuenteTablaClasificaciones(this, lstClas);
			Boolean resp = await GetCategorias();

			if (resp)
			{
				loadPop.Hide();
				tblCategorias.ReloadData();
			}
		}
	}
}


