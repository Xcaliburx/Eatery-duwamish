using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EateryDuwamish.UserControl
{
    public class PageEventArgs
    {
        public int CurrentPageIndex;
        public PageEventArgs(int PageIndex)
        {
            CurrentPageIndex = PageIndex;
        }
    }
}