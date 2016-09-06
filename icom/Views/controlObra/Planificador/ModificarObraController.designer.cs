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
    [Register ("ModificarObraController")]
    partial class ModificarObraController
    {
        [Outlet]
        UIKit.UIButton btnEliminarObra { get; set; }


        [Outlet]
        UIKit.UIButton btnModificarObra { get; set; }


        [Outlet]
        UIKit.UITextView txtDescripcionobra { get; set; }


        [Outlet]
        UIKit.UITextField txtNombreObra { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnEliminarObra != null) {
                btnEliminarObra.Dispose ();
                btnEliminarObra = null;
            }

            if (btnModificarObra != null) {
                btnModificarObra.Dispose ();
                btnModificarObra = null;
            }

            if (txtDescripcionobra != null) {
                txtDescripcionobra.Dispose ();
                txtDescripcionobra = null;
            }

            if (txtNombreObra != null) {
                txtNombreObra.Dispose ();
                txtNombreObra = null;
            }
        }
    }
}