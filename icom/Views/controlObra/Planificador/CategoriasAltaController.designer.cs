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
	[Register ("CategoriasAltaController")]
	partial class CategoriasAltaController
	{
		[Outlet]
		UIKit.UIButton btnGuardar { get; set; }

		[Outlet]
		UIKit.UITextView txtComentarios { get; set; }

		[Outlet]
		UIKit.UITextField txtNombreCategoria { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtNombreCategoria != null) {
				txtNombreCategoria.Dispose ();
				txtNombreCategoria = null;
			}

			if (txtComentarios != null) {
				txtComentarios.Dispose ();
				txtComentarios = null;
			}

			if (btnGuardar != null) {
				btnGuardar.Dispose ();
				btnGuardar = null;
			}
		}
	}
}
