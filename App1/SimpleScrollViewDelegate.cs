using System.Diagnostics;
using CoreGraphics;
using UIKit;

namespace App1
{
    public class SimpleScrollViewDelegate : UIScrollViewDelegate
    {
        public override void WillEndDragging(UIScrollView scrollView, CGPoint velocity, ref CGPoint targetContentOffset)
        {
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            //Debug.WriteLine($"ScrollView: {scrollView.ContentOffset.Y}");
        }
    }
}