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
    [Register ("MaquinasController")]
    partial class MaquinasController
    {
        [Outlet]
        UIKit.UIButton btnbusqueda { get; set; }


        [Outlet]
        UIKit.UIButton btnTest { get; set; }


        [Outlet]
        UIKit.UITableView lstMaquinas { get; set; }


        [Outlet]
        UIKit.UITextField txtbusqueda { get; set; }

        [Action ("Btnbusqueda_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Btnbusqueda_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnbusqueda != null) {
                btnbusqueda.Dispose ();
                btnbusqueda = null;
            }

            if (lstMaquinas != null) {
                lstMaquinas.Dispose ();
                lstMaquinas = null;
            }

            if (txtbusqueda != null) {
                txtbusqueda.Dispose ();
                txtbusqueda = null;
            }
        }
    }
}