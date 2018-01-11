using System;
using CoreGraphics;
using UIKit;

namespace App1
{
    public class SimpleView : UIView
    {
        private const int TopViewMaxHeigth = 150;
        private UIPanGestureRecognizer m_panGestureRecognizer;
        private SimpleTableView m_tableView;
        private NSLayoutConstraint m_topViewHeigthConstraint;
        public SimpleTableSource TableSource { get; } = new SimpleTableSource();

        private nfloat TopViewHeigth
        {
            set => m_topViewHeigthConstraint.Constant = value;
            get => m_topViewHeigthConstraint.Constant;
        }

        public void BuildView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            BackgroundColor = UIColor.DarkGray;

            var topView = new UIView{ TranslatesAutoresizingMaskIntoConstraints = false, BackgroundColor = UIColor.Orange };
            m_tableView = new SimpleTableView { Source = TableSource };

            Add(topView);
            Add(m_tableView);

            topView.TopAnchor.ConstraintEqualTo(LayoutMarginsGuide.TopAnchor).Active = true;
            topView.LeftAnchor.ConstraintEqualTo(LayoutMarginsGuide.LeftAnchor).Active = true;
            topView.RightAnchor.ConstraintEqualTo(LayoutMarginsGuide.RightAnchor).Active = true;
            m_topViewHeigthConstraint = topView.HeightAnchor.ConstraintEqualTo(TopViewMaxHeigth);
            m_topViewHeigthConstraint.Active = true;

            m_tableView.TopAnchor.ConstraintEqualTo(topView.LayoutMarginsGuide.BottomAnchor).Active = true;
            m_tableView.LeftAnchor.ConstraintEqualTo(LayoutMarginsGuide.LeftAnchor).Active = true;
            m_tableView.RightAnchor.ConstraintEqualTo(LayoutMarginsGuide.RightAnchor).Active = true;
            m_tableView.BottomAnchor.ConstraintEqualTo(LayoutMarginsGuide.BottomAnchor).Active = true;

            m_panGestureRecognizer = new UIPanGestureRecognizer();
            m_panGestureRecognizer.AddTarget(HandleScrollPanning);
            m_tableView.AddGestureRecognizer(m_panGestureRecognizer);
            AddGestureRecognizer(m_panGestureRecognizer);
            m_panGestureRecognizer.ShouldRecognizeSimultaneously = (recognizer, gestureRecognizer) => true;
        }

        private void HandleScrollPanning()
        {
            if (m_panGestureRecognizer.State != UIGestureRecognizerState.Ended)
            {
                OnScrollPanned();
            }

            m_panGestureRecognizer.SetTranslation(CGPoint.Empty, m_tableView);
        }

        private void OnScrollPanned()
        {
            var translationY = m_panGestureRecognizer.TranslationInView(m_tableView).Y;
            var isOffsetHigherThanTop = m_tableView.ContentOffset.Y < 0;

            var scrollingDownwards = translationY < 0;
            var isTopViewHidden = TopViewHeigth <= 0;
            var isTopViewPartlyVisible = !isTopViewHidden && TopViewHeigth < TopViewMaxHeigth;
            var isTopViewFullyVisible = !isTopViewHidden && TopViewHeigth >= TopViewMaxHeigth;

            var scrollingDirection = scrollingDownwards ? "downwards" : "upwards";
            var topViewState = isTopViewHidden ? "hidden" : isTopViewPartlyVisible ? "partly visible" : "fully visible";
            //System.Diagnostics.Debug.WriteLine($"translationY: {Math.Round(translationY)}, isScrollingHigherThanTop: {isScrollingHigherThanTop}, scrolling direction: {scrollingDirection}, top view state {topViewState}");
            //System.Diagnostics.Debug.WriteLine(m_tableView.ContentOffset.Y);
            if (isTopViewFullyVisible)
            {
                if (scrollingDownwards)
                {
                    if (!isOffsetHigherThanTop)
                    {
                        TopViewHeigth += translationY;
                    }
                }
                else
                {

                }
            } else if (isTopViewPartlyVisible)
            {
                TopViewHeigth += translationY;
                if (scrollingDownwards && TopViewHeigth > 0)
                {
                    m_tableView.SetContentOffset(CGPoint.Empty, false);
                }
            } else if (isTopViewHidden)
            {
                if (scrollingDownwards)
                {
                }
                else
                {
                    if (isOffsetHigherThanTop)
                    {
                        TopViewHeigth += translationY;
                    }
                    else
                    {

                    }
                    
                }
            }
        }
    }
}