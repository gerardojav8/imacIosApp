using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
namespace icom
{
	public class ChatSource : UITableViewSource
	{
		static readonly NSString IncomingCellId = new NSString("Incoming");
		static readonly NSString OutgoingCellId = new NSString("Outgoing");
		static readonly NSString IncomingFileCellId = new NSString("IncomingFile");
		static readonly NSString OutgoingFileCellId = new NSString("OutgoingFile");
		private UIViewController vcontroller { get; set; }

		IList<Message> messages;

		readonly BubbleCell[] sizingCells;

		public ChatSource(IList<Message> messages, UIViewController viewpadre)
		{
			if (messages == null)
				throw new ArgumentNullException("messages");

			this.messages = messages;
			sizingCells = new BubbleCell[4];

			vcontroller = viewpadre;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return messages.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			BubbleCell cell = null;
			Message msg = messages[indexPath.Row];

				
			cell = (BubbleCell)tableView.DequeueReusableCell(GetReuseId(msg.Type));
			cell.vcpadre = vcontroller;
			cell.Message = msg;

			return cell;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			Message msg = messages[indexPath.Row];
			return CalculateHeightFor(msg, tableView);
		}

		public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
		{
			Message msg = messages[indexPath.Row];
			return CalculateHeightFor(msg, tableView);
		}

		nfloat CalculateHeightFor(Message msg, UITableView tableView)
		{
			var index = (int)msg.Type;
			BubbleCell cell = sizingCells[index];

			if (cell == null)
			{
				cell = sizingCells[index] = (BubbleCell)tableView.DequeueReusableCell(GetReuseId(msg.Type));
				cell.vcpadre = vcontroller;
			}

			cell.Message = msg;

			cell.SetNeedsLayout();
			cell.LayoutIfNeeded();
			CGSize size = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize);
			return NMath.Ceiling(size.Height) + 1;
		}

		NSString GetReuseId(MessageType msgType)
		{
			switch (msgType) {
				case MessageType.Incoming: return IncomingCellId ;				
				case MessageType.Outgoing: return OutgoingCellId ;
				case MessageType.IncomingFile: return IncomingFileCellId;
				case MessageType.OutgoingFile: return OutgoingFileCellId;
				default: return null;
			}				

		}
	}
}

