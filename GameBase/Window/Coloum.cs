using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class Coloum
    {
        ///<summary>
        ///absolute Position in the screen.
        ///</summary>
        public Vector pos;
        /// <summary>
        /// Coloum' text content
        /// </summary>
        public string str;
        /// <summary>
        /// a "int" command returned if the coloum gots "apply" 
        /// </summary>
        public int reply;
        /// <summary>
        /// it is selected in a window or not.
        /// </summary>
        public bool selected;
        static int deny_command = -1;       
        public Coloum(int _x,int _y,string _str, int _reply)
        {
            pos= new Vector(_x,_y);
            str = _str;
            reply = _reply;
        }

        public void is_selected() {
            selected = true;
        }
        public void is_left()
        {
            selected = false;
        }
        public int is_applied()
        {
            return reply;
        }
        public void draw(Drawing d)
        {
            new RichText(str, FontID.Medium, selected ? Color.Yellow : Color.White).Draw(d, pos, DepthID.Message);
        }
    }
    
    class Blank : Coloum
    {
        public Blank(int _x, int _y, string _str, int _reply) : base(_x,_y,_str,_reply)
        {    }

        public new int is_selected()
        {
            base.is_selected();
            Console.WriteLine(str);
            Console.Write("changes to : ");
            str = Console.ReadLine();
            base.is_left();
            return base.is_applied();
        }
    }
}
