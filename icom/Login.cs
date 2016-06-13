using System;

using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
namespace icom
{
	public partial class Login : UIViewController
	{
		public Login () : base ("Login", null)
		{
		}
		HttpClient client;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnLogin.TouchUpInside += loginboton;

		}

		async void loginboton(object sender, EventArgs e)
		{
			Boolean resp = await login();

			if (resp)
			{
				Principal viewprin = new Principal();
				viewprin.Title = "I.C.O.M";
				this.NavigationController.PushViewController(viewprin, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View,true);
				UIView.CommitAnimations();
			}
		}

		public async Task<Boolean> login()
		{
			String usuario = txtUsuario.Text;
			String pass = txtPass.Text;

			if (usuario.Equals(""))
			{
				MessageBox("Error", "Debe de ingresar usuario");
				return false;
			}

			if (pass.Equals(""))
			{
				MessageBox("Error", "Debe de ingresar password");
				return false;
			}

			client = new HttpClient();
			String RestUrl = "http://192.168.0.32/icomtoken/oauth2/token";
			var uri = new Uri(string.Format(RestUrl));


            var content = new StringContent("username="+usuario+"&password="+pass+"&grant_type=password&client_id=099153c2625149bc8ecb3e85e03f0022", Encoding.UTF8, "application/x-www-form-urlencoded");

			HttpResponseMessage response = null;

			response = await client.PostAsync(uri, content);

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();

			var jsonrequest = JObject.Parse(responseString);

			var jtokenerror = jsonrequest["error_description"];


			if (jtokenerror != null)
			{
				string error = jtokenerror.ToString();
				MessageBox("Error", error);
				return false;
			}

			return true;
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		private void MessageBox(string titulo, string mensaje)
		{
			using (UIAlertView Alerta = new UIAlertView())
			{
				Alerta.Title = titulo;
				Alerta.Message = mensaje;
				Alerta.AddButton("Enterado");
				Alerta.Show();
			};
		}
	}
}


