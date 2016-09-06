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
    [Register ("DetalleAgendaController")]
    partial class DetalleAgendaController
    {
        [Outlet]
        UIKit.UIButton btnAgregarCalendario { get; set; }


        [Outlet]
        UIKit.UIButton btnEnviar { get; set; }


        [Outlet]
        UIKit.UIButton btnUsuarios { get; set; }


        [Outlet]
        UIKit.UIView ContentDetalleAgencia { get; set; }


        [Outlet]
        UIKit.UIImageView imgFecha { get; set; }


        [Outlet]
        UIKit.UILabel lblComentario { get; set; }


        [Outlet]
        UIKit.UILabel lblLapso { get; set; }


        [Outlet]
        UIKit.UIScrollView scrDetalleAgencia { get; set; }


        [Outlet]
        UIKit.UITableView tblChatDetalleAgencia { get; set; }


        [Outlet]
        UIKit.UITextField txtChatDetalleAgencia { get; set; }


        [Outlet]
        UIKit.UIView viewBarraChat { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAgregarCalendario != null) {
                btnAgregarCalendario.Dispose ();
                btnAgregarCalendario = null;
            }

            if (btnEnviar != null) {
                btnEnviar.Dispose ();
                btnEnviar = null;
            }

            if (btnUsuarios != null) {
                btnUsuarios.Dispose ();
                btnUsuarios = null;
            }

            if (ContentDetalleAgencia != null) {
                ContentDetalleAgencia.Dispose ();
                ContentDetalleAgencia = null;
            }

            if (imgFecha != null) {
                imgFecha.Dispose ();
                imgFecha = null;
            }

            if (lblComentario != null) {
                lblComentario.Dispose ();
                lblComentario = null;
            }

            if (lblLapso != null) {
                lblLapso.Dispose ();
                lblLapso = null;
            }

            if (scrDetalleAgencia != null) {
                scrDetalleAgencia.Dispose ();
                scrDetalleAgencia = null;
            }

            if (tblChatDetalleAgencia != null) {
                tblChatDetalleAgencia.Dispose ();
                tblChatDetalleAgencia = null;
            }

            if (txtChatDetalleAgencia != null) {
                txtChatDetalleAgencia.Dispose ();
                txtChatDetalleAgencia = null;
            }

            if (viewBarraChat != null) {
                viewBarraChat.Dispose ();
                viewBarraChat = null;
            }
        }
    }
}