using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Threading.Tasks;
using icom.globales;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using QuickLook;
using System.IO;

namespace icom
{
	public abstract class BubbleCell : UITableViewCell
	{
		public UIImageView BubbleImageView { get; private set; }
		public UILabel MessageLabel { get; private set; }
		public UILabel UsuarioLabel { get; private set; }
		public UIImage BubbleImage { get; set; }
		public UIImage BubbleHighlightedImage { get; set; }
		public UIViewController vcpadre { get; set; }
		private bool blnTieneArchivo;
		private MessageType typebubble;
		LoadingOverlay loadPop;
		HttpClient client;
		String base64file;
		String filename;


		Message msg;

		public Message Message
		{
			get
			{
				return msg;
			}
			set
			{
				msg = value;
				BubbleImageView.Image = BubbleImage;
				BubbleImageView.HighlightedImage = BubbleHighlightedImage;





				if (blnTieneArchivo)
				{
					MessageLabel.Font = UIFont.FromName("Arial-BoldMT", 12f);
					String strmsg = "";

					if (msg.nombre.Equals(""))
					{
						strmsg = msg.fecha + " " + msg.hora + " :" + "\n Archivo:\n" + msg.filename;
					}
					else {
						strmsg = msg.nombre + "\n" + msg.fecha + " " + msg.hora + " :" + "\n Archivo\n" + msg.filename;
					}

					MessageLabel.AttributedText = new NSAttributedString(strmsg, underlineStyle:NSUnderlineStyle.Single);
					MessageLabel.UserInteractionEnabled = true;




					UITapGestureRecognizer tgrLabel = new UITapGestureRecognizer(() =>
					{
						traeArchivo(Int32.Parse(msg.idmensaje));
						//funciones.MessageBox("Aviso", "idmensaje: " + msg.idmensaje);

					});
					MessageLabel.AddGestureRecognizer(tgrLabel);

					MessageLabel.TextColor = UIColor.White;

				}
				else { 

					MessageLabel.Font = UIFont.FromName("Arial", 12f);
					if (msg.nombre.Equals(""))
					{
						MessageLabel.Text = msg.fecha + " " + msg.hora + " :" + "\n" + msg.Text;
					}
					else {
						MessageLabel.Text = msg.nombre + "\n" + msg.fecha + " " + msg.hora + " :" + "\n" + msg.Text;
					}
				
				}


				UsuarioLabel.Font = UIFont.FromName("Arial-BoldMT", 12f);
				UsuarioLabel.Text = "   "+ msg.iniciales +"   ";
				UsuarioLabel.UserInteractionEnabled = true;
				UsuarioLabel.Layer.CornerRadius = 10;
				UsuarioLabel.ClipsToBounds = true;
				UsuarioLabel.BackgroundColor = UIColor.FromRGB(43, 119, 250);
				UsuarioLabel.TextColor = UIColor.White;


				BubbleImageView.UserInteractionEnabled = false;
			}



		}

		async void traeArchivo(int id) {


			
			Boolean resp = await getArchivoMensaje(id);
			if (resp)
			{
				loadPop.Hide();
				char[] delimitantes = { '.' };
				string[] separacion = filename.Split(delimitantes);

				string nombrearchivo = "";
				string extensionarchivo = "";

				for (int i = 0; i < separacion.Length; i++)
				{
					if (i == separacion.Length - 1)
					{
						extensionarchivo = separacion[i];
					}
					else {
						if (i > 0)
						{
							nombrearchivo = "."+ separacion[i];
						}else{
							nombrearchivo = separacion[i];
						}
					}
				}

				string nombretemp = "archivotemp." + extensionarchivo;

				String pathtemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nombretemp);
				if (File.Exists(pathtemp))
				{
					File.Delete(pathtemp);
				}

				//Convertir en archivo y guardar
				Byte[] bytesfile = Convert.FromBase64String(base64file);
				File.WriteAllBytes(pathtemp, bytesfile);

				PreviewDocsController previewDocs = new PreviewDocsController();
				previewDocs.tituloDocumento = nombrearchivo + "." + extensionarchivo;
				previewDocs.urlDocumento = pathtemp;
				vcpadre.NavigationController.PushViewController(previewDocs, true);
			}


		}

		async Task<Boolean> getArchivoMensaje(int idmensajearchivo)
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Obteniendo Archivo ...");
			vcpadre.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getArchivodeMensaje";
			var uri = new Uri(string.Format(url));

			Dictionary<String, String> objpet = new Dictionary<string, string>();
			objpet.Add("idmensaje", idmensajearchivo.ToString());
			var json = JsonConvert.SerializeObject(objpet);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return false;
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI ");
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return false;
			}

			base64file = jsonresponse["archivo"].ToString();
			filename = jsonresponse["nombre"].ToString();
			return true;
		}

		public BubbleCell(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		public BubbleCell()
		{
			Initialize();
		}

		[Export("initWithStyle:reuseIdentifier:")]
		public BubbleCell(UITableViewCellStyle style, string reuseIdentifier)
			: base(style, reuseIdentifier)
		{
			if (reuseIdentifier == IncomingCell.CellId)
			{
				typebubble = MessageType.Incoming;
			}
			else if (reuseIdentifier == IncomingFileCell.CellId)
			{
				typebubble = MessageType.IncomingFile;
			}
			else if (reuseIdentifier == OutgoingCell.CellId) { 
				typebubble = MessageType.Outgoing;
			}
			else if (reuseIdentifier == OutgoingFileCell.CellId)
			{
				typebubble = MessageType.OutgoingFile;
			}
			
			if (reuseIdentifier == IncomingFileCell.CellId || reuseIdentifier == OutgoingFileCell.CellId)
			{
				blnTieneArchivo = true;
			}
			else {
				blnTieneArchivo = false;
			}


			Initialize();
		}

		void Initialize()
		{
			BubbleImageView = new UIImageView
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};

			UsuarioLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};


				MessageLabel = new UILabel
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					Lines = 0,
					PreferredMaxLayoutWidth = 220f
				};





			if (typebubble == MessageType.IncomingFile || typebubble == MessageType.Incoming)
			{
				ContentView.AddSubviews(UsuarioLabel, BubbleImageView, MessageLabel);
			}
			else {
				ContentView.AddSubviews(BubbleImageView, MessageLabel);
			}

		}

		public override void SetSelected(bool selected, bool animated)
		{
			base.SetSelected(selected, animated);
			BubbleImageView.Highlighted = selected;
		}

		protected static UIImage CreateColoredImage(UIColor color, UIImage mask)
		{
			var rect = new CGRect(CGPoint.Empty, mask.Size);
			UIGraphics.BeginImageContextWithOptions(mask.Size, false, mask.CurrentScale);
			CGContext context = UIGraphics.GetCurrentContext();
			mask.DrawAsPatternInRect(rect);
			context.SetFillColor(color.CGColor);
			context.SetBlendMode(CGBlendMode.SourceAtop);
			context.FillRect(rect);
			UIImage result = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return result;
		}

		protected static UIImage CreateBubbleWithBorder(UIImage bubbleImg, UIColor bubbleColor)
		{
			bubbleImg = CreateColoredImage(bubbleColor, bubbleImg);
			CGSize size = bubbleImg.Size;

			UIGraphics.BeginImageContextWithOptions(size, false, 0);
			var rect = new CGRect(CGPoint.Empty, size);
			bubbleImg.Draw(rect);

			var result = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return result;
		}
	}
}

