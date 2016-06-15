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
    [Register ("Maquinaria")]
    partial class Maquinaria
    {
        [Outlet]
        UIKit.UIButton btnHome { get; set; }


        [Outlet]
        UIKit.UIButton btnInventario { get; set; }


        [Outlet]
        UIKit.UIButton btnReporteOperador { get; set; }


        [Outlet]
        UIKit.UIButton btnReporteServicio { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnHome != null) {
                btnHome.Dispose ();
                btnHome = null;
            }

            if (btnInventario != null) {
                btnInventario.Dispose ();
                btnInventario = null;
            }

            if (btnReporteOperador != null) {
                btnReporteOperador.Dispose ();
                btnReporteOperador = null;
            }

            if (btnReporteServicio != null) {
                btnReporteServicio.Dispose ();
                btnReporteServicio = null;
            }
        }
    }
}