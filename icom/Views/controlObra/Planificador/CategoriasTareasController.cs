using System;

using UIKit;
using Alliance.Charts;
using System.Collections.Generic;

namespace icom
{
	public partial class CategoriasTareasController : UIViewController
	{
		List<clsClasificacion> lstClas;
		public CategoriasTareasController() : base("CategoriasTareasController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			clsClasificacion obj1 = new clsClasificacion
			{
				idclasificacion = 1,
				nombre = "Clasificacion 1",
				porcentaje = 50.0f,
				color = UIColor.Blue
			};

			clsClasificacion obj2 = new clsClasificacion
			{
				idclasificacion = 2,
				nombre = "Clasificacion 2",
				porcentaje = 80.0f,
				color = UIColor.Gray
			};

			clsClasificacion obj3 = new clsClasificacion
			{
				idclasificacion = 3,
				nombre = "Clasificacion 3",
				porcentaje = 24.0f,
				color = UIColor.Green
			};

			clsClasificacion obj4 = new clsClasificacion
			{
				idclasificacion = 4,
				nombre = "Clasificacion 4",
				porcentaje = 50.0f,
				color = UIColor.Orange
			};

			clsClasificacion obj5 = new clsClasificacion
			{
				idclasificacion = 5,
				nombre = "Clasificacion 5",
				porcentaje = 15.0f,
				color = UIColor.Red
			};

			lstClas = new List<clsClasificacion>();

			lstClas.Add(obj1);
			lstClas.Add(obj2);
			lstClas.Add(obj3);
			lstClas.Add(obj4);
			lstClas.Add(obj5);

			tblCategorias.SeparatorColor = UIColor.Gray;
			tblCategorias.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

			tblCategorias.Source = new FuenteTablaClasificaciones(this, lstClas);

			btnNuevaCategoria.TouchUpInside += delegate {
				CategoriasAltaController viewca = new CategoriasAltaController();


				viewca.Title = "Nueva Clasificacion";
				this.NavigationController.PushViewController(viewca, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.NavigationController.View, true);
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


