using System;
using System.Diagnostics;
using CoreGraphics;
using UIKit;

namespace App1
{
    public class SimpleView : UIView
    {
        private const int TopViewMaxHeigth = 150;
        private ScrollingMode m_mode;
        private SimpleScrollView m_scrollView;
        private nfloat m_start;
        private SimpleTableView m_tableView;
        private UIView m_topView;

        public SimpleTableSource TableSource { get; } = new SimpleTableSource();

        public void BuildView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.DarkGray;

            // CreateViewElements & SetStyle
            m_scrollView = new SimpleScrollView
            {
                Delegate = new SimpleScrollViewDelegate(),
                VerticalContentOffsetTranslator = ScrollViewVerticalContentOffsetTranslator
            };
            var containerView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false, BackgroundColor = UIColor.DarkGray };
            m_topView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false, BackgroundColor = UIColor.Orange };
            m_tableView = new SimpleTableView { Source = TableSource, VerticalContentOffsetTranslator = TableViewVerticalContentOffsetTranslator };

            // SetUpViewHierarchy
            Add(m_scrollView);
            m_scrollView.Add(containerView);
            containerView.Add(m_topView);
            containerView.Add(m_tableView);

            // SetConstraints
            m_scrollView.TopAnchor.ConstraintEqualTo(LayoutMarginsGuide.TopAnchor).Active = true;
            m_scrollView.LeftAnchor.ConstraintEqualTo(LayoutMarginsGuide.LeftAnchor).Active = true;
            m_scrollView.RightAnchor.ConstraintEqualTo(LayoutMarginsGuide.RightAnchor).Active = true;
            m_scrollView.BottomAnchor.ConstraintEqualTo(LayoutMarginsGuide.BottomAnchor).Active = true;

            containerView.TopAnchor.ConstraintEqualTo(m_scrollView.TopAnchor).Active = true;
            containerView.LeftAnchor.ConstraintEqualTo(m_scrollView.LeftAnchor).Active = true;
            containerView.RightAnchor.ConstraintEqualTo(m_scrollView.RightAnchor).Active = true;
            containerView.WidthAnchor.ConstraintEqualTo(m_scrollView.WidthAnchor).Active = true;
            containerView.BottomAnchor.ConstraintEqualTo(m_scrollView.BottomAnchor).Active = true;

            m_topView.TopAnchor.ConstraintEqualTo(containerView.TopAnchor).Active = true;
            m_topView.LeftAnchor.ConstraintEqualTo(containerView.LeftAnchor).Active = true;
            m_topView.RightAnchor.ConstraintEqualTo(containerView.RightAnchor).Active = true;
            m_topView.HeightAnchor.ConstraintEqualTo(TopViewMaxHeigth).Active = true;

            m_tableView.TopAnchor.ConstraintEqualTo(m_topView.BottomAnchor).Active = true;
            m_tableView.LeftAnchor.ConstraintEqualTo(containerView.LeftAnchor).Active = true;
            m_tableView.RightAnchor.ConstraintEqualTo(containerView.RightAnchor).Active = true;
            m_tableView.BottomAnchor.ConstraintEqualTo(containerView.BottomAnchor).Active = true;
            m_tableView.HeightAnchor.ConstraintEqualTo(m_scrollView.HeightAnchor).Active = true;

            // SetUpGetRecognizers
            var gesture = new UIPanGestureRecognizer();
            m_scrollView.AddGestureRecognizer(gesture);
            m_tableView.AddGestureRecognizer(gesture);
            gesture.ShouldRecognizeSimultaneously = (panGesture, scrollGesture) => true;

            // Scroll setup
        }

        private nfloat ScrollViewVerticalContentOffsetTranslator(nfloat verticalOffsetInScrollView)
        {
            //Debug.WriteLine(
            //  $"Shared offset: {Math.Round(m_scrollView.ContentOffset.Y + m_tableView.ContentOffset.Y)} -> {Math.Round(verticalOffsetInScrollView + m_tableView.ContentOffset.Y)}, ScrollView: {Math.Round(m_scrollView.ContentOffset.Y)} -> {Math.Round(verticalOffsetInScrollView)}");
            // Debug.WriteLine("ScrollView: " + verticalOffsetInScrollView);
            return verticalOffsetInScrollView;
        }

        private void TableViewVerticalContentOffsetTranslator(Action<CGPoint> setBaseOffsetProperty, CGPoint rawOffset)
        {
            if (!TableSource.DraggingStartDetected)
            {
                TableSource.DraggingStartDetected = true;
                m_start = m_scrollView.ContentOffset.Y;
            }

            var src = m_tableView.ContentOffset.Y;
            var trg = rawOffset.Y;

            //Debug.WriteLine($"start: {Math.Round(m_start)} src: {Math.Round(src)} trg: {Math.Round(trg)}");

            if (src < float.Epsilon && m_start < 150)
            {
                if (trg < 150 - m_start)
                {
                    // A -> A
                    //Debug.WriteLine("A -> A");
                    m_mode = ScrollingMode.AA;
                    m_scrollView.SetContentOffset(new CGPoint(m_scrollView.ContentOffset.X, m_start + trg), false);
                    if (Math.Abs(src) > float.Epsilon)
                    {
                        setBaseOffsetProperty(new CGPoint(m_tableView.ContentOffset.X, 0));
                    }
                }
                else
                {
                    // A -> B
                    //Debug.WriteLine("A -> B");
                    m_mode = ScrollingMode.AB;
                    setBaseOffsetProperty(new CGPoint(m_tableView.ContentOffset.X, trg - (150 - m_start)));
                    if (Math.Abs(m_scrollView.ContentOffset.Y - 150) > float.Epsilon)
                    {
                        m_scrollView.SetContentOffset(new CGPoint(m_scrollView.ContentOffset.X, 150), false);
                    }
                }
            }
            else
            {
                if (trg < 0)
                {
                    // B -> A
                    //Debug.WriteLine("B -> A");
                    m_mode = ScrollingMode.BA;
                    m_scrollView.SetContentOffset(new CGPoint(m_scrollView.ContentOffset.X, 150 + trg), false);
                    if (Math.Abs(src) > float.Epsilon)
                    {
                        setBaseOffsetProperty(new CGPoint(m_tableView.ContentOffset.X, 0));
                    }
                }
                else
                {
                    // B -> B
                    //Debug.WriteLine("B -> B");
                    m_mode = ScrollingMode.BB;
                    setBaseOffsetProperty(new CGPoint(m_tableView.ContentOffset.X, m_start < 150 ? trg - (150 - m_start) : trg));
                    if (Math.Abs(m_scrollView.ContentOffset.Y - 150) > float.Epsilon)
                    {
                        m_scrollView.SetContentOffset(new CGPoint(m_scrollView.ContentOffset.X, 150), false);
                    }
                }
            }
        }
    }
}