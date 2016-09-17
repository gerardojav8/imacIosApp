using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public class FuenteTablaGraficas : UITableViewSource
	{
		static readonly string celdaGraficasBlack = "CeldaGraficasBlack";
		static readonly string celdaGraficasWhite = "CeldaGraficasWhite";
		private List<clsClasificacion> lstCalsificacion;
		protected UIViewController viewparent;

		public FuenteTablaGraficas(UIViewController view, List<clsClasificacion> lst)
		{
			viewparent = view;
			lstCalsificacion = lst;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstCalsificacion.Count;
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
			clsClasificacion objclas = lstCalsificacion.ElementAt(indicearreglo);

			if (indicearreglo % 2 == 0)
			{
				cell = tableView.DequeueReusableCell(celdaGraficasBlack) as CustomGraficaCellBlack;

				if (cell == null)
				{
					cell = new CustomGraficaCellBlack((NSString)celdaGraficasBlack, objclas.color);
				}
			}
			else { 
				cell = tableView.DequeueReusableCell(celdaGraficasWhite) as CustomGraficaCellWhite;

				if (cell == null)
				{
					cell = new CustomGraficaCellWhite((NSString)celdaGraficasWhite, objclas.color);
				}
			}

			String strpor = objclas.porcentaje.ToString() + " % ";


			((CustomGraficaCell)cell).UpdateCell(objclas.nombre, strpor);

			return cell;




		}
	}

	public class CustomGraficaCell : UITableViewCell
	{
		UILabel headingLabel, colorLabel, porcentajeLabel;

		public CustomGraficaCell(NSString cellId, UIColor colorclas) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);


			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			porcentajeLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right				                         
			};

			colorLabel = new UILabel()
			{
				BackgroundColor = colorclas
			};


			ContentView.AddSubviews(new UIView[] { colorLabel, headingLabel, porcentajeLabel });

		}
		public void UpdateCell(string titulo, string por)
		{

			headingLabel.Text = titulo;
			porcentajeLabel.Text = por;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			headingLabel.Frame = new CGRect(14, 12, ContentView.Bounds.Width - 63, 25);
			porcentajeLabel.Frame = new CGRect(10, 12, ContentView.Bounds.Width - 14, 25);
			colorLabel.Frame = new CGRect(0, 0, 10, 50);

		}
	}

	public class CustomGraficaCellBlack : CustomGraficaCell
	{

		public CustomGraficaCellBlack(NSString cellId, UIColor colorclas) : base(cellId, colorclas)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomGraficaCellWhite : CustomGraficaCell
	{

		public CustomGraficaCellWhite(NSString cellId, UIColor colorclas) : base(cellId, colorclas)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}
}
