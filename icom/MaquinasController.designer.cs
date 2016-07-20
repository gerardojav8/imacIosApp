// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace icom
{
	[Register ("MaquinasController")]
	partial class MaquinasController
	{
		[Outlet]
		UIKit.UIButton btnAgregar { get; set; }

		[Outlet]
		UIKit.UIButton btnSearch { get; set; }

		[Outlet]
		UIKit.UITableView lstMaquinas { get; set; }

		[Outlet]
		UIKit.UITextField txtSearch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAgregar != null) {
				btnAgregar.Dispose ();
				btnAgregar = null;
			}

			if (btnSearch != null) {
				btnSearch.Dispose ();
				btnSearch = null;
			}

			if (lstMaquinas != null) {
				lstMaquinas.Dispose ();
				lstMaquinas = null;
			}

			if (txtSearch != null) {
				txtSearch.Dispose ();
				txtSearch = null;
			}
		}
	}
}
