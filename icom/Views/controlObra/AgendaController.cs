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
	public partial class AgendaController : UIViewController
	{

		LoadingOverlay loadPop;
		HttpClient client;

		List<clsAgenda> LstDatosAgenda;


		public AgendaController() : base("AgendaController", null)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();


			LstDatosAgenda = new List<clsAgenda>();
			lstAgenda.Source = new FuenteTablaAgenda(this, LstDatosAgenda);




			btnNuevoEvento.TouchUpInside += delegate {
				NuevoEventoController viewne = new NuevoEventoController();
				viewne.Title = "Nuevo Evento";
				viewne.viewagenda = this;

				this.NavigationController.PushViewController(viewne, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			Boolean resp = await getAgenda();

			if (resp)
			{
				loadPop.Hide();
				lstAgenda.ReloadData();
			}

			/*clsAgenda obj1 = new clsAgenda();
			obj1.mes = 1;
			obj1.comentario = "";

			List<clsEventoAgenda> lste1 = new List<clsEventoAgenda>();

			clsEventoAgenda e11 = new clsEventoAgenda();
			e11.idevento = 1;
			e11.dia = 15;
			e11.comentario = "Reunion supervision";
			e11.lapso = "11:00 am - 12:00 pm";

			clsEventoAgenda e12 = new clsEventoAgenda();
			e12.idevento = 2;
			e12.dia = 25;
			e12.comentario = "Junta planeacion";
			e12.lapso = "1:00 am - 2:00 pm";

			lste1.Add(e11);
			lste1.Add(e12);



			obj1.lstEventos = lste1;

			clsAgenda obj2 = new clsAgenda();
			obj2.mes = 2;
			obj2.comentario = "";

			List<clsEventoAgenda> lste2 = new List<clsEventoAgenda>();

			clsEventoAgenda e21 = new clsEventoAgenda();
			e21.idevento = 3;
			e21.dia = 17;
			e21.comentario = "Reunion supervision";
			e21.lapso = "11:00 am - 12:00 pm";
			lste2.Add(e21);

			obj2.lstEventos = lste2;

			clsAgenda obj3 = new clsAgenda();
			obj3.mes = 3;
			obj3.comentario = "";

			clsAgenda obj4 = new clsAgenda();
			obj4.mes = 4;
			obj4.comentario = "";

			clsAgenda obj5 = new clsAgenda();
			obj5.mes = 5;
			obj5.comentario = "";

			clsAgenda obj6 = new clsAgenda();
			obj6.mes = 6;
			obj6.comentario = "";

			clsAgenda obj7 = new clsAgenda();
			obj7.mes = 7;
			obj7.comentario = "";

			clsAgenda obj8 = new clsAgenda();
			obj8.mes = 8;
			obj8.comentario = "";


			List<clsEventoAgenda> lste8 = new List<clsEventoAgenda>();
			clsEventoAgenda e81 = new clsEventoAgenda();
			e81.idevento = 4;
			e81.dia = 15;
			e81.comentario = "Evento en agosto";
			e81.lapso = "11:00 am - 12:00 pm";

			clsEventoAgenda e82 = new clsEventoAgenda();
			e82.idevento = 5;
			e82.dia = 25;
			e82.comentario = "Segundo evento en agosto";
			e82.lapso = "1:00 am - 2:00 pm";

			lste8.Add(e81);
			lste8.Add(e82);
			obj8.lstEventos = lste8;


			clsAgenda obj9 = new clsAgenda();
			obj9.mes = 9;
			obj9.comentario = "";

			clsAgenda obj10 = new clsAgenda();
			obj10.mes = 10;
			obj10.comentario = "";

			clsAgenda obj11 = new clsAgenda();
			obj11.mes = 11;
			obj11.comentario = "";

			clsAgenda obj12 = new clsAgenda();
			obj12.mes = 12;
			obj12.comentario = "";

			LstDatosAgenda.Add(obj1);
			LstDatosAgenda.Add(obj2);
			LstDatosAgenda.Add(obj3);
			LstDatosAgenda.Add(obj4);
			LstDatosAgenda.Add(obj5);
			LstDatosAgenda.Add(obj6);
			LstDatosAgenda.Add(obj7);
			LstDatosAgenda.Add(obj8);
			LstDatosAgenda.Add(obj9);
			LstDatosAgenda.Add(obj10);
			LstDatosAgenda.Add(obj11);
			LstDatosAgenda.Add(obj12);*/

			DateTime dthoy = DateTime.Now;
			int mesact = dthoy.Month - 1;
			NSIndexPath myindex = NSIndexPath.FromItemSection(mesact, 0);

			lstAgenda.Source.RowSelected(lstAgenda, myindex);

			var poss = UITableViewScrollPosition.Middle;
			lstAgenda.ScrollToRow(myindex, poss, true);


		}

		public async void recargarListadoAgenda()
		{

			LstDatosAgenda = new List<clsAgenda>();
			lstAgenda.Source = new FuenteTablaAgenda(this, LstDatosAgenda);
			Boolean resp = await getAgenda();

			if (resp)
			{
				loadPop.Hide();
				lstAgenda.ReloadData();
			}

		}

		public async Task<Boolean> getAgenda()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Agenda ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getListadoAgenda";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> obj = new Dictionary<string, string>();
			obj.Add("idusuario", Consts.idusuarioapp);
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

				string mensaje = "error al traer los mensajes del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}




			foreach (var jsonag in jrarray)
			{
				clsAgenda objag = getobjAgenda(jsonag);
				LstDatosAgenda.Add(objag);
			}


			return true;
		}

		private clsAgenda getobjAgenda(Object varjson) {
			clsAgenda obj = new clsAgenda();
			JObject json = (JObject)varjson;

			obj.mes = Int32.Parse(json["mes"].ToString());
			obj.comentario = "";

			List<clsEventoAgenda> lste = new List<clsEventoAgenda>();
			JArray jrarray;
			try
			{
				var jsoneventos = JArray.Parse(json["eventos"].ToString());
				jrarray = jsoneventos;
			}
			catch (Exception e){
				Console.WriteLine(e.ToString());
				jrarray = null;
			}

			if (jrarray != null) {
				
				foreach (var ev in jrarray) { 
					clsEventoAgenda e = new clsEventoAgenda();
					JObject jsonev = (JObject)ev;
					e.idevento = Int32.Parse(jsonev["idevento"].ToString());
					e.dia = Int32.Parse(jsonev["dia"].ToString());
					e.comentario = jsonev["titulo"].ToString();
					e.lapso = jsonev["lapso"].ToString();
					lste.Add(e);
				}
			}

			obj.lstEventos = lste;
			return obj;
		}
	}



}



