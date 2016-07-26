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
	[Register ("NuevoEventoController")]
	partial class NuevoEventoController
	{
		[Outlet]
		UIKit.UIButton btnaceptar { get; set; }

		[Outlet]
		UIKit.UIButton btnfecha { get; set; }

		[Outlet]
		UIKit.UIButton btnhora { get; set; }

		[Outlet]
		UIKit.UIButton btnhorafin { get; set; }

		[Outlet]
		UIKit.UITextView txtComentario { get; set; }

		[Outlet]
		UIKit.UITextField txtfechaevento { get; set; }

		[Outlet]
		UIKit.UITextField txthorafin { get; set; }

		[Outlet]
		UIKit.UITextField txthorainicio { get; set; }

		[Outlet]
		UIKit.UITextField txtTitulo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtTitulo != null) {
				txtTitulo.Dispose ();
				txtTitulo = null;
			}

			if (txtComentario != null) {
				txtComentario.Dispose ();
				txtComentario = null;
			}

			if (txtfechaevento != null) {
				txtfechaevento.Dispose ();
				txtfechaevento = null;
			}

			if (txthorainicio != null) {
				txthorainicio.Dispose ();
				txthorainicio = null;
			}

			if (txthorafin != null) {
				txthorafin.Dispose ();
				txthorafin = null;
			}

			if (btnfecha != null) {
				btnfecha.Dispose ();
				btnfecha = null;
			}

			if (btnhora != null) {
				btnhora.Dispose ();
				btnhora = null;
			}

			if (btnhorafin != null) {
				btnhorafin.Dispose ();
				btnhorafin = null;
			}

			if (btnaceptar != null) {
				btnaceptar.Dispose ();
				btnaceptar = null;
			}
		}
	}
}
