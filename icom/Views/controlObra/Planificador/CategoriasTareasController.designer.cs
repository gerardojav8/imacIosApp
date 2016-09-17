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
    [Register ("CategoriasTareasController")]
    partial class CategoriasTareasController
    {
        [Outlet]
        UIKit.UIButton btnBusquedaCategoria { get; set; }


        [Outlet]
        UIKit.UIButton btnGrafica { get; set; }


        [Outlet]
        UIKit.UIButton btnModificarCategoria { get; set; }


        [Outlet]
        UIKit.UIButton btnNuevaCategoria { get; set; }


        [Outlet]
        UIKit.UITableView tblCategorias { get; set; }


        [Outlet]
        UIKit.UITextField txtbusquedaCategoria { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBusquedaCategoria != null) {
                btnBusquedaCategoria.Dispose ();
                btnBusquedaCategoria = null;
            }

            if (btnGrafica != null) {
                btnGrafica.Dispose ();
                btnGrafica = null;
            }

            if (btnModificarCategoria != null) {
                btnModificarCategoria.Dispose ();
                btnModificarCategoria = null;
            }

            if (btnNuevaCategoria != null) {
                btnNuevaCategoria.Dispose ();
                btnNuevaCategoria = null;
            }

            if (tblCategorias != null) {
                tblCategorias.Dispose ();
                tblCategorias = null;
            }

            if (txtbusquedaCategoria != null) {
                txtbusquedaCategoria.Dispose ();
                txtbusquedaCategoria = null;
            }
        }
    }
}