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
    [Register ("ObrasController")]
    partial class ObrasController
    {
        [Outlet]
        UIKit.UIButton btnBusquedaObra { get; set; }


        [Outlet]
        UIKit.UIButton btnModificarObra { get; set; }


        [Outlet]
        UIKit.UIButton btnNuevaObra { get; set; }


        [Outlet]
        UIKit.UITableView tblObras { get; set; }


        [Outlet]
        UIKit.UITextField txtBusquedaObra { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBusquedaObra != null) {
                btnBusquedaObra.Dispose ();
                btnBusquedaObra = null;
            }

            if (btnModificarObra != null) {
                btnModificarObra.Dispose ();
                btnModificarObra = null;
            }

            if (btnNuevaObra != null) {
                btnNuevaObra.Dispose ();
                btnNuevaObra = null;
            }

            if (tblObras != null) {
                tblObras.Dispose ();
                tblObras = null;
            }

            if (txtBusquedaObra != null) {
                txtBusquedaObra.Dispose ();
                txtBusquedaObra = null;
            }
        }
    }
}