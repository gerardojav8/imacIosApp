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
    [Register ("AltaObraController")]
    partial class AltaObraController
    {
        [Outlet]
        UIKit.UIButton btnGuardarObra { get; set; }


        [Outlet]
        UIKit.UITextView txtdescripcion { get; set; }


        [Outlet]
        UIKit.UITextField txtnombreobra { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnGuardarObra != null) {
                btnGuardarObra.Dispose ();
                btnGuardarObra = null;
            }

            if (txtdescripcion != null) {
                txtdescripcion.Dispose ();
                txtdescripcion = null;
            }

            if (txtnombreobra != null) {
                txtnombreobra.Dispose ();
                txtnombreobra = null;
            }
        }
    }
}