using System;
using System.IO;
using Foundation;
using icom.globales;

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
		}



		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}



