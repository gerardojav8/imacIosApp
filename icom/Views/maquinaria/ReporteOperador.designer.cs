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
    [Register ("ReporteOperador")]
    partial class ReporteOperador
    {
        [Outlet]
        UIKit.UIButton btnAtiende { get; set; }


        [Outlet]
        UIKit.UIButton btnEquipo { get; set; }


        [Outlet]
        UIKit.UIButton btnGuardar { get; set; }


        [Outlet]
        UIKit.UIButton btnModelo { get; set; }


        [Outlet]
        UIKit.UIButton btnReporto { get; set; }


        [Outlet]
        UIKit.UIButton btnTipoFalla { get; set; }


        [Outlet]
        UIKit.UIView ContentView { get; set; }


        [Outlet]
        UIKit.UIScrollView ScrView { get; set; }


        [Outlet]
        UIKit.UITextField txtatiende { get; set; }


        [Outlet]
        UIKit.UITextView txtDescripcion { get; set; }


        [Outlet]
        UIKit.UITextField txtequipo { get; set; }


        [Outlet]
        UIKit.UITextField txtfechahora { get; set; }


        [Outlet]
        UIKit.UITextField txtfipofalla { get; set; }


        [Outlet]
        UIKit.UITextField txtFolio { get; set; }


        [Outlet]
        UIKit.UITextField txtkmho { get; set; }


        [Outlet]
        UIKit.UITextField txtkmhorometro { get; set; }


        [Outlet]
        UIKit.UITextField txtmodelo { get; set; }


        [Outlet]
        UIKit.UITextField txtnoserie { get; set; }


        [Outlet]
        UIKit.UITextField txtreporto { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAtiende != null) {
                btnAtiende.Dispose ();
                btnAtiende = null;
            }

            if (btnGuardar != null) {
                btnGuardar.Dispose ();
                btnGuardar = null;
            }

            if (btnReporto != null) {
                btnReporto.Dispose ();
                btnReporto = null;
            }

            if (btnTipoFalla != null) {
                btnTipoFalla.Dispose ();
                btnTipoFalla = null;
            }

            if (ContentView != null) {
                ContentView.Dispose ();
                ContentView = null;
            }

            if (ScrView != null) {
                ScrView.Dispose ();
                ScrView = null;
            }

            if (txtatiende != null) {
                txtatiende.Dispose ();
                txtatiende = null;
            }

            if (txtDescripcion != null) {
                txtDescripcion.Dispose ();
                txtDescripcion = null;
            }

            if (txtequipo != null) {
                txtequipo.Dispose ();
                txtequipo = null;
            }

            if (txtfechahora != null) {
                txtfechahora.Dispose ();
                txtfechahora = null;
            }

            if (txtfipofalla != null) {
                txtfipofalla.Dispose ();
                txtfipofalla = null;
            }

            if (txtFolio != null) {
                txtFolio.Dispose ();
                txtFolio = null;
            }

            if (txtkmho != null) {
                txtkmho.Dispose ();
                txtkmho = null;
            }

            if (txtmodelo != null) {
                txtmodelo.Dispose ();
                txtmodelo = null;
            }

            if (txtnoserie != null) {
                txtnoserie.Dispose ();
                txtnoserie = null;
            }

            if (txtreporto != null) {
                txtreporto.Dispose ();
                txtreporto = null;
            }
        }
    }
}