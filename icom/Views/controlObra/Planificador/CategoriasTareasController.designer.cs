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
	[Register ("CategoriasTareasController")]
	partial class CategoriasTareasController
	{
		[Outlet]
		UIKit.UIButton btnNuevaCategoria { get; set; }

		[Outlet]
		UIKit.UITableView tblCategorias { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblCategorias != null) {
				tblCategorias.Dispose ();
				tblCategorias = null;
			}

			if (btnNuevaCategoria != null) {
				btnNuevaCategoria.Dispose ();
				btnNuevaCategoria = null;
			}
		}
	}
}
