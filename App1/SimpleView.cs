using System;
using System.Diagnostics;
using CoreGraphics;
using UIKit;

namespace App1
{
    public class SimpleView : UIView
    {
        private const int TopViewMaxHeigth = 150;
        private SimpleScrollView m_scrollView;
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
            return verticalOffsetInScrollView;
        }

        private nfloat TableViewVerticalContentOffsetTranslator(nfloat verticalOffsetInTableView)
        {
            var tableOffsetSource = m_tableView.ContentOffset.Y;
            var tableOffsetTarget = verticalOffsetInTableView;
            var tableOffsetDiff = tableOffsetTarget - tableOffsetSource;

            var sharedOffsetSource = m_scrollView.ContentOffset.Y + m_tableView.ContentOffset.Y;
            var sharedOffsetTarget = sharedOffsetSource + tableOffsetDiff;

            //Debug.WriteLine(
              //  $"Shared offset: {Math.Round(sharedOffsetSource)} -> {Math.Round(sharedOffsetTarget)}, TableView: {Math.Round(m_tableView.ContentOffset.Y)} -> {Math.Round(verticalOffsetInTableView)}");

            if (!TableSource.DraggingStartDetected)
            {
                TableSource.DraggingStartDetected = true;
                if (sharedOffsetTarget < 150)
                {
                    TableSource.DraggingStartedInState1 = true;
                }
                TableSource.DraggingStartedInState1 = false;
            }

            if (sharedOffsetTarget < 150)
            {
                var diff = TableSource.DraggingStartedInState1 ? verticalOffsetInTableView - 150 : verticalOffsetInTableView;
                m_scrollView.ContentOffset = new CGPoint(m_scrollView.ContentOffset.X, m_scrollView.ContentOffset.Y + diff);
                return 0;
            }
            m_scrollView.ContentOffset = new CGPoint(m_scrollView.ContentOffset.X, 150);
            return sharedOffsetTarget - 150;
        }
    }
}