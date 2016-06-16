using System;

using UIKit;
using Foundation;
using SQLite;
using System.Collections.Generic;

namespace icom
{
	public partial class ReporteServicio : UIViewController
	{
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

			List<String> lstItems = new List<String>();

			lstItems.Add ("Item One");
			lstItems.Add ("Item Two");
			lstItems.Add ("Item Three");

			TableSource source = new TableSource(lstItems.ToArray()); 
			lstRefacciones.Source = source;

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
}


