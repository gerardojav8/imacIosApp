﻿using System;
using UIKit;
using CoreGraphics;
namespace icom.globales.ModalViewPicker
{
	public class ModalPickerAnimatedDismissed : UIViewControllerAnimatedTransitioning
	{
		public bool IsPresenting { get; set; }
		float _transitionDuration = 0.25f;

		public ModalPickerAnimatedDismissed()
		{
		}

		public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
		{
			return _transitionDuration;
		}

		public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
		{

			var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
			var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

			var screenBounds = UIScreen.MainScreen.Bounds;
			var fromFrame = fromViewController.View.Frame;

			UIView.AnimateNotify(_transitionDuration,
								 () =>
			{
				toViewController.View.Alpha = 1.0f;

				switch (fromViewController.InterfaceOrientation)
				{
					case UIInterfaceOrientation.Portrait:
						fromViewController.View.Frame = new CGRect(0, screenBounds.Height, fromFrame.Width, fromFrame.Height);
						break;
					case UIInterfaceOrientation.LandscapeLeft:
						fromViewController.View.Frame = new CGRect(screenBounds.Width, 0, fromFrame.Width, fromFrame.Height);
						break;
					case UIInterfaceOrientation.LandscapeRight:
						fromViewController.View.Frame = new CGRect(screenBounds.Width * -1, 0, fromFrame.Width, fromFrame.Height);
						break;
					default:
						break;
				}

			},
								 (finished) => transitionContext.CompleteTransition(true));
		}
	}
}

