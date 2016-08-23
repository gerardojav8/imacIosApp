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
	[Register ("CtrlObra")]
	partial class CtrlObra
	{
		[Outlet]
		UIKit.UIButton btnAgenda { get; set; }

		[Outlet]
		UIKit.UIButton btnMensajes { get; set; }

		[Outlet]
		UIKit.UIButton btnPlanificador { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAgenda != null) {
				btnAgenda.Dispose ();
				btnAgenda = null;
			}

			if (btnMensajes != null) {
				btnMensajes.Dispose ();
				btnMensajes = null;
			}

			if (btnPlanificador != null) {
				btnPlanificador.Dispose ();
				btnPlanificador = null;
			}
		}
	}
}
