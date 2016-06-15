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
		UIKit.UIButton btnbusqueda { get; set; }

		[Outlet]
		UIKit.UIButton btnTest { get; set; }

		[Outlet]
		UIKit.UITableView lstMaquinas { get; set; }

		[Outlet]
		UIKit.UITextField txtbusqueda { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtbusqueda != null) {
				txtbusqueda.Dispose ();
				txtbusqueda = null;
			}

			if (btnbusqueda != null) {
				btnbusqueda.Dispose ();
				btnbusqueda = null;
			}

			if (btnTest != null) {
				btnTest.Dispose ();
				btnTest = null;
			}

			if (lstMaquinas != null) {
				lstMaquinas.Dispose ();
				lstMaquinas = null;
			}
		}
	}
}
