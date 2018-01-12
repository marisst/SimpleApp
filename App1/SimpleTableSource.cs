using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using UIKit;

namespace App1
{
    public class SimpleTableSource : UITableViewSource
    {
        public List<string> Data { private get; set; }
        public bool DraggingStartDetected { get; set; }
        public bool DraggingStartedInState1 { get; set; }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("Cell") ?? new UITableViewCell(UITableViewCellStyle.Default, "Cell");
            cell.TextLabel.Text = Data[indexPath.Row];
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Data.Count;
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            //Debug.WriteLine($"TableView:  {scrollView.ContentOffset.Y}");
        }

        public override void DraggingStarted(UIScrollView scrollView)
        {
            DraggingStartDetected = false;
        }
    }
}