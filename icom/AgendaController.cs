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

		public static List<clsAgenda> LstDatosAgenda;


		public AgendaController() : base("AgendaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			LstDatosAgenda = new List<clsAgenda>();
			lstAgenda.Source = new FuenteTablaAgenda(this);

			btnNuevoEvento.TouchUpInside += delegate {
				NuevoEventoController viewne = new NuevoEventoController();
				viewne.Title = "Nuevo Evento";


				this.NavigationController.PushViewController(viewne, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			/*Boolean resp = await getAgenda();

			if (resp)
			{
				loadPop.Hide();
				lstAgenda.ReloadData();
			}*/

			clsAgenda obj1 = new clsAgenda();
			obj1.mes = 1;
			obj1.comentario = "";

			List<clsEventoAgenda> lste1 = new List<clsEventoAgenda>();

			clsEventoAgenda e11 = new clsEventoAgenda();
			e11.dia = 15;
			e11.comentario = "Reunion supervision";
			e11.lapso = "11:00 am - 12:00 pm";

			clsEventoAgenda e12 = new clsEventoAgenda();
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
			LstDatosAgenda.Add(obj12);


			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public async Task<Boolean> getAgenda()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Agenda ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getListadoAgenda";
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

	public class FuenteTablaAgenda : UITableViewSource
	{
		static readonly string celdahija = "Celda_hija";
		static readonly string celdapadre = "Celda_padre";
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;
		protected UIViewController viewparent;
		protected Boolean sec = false;

		public FuenteTablaAgenda(UIViewController view)
		{
			viewparent = view;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (currentExpandedIndex > -1)
			{
				return icom.AgendaController.LstDatosAgenda.Count + icom.AgendaController.LstDatosAgenda.ElementAt(currentExpandedIndex).lstEventos.Count;
			}

			return icom.AgendaController.LstDatosAgenda.Count;
		}



		void collapseSubItemsAtIndex(UITableView tableView, int index, int cant)
		{
			for (int i = 1; i <= cant; i++)
			{
				tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + i, 0) }, UITableViewRowAnimation.Fade);
			}

		}

		void expandItemAtIndex(UITableView tableView, int index, int cant)
		{
			int insertPos = index + 1;
			for (int i = 1; i <= cant; i++)
			{
				tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
			}


		}

		protected bool isChild(NSIndexPath indexPath)
		{			
			bool blnischild = currentExpandedIndex > -1 &&
				   indexPath.Row > currentExpandedIndex &&
				            indexPath.Row <= currentExpandedIndex + icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count;
			
			return blnischild;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (isChild(indexPath))
			{
				//Handle selection of child cell

				if (indexPath.Row > currentExpandedIndex)
				{
					DetalleAgendaController viewda = new DetalleAgendaController();
					viewda.Title = "Evento";
					viewda.viewagenda = viewparent;
					viewda.idagenda = 0;
					viewda.idevento = 0;


					viewparent.NavigationController.PushViewController(viewda, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
					UIView.CommitAnimations();

					tableView.DeselectRow(indexPath, true);
					return;
					//funciones.MessageBox("Aviso", "Click en evento");
				}

			}



			tableView.BeginUpdates();
			if (currentExpandedIndex == indexPath.Row)
			{
				this.collapseSubItemsAtIndex(tableView, currentExpandedIndex, icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
				currentExpandedIndex = -1;
			}
			else {	
				var shouldCollapse = currentExpandedIndex > -1;
				if (shouldCollapse)
				{
					this.collapseSubItemsAtIndex(tableView, currentExpandedIndex, icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
				}
				currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count : indexPath.Row;
				this.expandItemAtIndex(tableView, currentExpandedIndex, icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
			}
			tableView.EndUpdates();
			tableView.DeselectRow(indexPath, true);
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return (nfloat)0.0;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (!isChild(indexPath))
			{
				return (nfloat)50.0;
			}
			else {
				return (nfloat)70.0;
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			if (isChild(indexPath))
			{
				int indicesubarreglo = indexPath.Row - (currentExpandedIndex + 1);

				var cell = tableView.DequeueReusableCell(celdahija) as CustomAgendaCell;

				if (cell == null)
				{
					cell = new CustomAgendaCell((NSString)celdahija, sec);
				}


				clsAgenda objagenda = icom.AgendaController.LstDatosAgenda.ElementAt(currentExpandedIndex);

				UIImage imgFecha = UIImage.FromFile("fechaicon.png");

				clsEventoAgenda objev = objagenda.lstEventos.ElementAt(indicesubarreglo);
				String strComentario = objev.comentario;
				String strLapso = objev.lapso;

				cell.UpdateCell(strComentario, strLapso, imgFecha);


				cell.Accessory = UITableViewCellAccessory.None;

				return cell;
			}
			else {

				int indicearreglo = indexPath.Row;

				if (currentExpandedIndex > -1 && indexPath.Row > currentExpandedIndex)
				{
					indicearreglo -= icom.AgendaController.LstDatosAgenda.ElementAt(currentExpandedIndex).lstEventos.Count;
				}

				Boolean blnTieneEventos = false;
				if (icom.AgendaController.LstDatosAgenda.ElementAt(indicearreglo).lstEventos.Count > 0) {
					blnTieneEventos = true;
				}

				clsAgenda objagenda = icom.AgendaController.LstDatosAgenda.ElementAt(indicearreglo);
				var cell = tableView.DequeueReusableCell(celdapadre) as CustomPadreAgendaCell;

				if (cell == null)
				{
					cell = new CustomPadreAgendaCell((NSString)ChildCellIndentifier, funciones.getColorMes(objagenda.mes));
				}

				UIImage img = null;
				if (blnTieneEventos)
					img = UIImage.FromFile("more.png");
				
				cell.UpdateCell(funciones.getNombreMes(objagenda.mes), img);

				return cell;


			}

		}
	}

	public class CustomPadreAgendaCell : UITableViewCell
	{
		UILabel headingLabel;
		UIImageView imageView;

		public CustomPadreAgendaCell(NSString cellId, UIColor colorcell) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			ContentView.BackgroundColor = colorcell;

			imageView = new UIImageView();			
			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(227, 234, 243),
				BackgroundColor = UIColor.Clear
			};
			ContentView.AddSubviews(new UIView[] { headingLabel, imageView});

		}
		public void UpdateCell(string caption, UIImage image)
		{
			imageView.Image = image;
			headingLabel.Text = caption;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			headingLabel.Frame = new CGRect(10, 7, ContentView.Bounds.Width - 63, 25);
			imageView.Frame = new CGRect(250, 6, 20, 20);

		}

	}

	public class CustomAgendaCell : UITableViewCell
	{
		UILabel headingLabel, subheadingLabel;
		UIImageView imageView;

		public CustomAgendaCell(NSString cellId, Boolean sec) : base(UITableViewCellStyle.Default, cellId)
		{
			ContentView.BackgroundColor = UIColor.FromRGB(217, 215, 213);

			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			imageView = new UIImageView();
			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};
			subheadingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 13f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};
			ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel, imageView });

		}
		public void UpdateCell(string caption, string subtitle, UIImage image)
		{
			imageView.Image = image;
			headingLabel.Text = caption;
			subheadingLabel.Text = subtitle;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			imageView.Frame = new CGRect(4, 4, 50, 50);
			headingLabel.Frame = new CGRect(70, 4, ContentView.Bounds.Width - 63, 25);
			subheadingLabel.Frame = new CGRect(70, 32, 500, 20);
		}

	}

}



