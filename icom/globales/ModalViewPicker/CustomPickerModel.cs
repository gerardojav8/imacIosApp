using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace icom.globales.ModalViewPicker
{
	public class CustomPickerModel : UIPickerViewModel
	{
		private List<string> _itemsList;

		public CustomPickerModel(List<string> itemsList)
		{
			_itemsList = itemsList;
		}

		public override nint GetComponentCount(UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
		{
			return _itemsList.Count;
		}

		public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
		{
			var label = new UILabel(new CGRect(0, 0, 300, 37))
			{
				BackgroundColor = UIColor.Clear,
				Text = _itemsList[(int)row],
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.BoldSystemFontOfSize(22.0f)
			};

			return label;
		}
	}
}

