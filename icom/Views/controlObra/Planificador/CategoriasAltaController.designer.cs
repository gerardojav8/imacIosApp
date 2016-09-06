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
    [Register ("CategoriasAltaController")]
    partial class CategoriasAltaController
    {
        [Outlet]
        UIKit.UIButton btnGuardar { get; set; }


        [Outlet]
        UIKit.UITextView txtComentarios { get; set; }


        [Outlet]
        UIKit.UITextField txtNombreCategoria { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnGuardar != null) {
                btnGuardar.Dispose ();
                btnGuardar = null;
            }

            if (txtComentarios != null) {
                txtComentarios.Dispose ();
                txtComentarios = null;
            }

            if (txtNombreCategoria != null) {
                txtNombreCategoria.Dispose ();
                txtNombreCategoria = null;
            }
        }
    }
}