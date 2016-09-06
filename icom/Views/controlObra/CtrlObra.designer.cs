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
    [Register ("CtrlObra")]
    partial class CtrlObra
    {
        [Outlet]
        UIKit.UIButton btnAgenda { get; set; }


        [Outlet]
        UIKit.UIButton btnMensajes { get; set; }


        [Outlet]
        UIKit.UIButton btnPlanificador { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAgenda != null) {
                btnAgenda.Dispose ();
                btnAgenda = null;
            }

            if (btnMensajes != null) {
                btnMensajes.Dispose ();
                btnMensajes = null;
            }

            if (btnPlanificador != null) {
                btnPlanificador.Dispose ();
                btnPlanificador = null;
            }
        }
    }
}