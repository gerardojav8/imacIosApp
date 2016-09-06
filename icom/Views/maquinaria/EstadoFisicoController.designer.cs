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
    [Register ("EstadoFisicoController")]
    partial class EstadoFisicoController
    {
        [Outlet]
        UIKit.UIButton btnGuardarEF { get; set; }


        [Outlet]
        UIKit.UILabel lbltitulo { get; set; }


        [Outlet]
        UIKit.UITextField txtcalificacion { get; set; }


        [Outlet]
        UIKit.UITextField txtcapacidad { get; set; }


        [Outlet]
        UIKit.UITextView txtComentarios { get; set; }


        [Outlet]
        UIKit.UITextField txtmarca { get; set; }


        [Outlet]
        UIKit.UITextField txttipo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnGuardarEF != null) {
                btnGuardarEF.Dispose ();
                btnGuardarEF = null;
            }

            if (lbltitulo != null) {
                lbltitulo.Dispose ();
                lbltitulo = null;
            }

            if (txtcalificacion != null) {
                txtcalificacion.Dispose ();
                txtcalificacion = null;
            }

            if (txtcapacidad != null) {
                txtcapacidad.Dispose ();
                txtcapacidad = null;
            }

            if (txtComentarios != null) {
                txtComentarios.Dispose ();
                txtComentarios = null;
            }

            if (txtmarca != null) {
                txtmarca.Dispose ();
                txtmarca = null;
            }

            if (txttipo != null) {
                txttipo.Dispose ();
                txttipo = null;
            }
        }
    }
}