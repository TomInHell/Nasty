using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Weekday
    {
        Menu menu;
        public string day;

        public Weekday(Menu menu)
        {
            this.menu = menu;
            day = menu.day;
        }

        public string show()
        {
            return ($"{menu.breakfast}#{menu.lunch}#{menu.dinner}");
        }

        public void rebild(Menu obj)
        {
            menu = null;
            menu = obj;
        }
    }
}
