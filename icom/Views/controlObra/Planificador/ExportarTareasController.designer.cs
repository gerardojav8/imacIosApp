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
    [Register ("ExportarTareasController")]
    partial class ExportarTareasController
    {
        [Outlet]
        UIKit.UIButton btnAceptar { get; set; }


        [Outlet]
        UIKit.UIButton btnDesde { get; set; }


        [Outlet]
        UIKit.UIButton btnHasta { get; set; }


        [Outlet]
        UIKit.UITextField txtDesde { get; set; }


        [Outlet]
        UIKit.UITextField txtHasta { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAceptar != null) {
                btnAceptar.Dispose ();
                btnAceptar = null;
            }

            if (btnDesde != null) {
                btnDesde.Dispose ();
                btnDesde = null;
            }

            if (btnHasta != null) {
                btnHasta.Dispose ();
                btnHasta = null;
            }

            if (txtDesde != null) {
                txtDesde.Dispose ();
                txtDesde = null;
            }

            if (txtHasta != null) {
                txtHasta.Dispose ();
                txtHasta = null;
            }
        }
    }
}