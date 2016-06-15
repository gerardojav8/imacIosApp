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
    [Register ("Login")]
    partial class Login
    {
        [Outlet]
        UIKit.UIButton btnLogin { get; set; }


        [Outlet]
        UIKit.UITextField txtPass { get; set; }


        [Outlet]
        UIKit.UITextField txtUsuario { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnLogin != null) {
                btnLogin.Dispose ();
                btnLogin = null;
            }

            if (txtPass != null) {
                txtPass.Dispose ();
                txtPass = null;
            }

            if (txtUsuario != null) {
                txtUsuario.Dispose ();
                txtUsuario = null;
            }
        }
    }
}