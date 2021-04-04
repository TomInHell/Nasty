using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Menu
    {
        public string day;
        public string breakfast;
        public string lunch;
        public string dinner;

        public Menu(string day, string breakfast, string lunch, string dinner)
        {
            this.day = day;
            this.breakfast = breakfast;
            this.lunch = lunch;
            this.dinner = dinner;
        }

    }
}
