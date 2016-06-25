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
		UIKit.UITextView txtComentarios { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lbltitulo != null) {
				lbltitulo.Dispose ();
				lbltitulo = null;
			}

			if (txtComentarios != null) {
				txtComentarios.Dispose ();
				txtComentarios = null;
			}

			if (btnGuardarEF != null) {
				btnGuardarEF.Dispose ();
				btnGuardarEF = null;
			}
		}
	}
}
