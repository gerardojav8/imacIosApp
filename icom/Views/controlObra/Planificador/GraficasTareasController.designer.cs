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
	[Register ("GraficasTareasController")]
	partial class GraficasTareasController
	{
		[Outlet]
		UIKit.UIButton btnExportarGrafica { get; set; }

		[Outlet]
		UIKit.UITableView tblClasificaciones { get; set; }

		[Outlet]
		UIKit.UIView vwGrafica { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnExportarGrafica != null) {
				btnExportarGrafica.Dispose ();
				btnExportarGrafica = null;
			}

			if (tblClasificaciones != null) {
				tblClasificaciones.Dispose ();
				tblClasificaciones = null;
			}

			if (vwGrafica != null) {
				vwGrafica.Dispose ();
				vwGrafica = null;
			}
		}
	}
}