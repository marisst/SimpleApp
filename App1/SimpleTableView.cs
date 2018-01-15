using System;
using CoreGraphics;
using UIKit;

namespace App1
{
    public sealed class SimpleTableView : UITableView
    {
        public SimpleTableView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.Brown;
            ScrollEnabled = true;
        }

        public Action<Action<CGPoint>, CGPoint> VerticalContentOffsetTranslator { private get; set; } = (action, point) => action(point);

        public override CGPoint ContentOffset
        {
            get => base.ContentOffset;
            set => VerticalContentOffsetTranslator(point => base.ContentOffset = point, value);
        }
    }
}