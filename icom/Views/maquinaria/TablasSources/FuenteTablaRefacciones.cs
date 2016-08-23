using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
using icom.globales;
namespace icom
{
	public class FuenteTablaRefacciones : UITableViewSource
	{
		static readonly string CeldaWhite = "CeldaWhite";
		static readonly string CeldaBlack = "CeldaBlack";
		private List<String> lstref;


		public FuenteTablaRefacciones(List<String> lst)
		{			
			lstref = lst;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell;
			if (indexPath.Row % 2 == 0)
			{

				cell = tableView.DequeueReusableCell(CeldaBlack) as CustomrefaccionesCellBlack;
				if (cell == null)
				{
					cell = new CustomrefaccionesCellBlack((NSString)CeldaBlack);
				}
			}
			else { 
				cell = tableView.DequeueReusableCell(CeldaWhite) as CustomrefaccionesCellWhite;
				if (cell == null)
				{
					cell = new CustomrefaccionesCellWhite((NSString)CeldaWhite);
				}
			}

			String refa = lstref.ElementAt(indexPath.Row);

			((CustomrefaccionesCell)cell).UpdateCell(refa);
			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstref.Count;
		}
	}

	public class CustomrefaccionesCell : UITableViewCell
	{
		UILabel Refaccion;

		public CustomrefaccionesCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{			
			SelectionStyle = UITableViewCellSelectionStyle.Gray;				
			Refaccion = new UILabel()
			{
				Font = UIFont.FromName("Arial", 15f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			ContentView.AddSubviews(new UIView[] { Refaccion });

		}
		public void UpdateCell(string refaccion)
		{
			Refaccion.Text = refaccion;

		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Refaccion.Frame = new CGRect(20, 10, 200, 20);

		}

	}

	public class CustomrefaccionesCellBlack : CustomrefaccionesCell
	{

		public CustomrefaccionesCellBlack(NSString cellId) : base(cellId)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomrefaccionesCellWhite : CustomrefaccionesCell
	{

		public CustomrefaccionesCellWhite(NSString cellId) : base(cellId)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}
}

