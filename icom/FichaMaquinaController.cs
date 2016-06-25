using System;

using UIKit;

namespace icom
{
	public partial class FichaMaquinaController : UIViewController
	{
		public UIViewController viewmaq { get; set; }
		public String noserie { get; set; }

		public FichaMaquinaController() : base("FichaMaquinaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			scrViewFichaMaquina.ContentSize = new CoreGraphics.CGSize(375, 1883);



			imgMaq.Layer.BorderColor = UIColor.Black.CGColor;
			imgMaq.Layer.BorderWidth = (nfloat)2.0;
			btnEFMotor.TouchUpInside += delegate {
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Motor";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEFTransmision.TouchUpInside += delegate {
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Transmision";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEFEqHid.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Euipo Hidraulico";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEqelec.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Euipo Electrico";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnMandosfin.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Mandos Finales";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnLlantas.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Llantas";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnmangueras.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Mangueras y Conexiones";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnDireccion.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Direccion";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEquipodesgaste.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Equipo de Desgaste";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnPintura.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Pintura";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnTapiceria.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Tapiceria";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			otros.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Otros";
				viewef.viewft = this;

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


