// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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