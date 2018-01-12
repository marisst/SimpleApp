﻿using System;
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

        public Func<nfloat, nfloat> VerticalContentOffsetTranslator { private get; set; } = offset => offset;

        public override CGPoint ContentOffset
        {
            get => base.ContentOffset;
            set => base.ContentOffset = new CGPoint(value.X, VerticalContentOffsetTranslator(value.Y));
        }
    }
}