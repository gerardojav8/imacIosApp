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

        void ReleaseDesignerOutlets ()
        {
            if (lstMaquinas != null) {
                lstMaquinas.Dispose ();
                lstMaquinas = null;
            }
        }
    }
}