using ObjCRuntime;
using UIKit;

namespace App1
{
    public sealed class SimpleTableView : UITableView
    {
        private bool m_scroll = true;
        public SimpleTableView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.Brown;
            ScrollEnabled = true;
            Bounces = true;
        }

        public bool Scroll
        {
            set => m_scroll = value;
        }

        /*public override bool GestureRecognizerShouldBegin(UIGestureRecognizer gestureRecognizer)
        {
            return m_scroll;
        }*/
    }
}