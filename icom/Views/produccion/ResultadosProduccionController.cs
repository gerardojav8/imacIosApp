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
	public partial class ResultadosProduccionController : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;
		private List<clsProduccion> lstProd;
		public string folio { get; set; }
		public string material { get; set; }
		public string cantidad { get; set; }
		public string unidad { get; set; }
		public string cliente { get; set; }
		public string fechaini { get; set; }
		public string fechafin { get; set; }
		public ResultadosProduccionController() : base("ResultadosProduccionController", null)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstProd = new List<clsProduccion>();
			tblProduccion.Source = new FuenteTablaProduccion(this, lstProd);

			Boolean resp = await getBusquedaProduccion();

			if (resp)
			{
				loadPop.Hide();
				tblProduccion.ReloadData();
			}

			/*clsProduccion obj = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "24.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			clsProduccion obj1 = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "875424.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			clsProduccion obj2 = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "724.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			lstProd.Add(obj);
			lstProd.Add(obj1);
			lstProd.Add(obj2);*/
		}

		public async Task<Boolean> getBusquedaProduccion()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Datos ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "reportes/getReporteProduccion";
			var uri = new Uri(string.Format(url));
			Dictionary<string, string> pet = new Dictionary<string, string>();

			pet.Add("folio", folio);
			pet.Add("material", material);
			pet.Add("cantidad", cantidad);
			pet.Add("unidad", unidad);
			pet.Add("cliente", cliente);
			pet.Add("fecha", fechaini);
			pet.Add("fechafin", fechafin);
			var json = JsonConvert.SerializeObject(pet);

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return false;
			}

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

				string mensaje = "error al traer datos del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var item in jrarray)
			{
				clsProduccion objp = getobjProduccion(item);
				lstProd.Add(objp);
			}


			return true;
		}

		private clsProduccion getobjProduccion(Object varjson) {

			clsProduccion obj = new clsProduccion();
			JObject json = (JObject)varjson;
			obj.folio = json["folio"].ToString();
			obj.material = json["material"].ToString();
			obj.cantidad = json["cantidad"].ToString();
			obj.unidad = json["unidad"].ToString();
			obj.cliente = json["cliente"].ToString();
			obj.fecha = json["fecha"].ToString();
			return obj;
		}


	}


}

