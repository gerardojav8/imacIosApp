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
	[Register ("EstadoFisicoController")]
	partial class EstadoFisicoController
	{
		[Outlet]
		UIKit.UIButton btnGuardarEF { get; set; }

		[Outlet]
		UIKit.UILabel lbltitulo { get; set; }

		[Outlet]
		UIKit.UITextField txtcalificacion { get; set; }

		[Outlet]
		UIKit.UITextField txtcapacidad { get; set; }

		[Outlet]
		UIKit.UITextView txtComentarios { get; set; }

		[Outlet]
		UIKit.UITextField txtmarca { get; set; }

		[Outlet]
		UIKit.UITextField txttipo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnGuardarEF != null) {
				btnGuardarEF.Dispose ();
				btnGuardarEF = null;
			}

			if (lbltitulo != null) {
				lbltitulo.Dispose ();
				lbltitulo = null;
			}

			if (txtcalificacion != null) {
				txtcalificacion.Dispose ();
				txtcalificacion = null;
			}

			if (txtcapacidad != null) {
				txtcapacidad.Dispose ();
				txtcapacidad = null;
			}

			if (txtComentarios != null) {
				txtComentarios.Dispose ();
				txtComentarios = null;
			}

			if (txtmarca != null) {
				txtmarca.Dispose ();
				txtmarca = null;
			}

			if (txttipo != null) {
				txttipo.Dispose ();
				txttipo = null;
			}
		}
	}
}
