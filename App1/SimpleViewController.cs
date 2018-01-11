using System.Collections.Generic;
using UIKit;

namespace App1
{
    public class SimpleViewController : UIViewController
    {
        private SimpleView m_simpleView;

        public override void LoadView()
        {
            m_simpleView = new SimpleView();
            m_simpleView.BuildView();
            View = m_simpleView;
        }

        public override void ViewDidLoad()
        {
            m_simpleView.TableSource.Data = new List<string>
            {
                "Chathurika",
                "Chandu",
                "Lakmal",
                "Mariss",
                "Morten",
                "Prasad",
                "Rizan",
                "Sandun",
                "Sudantha",
                "Tharika",
                "Runar",
                "Peter",
                "Lars",
                "Jonas",
                "Karoline",
                "Phuong",
                "Malin"
            };
        }
    }
}