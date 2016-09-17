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
    [Register ("NuevaTareaController")]
    partial class NuevaTareaController
    {
        [Outlet]
        UIKit.UIButton btnFinal { get; set; }


        [Outlet]
        UIKit.UIButton btnGuardar { get; set; }


        [Outlet]
        UIKit.UIButton btnInicio { get; set; }


        [Outlet]
        UIKit.UISwitch swTodoDia { get; set; }


        [Outlet]
        UIKit.UITextField txtCategoria { get; set; }


        [Outlet]
        UIKit.UITextField txtFinal { get; set; }


        [Outlet]
        UIKit.UITextField txtInicio { get; set; }


        [Outlet]
        UIKit.UITextView txtNotas { get; set; }


        [Outlet]
        UIKit.UITextField txtObra { get; set; }


        [Outlet]
        UIKit.UITextField txtPorcentaje { get; set; }


        [Outlet]
        UIKit.UITextField txtTitulo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnFinal != null) {
                btnFinal.Dispose ();
                btnFinal = null;
            }

            if (btnGuardar != null) {
                btnGuardar.Dispose ();
                btnGuardar = null;
            }

            if (btnInicio != null) {
                btnInicio.Dispose ();
                btnInicio = null;
            }

            if (swTodoDia != null) {
                swTodoDia.Dispose ();
                swTodoDia = null;
            }

            if (txtCategoria != null) {
                txtCategoria.Dispose ();
                txtCategoria = null;
            }

            if (txtFinal != null) {
                txtFinal.Dispose ();
                txtFinal = null;
            }

            if (txtInicio != null) {
                txtInicio.Dispose ();
                txtInicio = null;
            }

            if (txtNotas != null) {
                txtNotas.Dispose ();
                txtNotas = null;
            }

            if (txtObra != null) {
                txtObra.Dispose ();
                txtObra = null;
            }

            if (txtPorcentaje != null) {
                txtPorcentaje.Dispose ();
                txtPorcentaje = null;
            }

            if (txtTitulo != null) {
                txtTitulo.Dispose ();
                txtTitulo = null;
            }
        }
    }
}