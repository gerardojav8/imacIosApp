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
    [Register ("ReporteServicio")]
    partial class ReporteServicio
    {
        [Outlet]
        UIKit.UIButton btnGuardar { get; set; }


        [Outlet]
        UIKit.UIView ContentViewRepServicios { get; set; }


        [Outlet]
        UIKit.UITableView lstRefacciones { get; set; }


        [Outlet]
        UIKit.UIScrollView scrViewRepServicios { get; set; }


        [Outlet]
        UIKit.UITextView txtDescFalla { get; set; }


        [Outlet]
        UIKit.UITextView txtObservaciones { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnGuardar != null) {
                btnGuardar.Dispose ();
                btnGuardar = null;
            }

            if (ContentViewRepServicios != null) {
                ContentViewRepServicios.Dispose ();
                ContentViewRepServicios = null;
            }

            if (lstRefacciones != null) {
                lstRefacciones.Dispose ();
                lstRefacciones = null;
            }

            if (scrViewRepServicios != null) {
                scrViewRepServicios.Dispose ();
                scrViewRepServicios = null;
            }

            if (txtDescFalla != null) {
                txtDescFalla.Dispose ();
                txtDescFalla = null;
            }

            if (txtObservaciones != null) {
                txtObservaciones.Dispose ();
                txtObservaciones = null;
            }
        }
    }
}