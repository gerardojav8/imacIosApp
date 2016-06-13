using System;

using UIKit;
using System.Collections.Generic;
using Foundation;

namespace icom
{
	public partial class MaquinasController : UIViewController
	{
		public MaquinasController() : base("MaquinasController", null)
		{
		}
		List<String> lstItems = new List<String>();
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			lstItems.Add("Maquina 1");
			lstItems.Add("Maquina 2");
			lstItems.Add("Maquina 3");

			fuentetabla source = new fuentetabla(lstItems.ToArray());
			lstMaquinas.Source = source;


			btnTest.TouchUpInside += delegate
			{
				WillBeginTableEditing(lstMaquinas);
			};

		}

		public void WillBeginTableEditing(UITableView tableView)
		{
			tableView.BeginUpdates();
			// insert the 'ADD NEW' row at the end of table display
			tableView.InsertRows(new NSIndexPath[] {
			NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
			}, UITableViewRowAnimation.Fade);
			// create a new item and add it to our underlying data (it is not intended to be permanent)
			lstItems.Add("(add new)");
			fuentetabla source = new fuentetabla(lstItems.ToArray());
			lstMaquinas.Source = source;
			tableView.EndUpdates(); // applies the changes
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class fuentetabla : TableSource
	{
		private string[] lstItems;
		public fuentetabla(string[] items) : base(items) {
			lstItems = items;
		}
		public override nint NumberOfSections(UITableView tableView)
		{
			return lstItems.Length;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			//return base.RowsInSection(tableview, section);
			return 2;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = base.GetCell(tableView, indexPath);
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
			return cell;
		}

		public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
		{
			new UIAlertView(lstItems[indexPath.Row].ToString(), "prueba",null, "Aceptar", null ).Show ();

			tableView.DeselectRow(indexPath, true);

		}
	}
	public class ExpandableTableSource<T> : UITableViewSource
	{
		public IReadOnlyList<T> Items;
		protected readonly Action<T> TSelected;
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;

		public ExpandableTableSource() { }


		public ExpandableTableSource(Action<T> TSelected)
		{
			this.TSelected = TSelected;

		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count + ((currentExpandedIndex > -1) ? 1 : 0);
		}

		void collapseSubItemsAtIndex(UITableView tableView, int index)
		{
			tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
		}

		void expandItemAtIndex(UITableView tableView, int index)
		{
			int insertPos = index + 1;
			tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
		}

		protected bool isChild(NSIndexPath indexPath)
		{
			return currentExpandedIndex > -1 &&
				   indexPath.Row > currentExpandedIndex &&
				   indexPath.Row <= currentExpandedIndex + 1;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (isChild(indexPath))
			{
				//Handle selection of child cell
				Console.WriteLine("You touched a child!");
				tableView.DeselectRow(indexPath, true);
				return;
			}
			tableView.BeginUpdates();
			if (currentExpandedIndex == indexPath.Row)
			{
				this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
				currentExpandedIndex = -1;
			}
			else {
				var shouldCollapse = currentExpandedIndex > -1;
				if (shouldCollapse)
				{
					this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
				}
				currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - 1 : indexPath.Row;
				this.expandItemAtIndex(tableView, currentExpandedIndex);
			}
			tableView.EndUpdates();
			tableView.DeselectRow(indexPath, true);
		}

		//TODO: implement this here?
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			throw new NotImplementedException();
		}
	}

}


