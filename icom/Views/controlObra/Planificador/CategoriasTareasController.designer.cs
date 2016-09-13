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
		UIKit.UIButton btnBusquedaCategoria { get; set; }

		[Outlet]
		UIKit.UIButton btnGrafica { get; set; }

		[Outlet]
		UIKit.UIButton btnModificarCategoria { get; set; }

		[Outlet]
		UIKit.UIButton btnNuevaCategoria { get; set; }

		[Outlet]
		UIKit.UITableView tblCategorias { get; set; }

		[Outlet]
		UIKit.UITextField txtbusquedaCategoria { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnGrafica != null) {
				btnGrafica.Dispose ();
				btnGrafica = null;
			}

			if (btnBusquedaCategoria != null) {
				btnBusquedaCategoria.Dispose ();
				btnBusquedaCategoria = null;
			}

			if (btnModificarCategoria != null) {
				btnModificarCategoria.Dispose ();
				btnModificarCategoria = null;
			}

			if (btnNuevaCategoria != null) {
				btnNuevaCategoria.Dispose ();
				btnNuevaCategoria = null;
			}

			if (tblCategorias != null) {
				tblCategorias.Dispose ();
				tblCategorias = null;
			}

			if (txtbusquedaCategoria != null) {
				txtbusquedaCategoria.Dispose ();
				txtbusquedaCategoria = null;
			}
		}
	}
}
