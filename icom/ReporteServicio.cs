using System;

using UIKit;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;


namespace icom
{
	public partial class ReporteServicio : UIViewController
	{

		public static List<String> lstref = new List<String>();
		public static Boolean stacsec = false;

		public ReporteServicio () : base ("ReporteServicio", null)
		{
			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			scrViewRepServicios.ContentSize = new CoreGraphics.CGSize(359, 1783);
			txtDescFalla.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescFalla.Layer.BorderWidth = (nfloat) 2.0;
			txtDescFalla.Text = "";

			txtObservaciones.Layer.BorderColor = UIColor.Black.CGColor;
			txtObservaciones.Layer.BorderWidth = (nfloat) 2.0;
			txtObservaciones.Text = "";

			lstRefacciones.Layer.BorderColor = UIColor.Black.CGColor;
			lstRefacciones.Layer.BorderWidth = (nfloat) 2.0;



			lstRefacciones.Source = new FuenteTablaRefacciones();

			btnaddref.TouchUpInside += delegate {
				lstref.Add(txtaddref.Text);
				lstRefacciones.ReloadData();
			};

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class TableSource : UITableViewSource {

		string[] TableItems;
		string CellIdentifier = "TableCell";
		public int selectedIndex = 0;

		public TableSource (string[] items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Length;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			this.selectedIndex = indexPath.Row;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
			string item = TableItems[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier); }

			cell.TextLabel.Text = item;

			return cell;
		}
	}

	public class FuenteTablaRefacciones : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(idPersonaje) as CustomrefaccionesCell;
			if (cell == null)
			{
				cell = new CustomrefaccionesCell((NSString)idPersonaje);
			}

			String refa = icom.ReporteServicio.lstref.ElementAt(indexPath.Row);

			cell.UpdateCell(refa);


			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return icom.ReporteServicio.lstref.Count;
		}
	}

	public class CustomrefaccionesCell : UITableViewCell
	{
		UILabel Refaccion;

		public CustomrefaccionesCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			icom.ReporteServicio.stacsec = !icom.ReporteServicio.stacsec;
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			if (icom.ReporteServicio.stacsec)
			{
				ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);
			}
			else {
				ContentView.BackgroundColor = UIColor.White;
			}



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
}


