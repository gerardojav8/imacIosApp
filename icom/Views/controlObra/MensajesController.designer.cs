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
    [Register ("MensajesController")]
    partial class MensajesController
    {
        [Outlet]
        UIKit.UIButton btnArchivo { get; set; }


        [Outlet]
        UIKit.UIButton btnenviar { get; set; }


        [Outlet]
        UIKit.UITableView tblChat { get; set; }


        [Outlet]
        UIKit.UITextField txtmensaje { get; set; }


        [Outlet]
        UIKit.UIView viewbarrainf { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnArchivo != null) {
                btnArchivo.Dispose ();
                btnArchivo = null;
            }

            if (btnenviar != null) {
                btnenviar.Dispose ();
                btnenviar = null;
            }

            if (tblChat != null) {
                tblChat.Dispose ();
                tblChat = null;
            }

            if (txtmensaje != null) {
                txtmensaje.Dispose ();
                txtmensaje = null;
            }

            if (viewbarrainf != null) {
                viewbarrainf.Dispose ();
                viewbarrainf = null;
            }
        }
    }
}