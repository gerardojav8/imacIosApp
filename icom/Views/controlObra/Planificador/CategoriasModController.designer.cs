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
    [Register ("CategoriasModController")]
    partial class CategoriasModController
    {
        [Outlet]
        UIKit.UIButton btnEliminar { get; set; }


        [Outlet]
        UIKit.UIButton btnGuardar { get; set; }


        [Outlet]
        UIKit.UITextView txtComentario { get; set; }


        [Outlet]
        UIKit.UITextField txtNombreCategoria { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnEliminar != null) {
                btnEliminar.Dispose ();
                btnEliminar = null;
            }

            if (btnGuardar != null) {
                btnGuardar.Dispose ();
                btnGuardar = null;
            }

            if (txtComentario != null) {
                txtComentario.Dispose ();
                txtComentario = null;
            }

            if (txtNombreCategoria != null) {
                txtNombreCategoria.Dispose ();
                txtNombreCategoria = null;
            }
        }
    }
}