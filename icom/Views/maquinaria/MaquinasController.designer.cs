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
        UIKit.UIButton btnAgregar { get; set; }


        [Outlet]
        UIKit.UIButton btnSearch { get; set; }


        [Outlet]
        UIKit.UITableView lstMaquinas { get; set; }


        [Outlet]
        UIKit.UITextField txtSearch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAgregar != null) {
                btnAgregar.Dispose ();
                btnAgregar = null;
            }

            if (btnSearch != null) {
                btnSearch.Dispose ();
                btnSearch = null;
            }

            if (lstMaquinas != null) {
                lstMaquinas.Dispose ();
                lstMaquinas = null;
            }

            if (txtSearch != null) {
                txtSearch.Dispose ();
                txtSearch = null;
            }
        }
    }
}