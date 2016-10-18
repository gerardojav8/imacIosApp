using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
using icom.globales;
namespace icom
{
	
		public class FuenteTablaMaquinas : UITableViewSource
		{
			protected readonly string ParentCellIdentifierBlack = "ParentCellBlack";
			protected readonly string ParentCellIdentifierWhite = "ParentCellWhite";
			protected readonly string ChildCellIndentifier = "ChildCell";
			protected int currentExpandedIndex = -1;
			protected UIViewController viewparent;			
			private List<clsListadoMaquinas> lstmaq;

			public FuenteTablaMaquinas(UIViewController view, List<clsListadoMaquinas> lst)
			{
				viewparent = view;
				lstmaq = lst;
			}


			public override nint RowsInSection(UITableView tableview, nint section)
			{
				if (currentExpandedIndex > -1)
				{
					return lstmaq.Count + 3;
				}

				return lstmaq.Count;
			}



			void collapseSubItemsAtIndex(UITableView tableView, int index)
			{
				tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
				tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 2, 0) }, UITableViewRowAnimation.Fade);
				tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 3, 0) }, UITableViewRowAnimation.Fade);
			}

			void expandItemAtIndex(UITableView tableView, int index)
			{
				int insertPos = index + 1;
				tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
				tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
				tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
			}

			protected bool isChild(NSIndexPath indexPath)
			{
				return currentExpandedIndex > -1 &&
					   indexPath.Row > currentExpandedIndex &&
					   indexPath.Row <= currentExpandedIndex + 3;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				if (isChild(indexPath))
				{
					//Handle selection of child cell


					if (indexPath.Row == currentExpandedIndex + 1)
					{

						clsListadoMaquinas maquina = lstmaq.ElementAt(currentExpandedIndex);

						ReporteOperador viewro = new ReporteOperador();
						viewro.Title = "Reporte Operador";

						viewro.strNoeconomico = maquina.noeconomico.ToString();
						viewro.strModelo = maquina.modelo.ToString();
						viewro.strNoSerie = maquina.noserie;
						viewro.viewmaq = viewparent;

						viewparent.NavigationController.PushViewController(viewro, false);
						UIView.BeginAnimations(null);
						UIView.SetAnimationDuration(0.7);
						UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
						UIView.CommitAnimations();

						tableView.DeselectRow(indexPath, true);
						return;
					}
					else {
						if (indexPath.Row == currentExpandedIndex + 2)
						{

							clsListadoMaquinas maquina = lstmaq.ElementAt(currentExpandedIndex);
							ReporteServicio viewrs = new ReporteServicio();
							viewrs.Title = "Reporte Servicio";
							viewrs.strNoSerie = maquina.noserie;
							viewrs.viewmaq = viewparent;

							viewparent.NavigationController.PushViewController(viewrs, false);
							UIView.BeginAnimations(null);
							UIView.SetAnimationDuration(0.7);
							UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
							UIView.CommitAnimations();

							tableView.DeselectRow(indexPath, true);
							return;
						}
						else {

							clsListadoMaquinas maquina = lstmaq.ElementAt(currentExpandedIndex);

							FichaMaquinaController viewfm = new FichaMaquinaController();
							viewfm.Title = "Ficha Tecnica de la Maquina";
							viewfm.viewmaq = viewparent;
							viewfm.strNoserie = maquina.noserie;


							viewparent.NavigationController.PushViewController(viewfm, false);
							UIView.BeginAnimations(null);
							UIView.SetAnimationDuration(0.7);
							UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
							UIView.CommitAnimations();

							tableView.DeselectRow(indexPath, true);
							return;

						}
					}
				}

				tableView.BeginUpdates();
				if (currentExpandedIndex == indexPath.Row)
				{
					this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
					currentExpandedIndex = -1;
				}
				else {
					var shouldCollapse = currentExpandedIndex > -1;
					if (shouldCollapse)
					{
						this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
					}
					currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - 3 : indexPath.Row;
					this.expandItemAtIndex(tableView, currentExpandedIndex);
				}
				tableView.EndUpdates();
				tableView.DeselectRow(indexPath, true);
			}

			public override nfloat GetHeightForHeader(UITableView tableView, nint section)
			{
				return (nfloat)0.0;
			}

			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				if (isChild(indexPath))
				{
					return (nfloat)40.0;
				}
				else {
					return (nfloat)70.0;
				}
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{


				if (isChild(indexPath))
				{
					var cell = tableView.DequeueReusableCell(ChildCellIndentifier);
					if (cell == null)
					{
						cell = new UITableViewCell(UITableViewCellStyle.Subtitle, ChildCellIndentifier);
					}

					if (indexPath.Row == currentExpandedIndex + 1)
					{
						cell.TextLabel.Text = "Reporte de Operador";
					}
					else {
						if (indexPath.Row == currentExpandedIndex + 2)
						{
							cell.TextLabel.Text = "Reporte de Servicio";
						}
						else {
							cell.TextLabel.Text = "Ficha de Maquina";
						}
					}
					cell.TextLabel.TextColor = UIColor.FromRGB(54, 74, 97);
					cell.ImageView.Image = null;
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					cell.ContentView.BackgroundColor = UIColor.FromRGB(217, 215, 213);
					return cell;
				}
				else 
			    {
					int indicearreglo = indexPath.Row;

					if (currentExpandedIndex > -1 && indexPath.Row > currentExpandedIndex)
					{
						indicearreglo -= 3;
					}

					UITableViewCell cell;
					if (indicearreglo % 2 == 0)
					{
						cell = tableView.DequeueReusableCell(ParentCellIdentifierWhite) as CustomMaquinasCellWhite;
						if (cell == null)
						{
							cell = new CustomMaquinasCellWhite((NSString)ParentCellIdentifierWhite);
						}
					}
					else {
						
						cell = tableView.DequeueReusableCell(ParentCellIdentifierBlack) as CustomMaquinasCellBlack;
						if (cell == null)
						{
							cell = new CustomMaquinasCellBlack((NSString)ParentCellIdentifierBlack);
						}
					}																

					clsListadoMaquinas maquina = lstmaq.ElementAt(indicearreglo);

					UIImage imgmaq = null;
					String strtipomaq = "";

					switch (maquina.IdTipoMaquina) {
						case 1: imgmaq = UIImage.FromFile("excabadora.png"); break;
						case 2: imgmaq = UIImage.FromFile("revolvedora.png"); break;
						case 3: imgmaq = UIImage.FromFile("trascabo.png"); break;
						default: imgmaq = UIImage.FromFile("trascabo.png"); break;
						
					}
				    
					strtipomaq = Consts.tipomaquinas[maquina.IdTipoMaquina-1];
									
					String leyenda = "Marca: " + maquina.marca + "   Modelo: " + maquina.modelo;

					UIImage imagesem;

					if (maquina.tieneReporte == 1)
					{
						imagesem = UIImage.FromFile("red.png");
					}
					else {
						imagesem = UIImage.FromFile("green.png");
					}


					((CustomMaquinasCell)cell).UpdateCell(strtipomaq, leyenda, imgmaq, imagesem);


					cell.Accessory = UITableViewCellAccessory.None;					
					return cell;
				}

			}
		}

		public class CustomMaquinasCell : UITableViewCell
		{
			UILabel headingLabel, subheadingLabel;
			UIImageView imageView;
			UIImageView imageView2;

			public CustomMaquinasCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
			{

				SelectionStyle = UITableViewCellSelectionStyle.Gray;				
				imageView = new UIImageView();
				imageView2 = new UIImageView();
				headingLabel = new UILabel()
				{
					Font = UIFont.FromName("Arial", 22f),
					TextColor = UIColor.FromRGB(54, 74, 97),
					BackgroundColor = UIColor.Clear
				};
				subheadingLabel = new UILabel()
				{
					Font = UIFont.FromName("Arial", 13f),
					TextColor = UIColor.FromRGB(54, 74, 97),
					TextAlignment = UITextAlignment.Left,
					BackgroundColor = UIColor.Clear
				};
				ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel, imageView, imageView2 });

			}
			public void UpdateCell(string caption, string subtitle, UIImage image, UIImage image2)
			{
				imageView.Image = image;
				imageView2.Image = image2;
				headingLabel.Text = caption;
				subheadingLabel.Text = subtitle;
			}
			public override void LayoutSubviews()
			{
				base.LayoutSubviews();

				imageView.Frame = new CGRect(4, 4, 50, 50);
				headingLabel.Frame = new CGRect(70, 4, ContentView.Bounds.Width - 63, 25);
				imageView2.Frame = new CGRect(255, 6, 25, 25);
				subheadingLabel.Frame = new CGRect(70, 32, 500, 20);
			}

		}

	public class CustomMaquinasCellBlack : CustomMaquinasCell {
		public CustomMaquinasCellBlack(NSString cellId) : base(cellId) { 
			ContentView.BackgroundColor = UIColor.FromRGB(237, 242, 248);
		}
	}

	public class CustomMaquinasCellWhite : CustomMaquinasCell
	{
		public CustomMaquinasCellWhite(NSString cellId) : base(cellId)
		{
			ContentView.BackgroundColor = UIColor.FromRGB(216, 223, 231);
		}
	}

}

