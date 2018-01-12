using System;
using CoreGraphics;
using UIKit;

namespace App1
{
    public sealed class SimpleScrollView : UIScrollView
    {
        public SimpleScrollView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.Gray;
            ShowsVerticalScrollIndicator = false;
            Bounces = false;
        }

        public Func<nfloat, nfloat> VerticalContentOffsetTranslator { private get; set; } = offset => offset;

        public override CGPoint ContentOffset
        {
            get => base.ContentOffset;
            set => base.ContentOffset = new CGPoint(value.X, VerticalContentOffsetTranslator(value.Y));
        }
    }
}