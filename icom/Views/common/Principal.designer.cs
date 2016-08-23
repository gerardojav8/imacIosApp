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
	[Register ("Principal")]
	partial class Principal
	{
		[Outlet]
		UIKit.UIButton btnCerrarSesion { get; set; }

		[Outlet]
		UIKit.UIButton btnCtrlObra { get; set; }

		[Outlet]
		UIKit.UIButton btnInformacion { get; set; }

		[Outlet]
		UIKit.UIButton btnMaquinaria { get; set; }

		[Outlet]
		UIKit.UIButton btnProduccion { get; set; }

		[Outlet]
		UIKit.UILabel lblUsuario { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnCerrarSesion != null) {
				btnCerrarSesion.Dispose ();
				btnCerrarSesion = null;
			}

			if (btnCtrlObra != null) {
				btnCtrlObra.Dispose ();
				btnCtrlObra = null;
			}

			if (btnInformacion != null) {
				btnInformacion.Dispose ();
				btnInformacion = null;
			}

			if (btnMaquinaria != null) {
				btnMaquinaria.Dispose ();
				btnMaquinaria = null;
			}

			if (btnProduccion != null) {
				btnProduccion.Dispose ();
				btnProduccion = null;
			}

			if (lblUsuario != null) {
				lblUsuario.Dispose ();
				lblUsuario = null;
			}
		}
	}
}
