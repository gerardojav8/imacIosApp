using System;
using System.IO;
using Foundation;
using icom.globales;
using MessageUI;

using UIKit;

namespace icom
{
	public partial class PreviewDocsController : UIViewController
	{
		public PreviewDocsController() : base("PreviewDocsController", null)
		{
		}
		LoadingOverlay loadPop;
		public string tituloDocumento { get; set; }
		public string urlDocumento { get; set; }
		MFMailComposeViewController mailCtrl;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			txttitulo.Text = tituloDocumento;
			if (File.Exists(urlDocumento))
			{
				var bounds = UIScreen.MainScreen.Bounds;
				loadPop = new LoadingOverlay(bounds, "Obteniendo Archivo ...");
				this.Add(loadPop);
				webViewDocs.LoadRequest(new NSUrlRequest(new NSUrl(urlDocumento, false)));
				webViewDocs.ScalesPageToFit = true;
			}
			else { 
				funciones.MessageBox("Aviso", "El archivo " + urlDocumento + " no existe");
			}

			webViewDocs.LoadFinished += delegate
			{
				loadPop.Hide();
			};

			if (MFMailComposeViewController.CanSendMail)
			{
				mailCtrl = new MFMailComposeViewController();

				mailCtrl.Finished += async (object sender, MFComposeResultEventArgs e) =>
				{
					Console.WriteLine(e.Result.ToString());
					e.Controller.DismissViewController(true, null);
				};
				btnMail.TouchUpInside += sendMail;
			}
			else { 
				btnMail.TouchUpInside += delegate {
					funciones.MessageBox("Error", "No se puede mandar mail");
				};;
			}




		}

		private void sendMail(object sender, EventArgs e) {
			

			mailCtrl.SetToRecipients(new String[] { "gerardo_jav_8@hotmail.com" });
			mailCtrl.SetSubject("mail test");
			mailCtrl.SetMessageBody("Prueba de mensaje de correo", false);

			this.PresentViewController(mailCtrl, true, null);

		}



	}
}



