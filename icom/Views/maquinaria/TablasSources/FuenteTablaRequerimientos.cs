using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
namespace icom
{
	public class FuenteTablaRequerimientos : UITableViewSource
	{
		static readonly string celda = "Celda";
		static readonly string celdaBlack = "CeldaBlack";
		private List<clsSolicitudesMaquinas> lstsol;

		public FuenteTablaRequerimientos(List<clsSolicitudesMaquinas> lst)
		{
			lstsol = lst;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell;
			if (indexPath.Row % 2 == 0)
			{
				cell = tableView.DequeueReusableCell(celdaBlack) as CustomSolMaqCellBlack;
				if (cell == null)
				{
					cell = new CustomSolMaqCellBlack((NSString)celdaBlack);
				}

			}
			else {

				cell = tableView.DequeueReusableCell(celda) as CustomSolMaqCellWhite;
				if (cell == null)
				{
					cell = new CustomSolMaqCellWhite((NSString)celda);
				}

			}

			clsSolicitudesMaquinas sol = lstsol.ElementAt(indexPath.Row);
			((CustomSolMaqCell)cell).UpdateCell(sol.cantidad.ToString(), sol.equipo, sol.marca, sol.modelo);

			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstsol.Count;
		}

	}

	public class CustomSolMaqCell : UITableViewCell
	{
		UILabel cantidadlabel, equipolabel, marcalabel, modelolabel;


		public CustomSolMaqCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			cantidadlabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			equipolabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};
			marcalabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			modelolabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			ContentView.AddSubviews(new UIView[] { cantidadlabel, equipolabel, marcalabel, modelolabel });

		}
		public void UpdateCell(string cantidad, string equipo, String marca, String modelo)
		{
			cantidadlabel.Text = cantidad;
			equipolabel.Text = equipo;
			marcalabel.Text = marca;
			modelolabel.Text = modelo;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			cantidadlabel.Frame = new CGRect(20, 10, 30, 20);
			equipolabel.Frame = new CGRect(55, 10, 100, 20);
			marcalabel.Frame = new CGRect(170, 10, 70, 20);
			modelolabel.Frame = new CGRect(250, 10, 70, 20);
		}

	}

	public class CustomSolMaqCellBlack : CustomSolMaqCell
	{

		public CustomSolMaqCellBlack(NSString cellId) : base(cellId)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomSolMaqCellWhite : CustomSolMaqCell
	{

		public CustomSolMaqCellWhite(NSString cellId) : base(cellId)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}
}

