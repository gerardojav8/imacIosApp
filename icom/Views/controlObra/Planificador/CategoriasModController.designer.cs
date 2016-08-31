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
	[Register ("CategoriasModController")]
	partial class CategoriasModController
	{
		[Outlet]
		UIKit.UIButton btnEliminar { get; set; }

		[Outlet]
		UIKit.UIButton btnGuardar { get; set; }

		[Outlet]
		UIKit.UITextView txtComentario { get; set; }

		[Outlet]
		UIKit.UITextField txtNombreCategoria { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtNombreCategoria != null) {
				txtNombreCategoria.Dispose ();
				txtNombreCategoria = null;
			}

			if (txtComentario != null) {
				txtComentario.Dispose ();
				txtComentario = null;
			}

			if (btnEliminar != null) {
				btnEliminar.Dispose ();
				btnEliminar = null;
			}

			if (btnGuardar != null) {
				btnGuardar.Dispose ();
				btnGuardar = null;
			}
		}
	}
}
