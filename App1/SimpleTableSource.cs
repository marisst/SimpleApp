using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace App1
{
    public class SimpleTableSource : UITableViewSource
    {
        public List<string> Data { private get; set; }

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
    }
}