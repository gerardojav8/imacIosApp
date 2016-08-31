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
	[Register ("ExportarTareasController")]
	partial class ExportarTareasController
	{
		[Outlet]
		UIKit.UIButton btnAceptar { get; set; }

		[Outlet]
		UIKit.UIButton btnDesde { get; set; }

		[Outlet]
		UIKit.UIButton btnHasta { get; set; }

		[Outlet]
		UIKit.UITextField txtDesde { get; set; }

		[Outlet]
		UIKit.UITextField txtHasta { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtDesde != null) {
				txtDesde.Dispose ();
				txtDesde = null;
			}

			if (btnDesde != null) {
				btnDesde.Dispose ();
				btnDesde = null;
			}

			if (txtHasta != null) {
				txtHasta.Dispose ();
				txtHasta = null;
			}

			if (btnHasta != null) {
				btnHasta.Dispose ();
				btnHasta = null;
			}

			if (btnAceptar != null) {
				btnAceptar.Dispose ();
				btnAceptar = null;
			}
		}
	}
}
