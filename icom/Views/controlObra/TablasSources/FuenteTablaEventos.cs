using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public class FuenteTablaEventos : UITableViewSource
	{
		static readonly string celdaEventos = "CeldaEventos";
		private List<clsEvento> lstEventos;
		protected UIViewController viewparent;

		public FuenteTablaEventos(UIViewController view, List<clsEvento> lst)
		{
			viewparent = view;
			lstEventos = lst;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstEventos.Count;
		}


		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			
			return (nfloat)50.0;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			int indicearreglo = indexPath.Row;

			UITableViewCell cell;
			clsEvento objev = lstEventos.ElementAt(indicearreglo);
			cell = tableView.DequeueReusableCell(celdaEventos) as CustomEventosCell;

			if (cell == null)
			{
				cell = new CustomEventosCell((NSString)celdaEventos, objev.porcentajeavance);
			}


			String lapso = "De: " + objev.horainicio + " a " + objev.horafinal;
			String totalhoras = "Hrs.: " + objev.totalhoras;
			((CustomEventosCell)cell).UpdateCell(objev.titulo, objev.clasificacion, totalhoras, lapso);

			return cell;




		}
	}

	public class CustomEventosCell : UITableViewCell
	{
		UILabel headingLabel, categoriaLabel, horasLabel, iniciofinlabel, colorLabel, porLabel;
		private double porcentajelabel;

		public CustomEventosCell(NSString cellId, int por) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Blue;


			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			categoriaLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			horasLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right				                         
			};

			iniciofinlabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right
			};

			colorLabel = new UILabel()
			{				
				BackgroundColor = UIColor.FromRGB(220, 224, 231)
			};

			porLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 7f),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.FromRGB(78, 150, 167)
			};

			porcentajelabel = por;


			ContentView.AddSubviews(new UIView[] { colorLabel, porLabel,  headingLabel, categoriaLabel, horasLabel, iniciofinlabel });

		}
		public void UpdateCell(string titulo, string categoria, string horas, string iniciofin)
		{
			
			headingLabel.Text = titulo;
			categoriaLabel.Text = categoria;
			horasLabel.Text = horas;
			iniciofinlabel.Text = iniciofin;
			porLabel.Text = porcentajelabel.ToString() + " %";
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			headingLabel.Frame = new CGRect(10, 1, ContentView.Bounds.Width - 63, 25);
			categoriaLabel.Frame = new CGRect(10, 19, ContentView.Bounds.Width - 63, 25);
			horasLabel.Frame = new CGRect(100, 1, ContentView.Bounds.Width - 102, 25);
			iniciofinlabel.Frame = new CGRect(100, 19, ContentView.Bounds.Width - 102, 25);
			colorLabel.Frame = new CGRect(ContentView.Bounds.Width - 125, 0, 125, 50);

			double tam = (porcentajelabel * ContentView.Bounds.Width) / 100;
			porLabel.Frame = new CGRect(0, 42, tam, 7);



		}

	}


}

