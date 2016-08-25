using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public class FuenteTablaClasificaciones : UITableViewSource
	{
		static readonly string celda = "celda";
		private List<clsClasificacion> lstCalsificacion;
		protected UIViewController viewparent;

		public FuenteTablaClasificaciones(UIViewController view, List<clsClasificacion> lst)
		{
			lstCalsificacion = lst;
			viewparent = view;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstCalsificacion.Count;
		}


		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			CategoriasModController viewcm = new CategoriasModController();


			viewcm.Title = "Modificar Clasificacion";
			viewparent.NavigationController.PushViewController(viewcm, false);
			UIView.BeginAnimations(null);
			UIView.SetAnimationDuration(0.7);
			UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, viewparent.NavigationController.View, true);
			UIView.CommitAnimations();
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{

			return (nfloat)50.0;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(celda) as UITableViewCell;
			int indicearreglo = indexPath.Row;

			clsClasificacion objclas = lstCalsificacion.ElementAt(indicearreglo);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, celda);
			}

			cell.TextLabel.Text = objclas.nombre;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			if (indicearreglo % 2 == 0)
			{
				cell.BackgroundColor = UIColor.FromRGB(220, 224, 231);
			}
			else { 
				cell.BackgroundColor = UIColor.White;
			}

			return cell;
		}
	}
}

