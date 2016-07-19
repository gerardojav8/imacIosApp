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

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class FuenteTablaAgenda : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;
		protected int countEventos = -1;
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
			countEventos = -1;
		}

		void expandItemAtIndex(UITableView tableView, int index, int cant)
		{
			int insertPos = index + 1;
			for (int i = 1; i <= cant; i++)
			{
				tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
			}

			countEventos = 0;
		}

		protected bool isChild(NSIndexPath indexPath)
		{			
			return currentExpandedIndex > -1 &&
				   indexPath.Row > currentExpandedIndex &&
				            indexPath.Row <= currentExpandedIndex + icom.AgendaController.LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count;
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
				var cell = tableView.DequeueReusableCell(idPersonaje) as CustomAgendaCell;

				if (cell == null)
				{
					cell = new CustomAgendaCell((NSString)idPersonaje, sec);
				}


				clsAgenda objagenda = icom.AgendaController.LstDatosAgenda.ElementAt(currentExpandedIndex);

				UIImage imgFecha = UIImage.FromFile("fechaicon.png");

				clsEventoAgenda objev = objagenda.lstEventos.ElementAt(countEventos);
				String strComentario = objev.comentario;
				String strLapso = objev.lapso;


				UIImage imagesem = UIImage.FromFile("red.png");

				cell.UpdateCell(strComentario, strLapso, imgFecha);

				countEventos++;

				cell.Accessory = UITableViewCellAccessory.None;

				return cell;
			}
			else {

				var cell = tableView.DequeueReusableCell(idPersonaje);
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, idPersonaje);
				}


				int indicearreglo = indexPath.Row;

				if (currentExpandedIndex > -1 && indexPath.Row > currentExpandedIndex)
				{
					indicearreglo -= icom.AgendaController.LstDatosAgenda.ElementAt(currentExpandedIndex).lstEventos.Count;
				}
					

				clsAgenda objagenda = icom.AgendaController.LstDatosAgenda.ElementAt(indicearreglo);

				cell.TextLabel.Text = funciones.getNombreMes(objagenda.mes);


				cell.TextLabel.TextColor = UIColor.FromRGB(227, 234, 243);

				cell.TextLabel.Font = UIFont.FromName("Arial-BoldMT", 20f);

				cell.ImageView.Image = null;
				cell.ContentView.BackgroundColor = funciones.getColorMes(objagenda.mes);

				return cell;


			}

		}
	}

	public class CustomAgendaCell : UITableViewCell
	{
		UILabel headingLabel, subheadingLabel;
		UIImageView imageView;
		UIImageView imageView2;

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



