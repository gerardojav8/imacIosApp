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
    [Register ("NuevoEventoController")]
    partial class NuevoEventoController
    {
        [Outlet]
        UIKit.UIButton btnaceptar { get; set; }


        [Outlet]
        UIKit.UIButton btnAgregarAsistentes { get; set; }


        [Outlet]
        UIKit.UIButton btnBuscarUsuarios { get; set; }


        [Outlet]
        UIKit.UIButton btnEliminarAsistentes { get; set; }


        [Outlet]
        UIKit.UIButton btnfecha { get; set; }


        [Outlet]
        UIKit.UIButton btnFechafin { get; set; }


        [Outlet]
        UIKit.UIView ContentNuevoEvento { get; set; }


        [Outlet]
        UIKit.UIScrollView scrNuevoEvento { get; set; }


        [Outlet]
        UIKit.UISwitch swNotificarInvitados { get; set; }


        [Outlet]
        UIKit.UISwitch swTodoeldia { get; set; }


        [Outlet]
        UIKit.UITableView tblAsistentes { get; set; }


        [Outlet]
        UIKit.UITextField txtAsistentes { get; set; }


        [Outlet]
        UIKit.UITextView txtComentario { get; set; }


        [Outlet]
        UIKit.UITextField txtfechaevento { get; set; }


        [Outlet]
        UIKit.UITextField txtFechaFin { get; set; }


        [Outlet]
        UIKit.UITextField txtHoraFin { get; set; }


        [Outlet]
        UIKit.UITextField txthorainicio { get; set; }


        [Outlet]
        UIKit.UITextField txtTitulo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnaceptar != null) {
                btnaceptar.Dispose ();
                btnaceptar = null;
            }

            if (btnAgregarAsistentes != null) {
                btnAgregarAsistentes.Dispose ();
                btnAgregarAsistentes = null;
            }

            if (btnBuscarUsuarios != null) {
                btnBuscarUsuarios.Dispose ();
                btnBuscarUsuarios = null;
            }

            if (btnEliminarAsistentes != null) {
                btnEliminarAsistentes.Dispose ();
                btnEliminarAsistentes = null;
            }

            if (btnfecha != null) {
                btnfecha.Dispose ();
                btnfecha = null;
            }

            if (btnFechafin != null) {
                btnFechafin.Dispose ();
                btnFechafin = null;
            }

            if (ContentNuevoEvento != null) {
                ContentNuevoEvento.Dispose ();
                ContentNuevoEvento = null;
            }

            if (scrNuevoEvento != null) {
                scrNuevoEvento.Dispose ();
                scrNuevoEvento = null;
            }

            if (swNotificarInvitados != null) {
                swNotificarInvitados.Dispose ();
                swNotificarInvitados = null;
            }

            if (swTodoeldia != null) {
                swTodoeldia.Dispose ();
                swTodoeldia = null;
            }

            if (tblAsistentes != null) {
                tblAsistentes.Dispose ();
                tblAsistentes = null;
            }

            if (txtAsistentes != null) {
                txtAsistentes.Dispose ();
                txtAsistentes = null;
            }

            if (txtComentario != null) {
                txtComentario.Dispose ();
                txtComentario = null;
            }

            if (txtfechaevento != null) {
                txtfechaevento.Dispose ();
                txtfechaevento = null;
            }

            if (txtFechaFin != null) {
                txtFechaFin.Dispose ();
                txtFechaFin = null;
            }

            if (txtHoraFin != null) {
                txtHoraFin.Dispose ();
                txtHoraFin = null;
            }

            if (txthorainicio != null) {
                txthorainicio.Dispose ();
                txthorainicio = null;
            }

            if (txtTitulo != null) {
                txtTitulo.Dispose ();
                txtTitulo = null;
            }
        }
    }
}