using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
namespace icom
{
	public class FuenteTablaAgenda : UITableViewSource
	{
		static readonly string celdahija = "Celda_hija";
		static readonly string celdapadre = "Celda_padre";
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;
		protected UIViewController viewparent;
		private List<clsAgenda> LstDatosAgenda;

		public FuenteTablaAgenda(UIViewController view, List<clsAgenda> lst)
		{			
			viewparent = view;
			LstDatosAgenda = lst;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (currentExpandedIndex > -1)
			{
				return LstDatosAgenda.Count + LstDatosAgenda.ElementAt(currentExpandedIndex).lstEventos.Count;
			}

			return LstDatosAgenda.Count;
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
							indexPath.Row <= currentExpandedIndex + LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count;

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
					int indiceevento = indexPath.Row - currentExpandedIndex - 1;
					viewda.idevento = LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.ElementAt(indiceevento).idevento;


					viewparent.NavigationController.PushViewController(viewda, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
					UIView.CommitAnimations();

					tableView.DeselectRow(indexPath, true);
					return;
				}

			}

			tableView.BeginUpdates();

			if (currentExpandedIndex == indexPath.Row)
			{
				this.collapseSubItemsAtIndex(tableView, currentExpandedIndex, LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
				currentExpandedIndex = -1;
			} else {
				var shouldCollapse = currentExpandedIndex > -1;
				if (shouldCollapse)
				{
					this.collapseSubItemsAtIndex(tableView, currentExpandedIndex, LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
				}
				currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count : indexPath.Row;
				this.expandItemAtIndex(tableView, currentExpandedIndex, LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count);
			}
			tableView.EndUpdates();
			tableView.DeselectRow(indexPath, true);

			if ((int)currentExpandedIndex == 11)
			{
				tableView.ScrollToRow(NSIndexPath.FromRowSection(11 +LstDatosAgenda.ElementAt((int)currentExpandedIndex).lstEventos.Count, 0), UITableViewScrollPosition.Bottom, true);
			}

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
					cell = new CustomAgendaCell((NSString)celdahija);
				}


				clsAgenda objagenda = LstDatosAgenda.ElementAt(currentExpandedIndex);



				clsEventoAgenda objev = objagenda.lstEventos.ElementAt(indicesubarreglo);
				String strComentario = objev.comentario;
				String strLapso = objev.lapso;

				UIImage imgFecha = UIImage.FromFile("calendario/schedule_" + objev.dia + ".png");
				cell.UpdateCell(strComentario, strLapso, imgFecha);


				cell.Accessory = UITableViewCellAccessory.None;



				return cell;
			}
			else {

				int indicearreglo = indexPath.Row;

				if (currentExpandedIndex > -1 && indexPath.Row > currentExpandedIndex)
				{
					indicearreglo -= LstDatosAgenda.ElementAt(currentExpandedIndex).lstEventos.Count;
				}

				Boolean blnTieneEventos = false;
				if (LstDatosAgenda.ElementAt(indicearreglo).lstEventos.Count > 0)
				{
					blnTieneEventos = true;
				}

				clsAgenda objagenda = LstDatosAgenda.ElementAt(indicearreglo);
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
			ContentView.AddSubviews(new UIView[] { headingLabel, imageView });

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

		public CustomAgendaCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
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

