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
	[Register ("Maquinaria")]
	partial class Maquinaria
	{
		[Outlet]
		UIKit.UIButton btnHome { get; set; }

		[Outlet]
		UIKit.UIButton btnInventario { get; set; }

		[Outlet]
		UIKit.UIButton btnReporteOperador { get; set; }

		[Outlet]
		UIKit.UIButton btnReporteServicio { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnHome != null) {
				btnHome.Dispose ();
				btnHome = null;
			}

			if (btnInventario != null) {
				btnInventario.Dispose ();
				btnInventario = null;
			}

			if (btnReporteOperador != null) {
				btnReporteOperador.Dispose ();
				btnReporteOperador = null;
			}

			if (btnReporteServicio != null) {
				btnReporteServicio.Dispose ();
				btnReporteServicio = null;
			}
		}
	}
}
