using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class Coloum
    {
        #region Variables
        ///<summary>
        ///absolute Position in the screen.
        ///</summary>
        public Vector pos;
        /// <summary>
        /// Coloum' text content
        /// </summary>
        public string str;
        /// <summary>
        /// a int enum "Command" command returned if the coloum gots "apply" 
        /// </summary>
        public Command reply;
        /// <summary>
        /// it is selected in a window or not.
        /// </summary>
        public bool selected;
        /// <summary>
        /// default distance that between str and content. is used in Blank
        /// </summary>
        protected const int default_distance = 10;
        /// <summary>
        /// Coloum size
        /// </summary>
        public int w, h;
        protected int dx = 0, dy = 0;
        /// <summary>
        /// Coloumでは無意味。書き換えの内容に当たる。
        /// </summary>
        public string content;
        protected const int maximumOfCharsEachLine = 10;
        protected const FontID default_fontId = FontID.Medium;
        #endregion

        public Coloum(int _x, int _y, string _str, Command _reply, int _w = -1, int _h = -1)
        {
            pos = new Vector(_x, _y);
            str = _str;
            reply = _reply;
            w = _w; h = _h;
            PoorString pst = new PoorString(_str, maximumOfCharsEachLine, default_fontId, false);
            PoorString pco = new PoorString("", maximumOfCharsEachLine, default_fontId, false);
            if (w == -1)
            {
                if (pst.CountChar() <= 0)
                {
                    w = pst.str.Length * pst.getCharSizeX();
                } else { w = maximumOfCharsEachLine * pst.getCharSizeX(); }
            }
            if (h == -1) {
                h = (pst.CountChar() + 1) * pst.getCharSizeY();
            }
            if (w == -2)
            {
                if (pst.CountChar() <= 0)
                {
                    w = pst.str.Length * pst.getCharSizeX();
                }
                else { w = maximumOfCharsEachLine * pst.getCharSizeX(); }
                w += dx;
                if (pco.CountChar() <= 0)
                {
                    w = pco.str.Length * pco.getCharSizeX();
                }
                else { w = maximumOfCharsEachLine * pco.getCharSizeX(); }
            }
            if (h == -2)
            {
                h = pst.CountChar() > pco.CountChar() ? (pst.CountChar() + 1) * pst.getCharSizeY() : (pco.CountChar() + 1) * pco.getCharSizeY();
                h += dy;
            }
        }
        #region activity
        public void change_content(string c) { content = c; }
        public virtual Command update(KeyManager k, MouseManager m)
        {
            if (m != null) { update_with_mouse_manager(m); }
            if (k != null && selected) { return update_with_key_manager(k); }
            return Command.nothing;
        }
        public virtual Command update_with_key_manager(KeyManager k) {
            if (k.IsKeyDown(KeyID.Select))
            {
                return is_applied();
            }
            if (k.IsKeyDown(KeyID.Cancel))
            {
                return is_left();
            }
            return Command.nothing;
        }
        public virtual void update_with_mouse_manager(MouseManager m) {
            if (m.IsButtomDown(MouseButton.Left) && PosInside(m.MousePosition()))
            {
                is_selected();
            }
        }
        public virtual bool PosInside(Vector v){
            if(v.X < pos.X+w && v.X> pos.X && v.Y < pos.Y + h && v.Y > pos.Y) { return true; }
            return false;    
        }
        public virtual void is_selected() {
            selected = true;
        }
        public virtual Command is_left()
        {
            selected = false;
            return Command.left_and_go_back;
        }
        public virtual Command is_applied()
        {
            selected = false;
            return reply;
        }
        #endregion
        public virtual void draw(Drawing d)
        {
            if (str == "")
            {
                new RichText(str, default_fontId, selected ? Color.Yellow : Color.White).Draw(d, pos, DepthID.Message);
            }
        }
        public virtual void draw(Drawing d,int x, int y)
        {
            if (str == "")
            {
                Vector posNew = new Vector(pos.X + x, pos.Y + y);
                new RichText(str, default_fontId, selected ? Color.Yellow : Color.White).Draw(d, posNew, DepthID.Message);
            }
        }
    }

    class Blank : Coloum
    {
        /// <summary>
        /// 内容書き換え可能の項目
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_str">この項目の名前</param>
        /// <param name="_content">書き換えできる内容</param>
        /// <param name="_reply"></param>
        /// <param name="_w">-1でstrによって、-2でstrとcontentのサイズによって 自身のw,hを求める</param>
        /// <param name="_dx">名前と内容の距離差</param>
        public Blank(int _x, int _y, string _str, string _content, Command _reply, int _w=-2,int _h=-2,int _dx = default_distance, int _dy = 0) : base(_x, _y, _str, _reply,_w,_h)
        {
            content = _content;
            dx = _dx;
            dy = _dy;
        }

        public override void draw(Drawing d) {
            if (str == "")
            {
                new RichText(str, FontID.Medium, selected ? Color.Yellow : Color.White).Draw(d, pos, DepthID.Message);
            }
            if (content == "")
            {
                new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(pos.X + dx, pos.Y + dy), DepthID.Message);
            }
        }
        public override void draw(Drawing d, int x, int y)
        {
            Vector posNew = new Vector(pos.X + x, pos.Y + y);
            if (str == "")
            {
                new RichText(str, FontID.Medium, selected ? Color.Yellow : Color.White).Draw(d, posNew, DepthID.Message);
            }
            if (content == "")
            {
                new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(posNew.X + dx, posNew.Y + dy), DepthID.Message);
            }
        }
        public override Command is_applied()
        {
            Console.WriteLine(str);
            Console.Write("changes to : ");
            content = Console.ReadLine();
            return base.is_applied();
        }
    }// class Blank end

    class Button : Coloum
    {
        /// <summary>
        /// Is str a key of Texture. true- it is. false-it is not
        /// </summary>
        protected bool useTexture = false;
        public Button(int _x, int _y, string _str, string _content, Command _reply, bool _useTexture, int _dx = default_distance, int _dy = 0) : base(_x, _y, _str, _reply)
        {
            useTexture = _useTexture;
            content = _content;
            dx = _dx;
            dy = _dy;
        }

        public override void draw(Drawing d)
        {
            if (!useTexture)
            {
                base.draw(d);
                if (content == "")
                {
                    new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(pos.X + dx, pos.Y + dy), DepthID.Message);
                }
            }
            else
            {
                d.Draw(pos,DataBase.getTex(str),DepthID.Message);
                if (content == "")
                {
                    new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(pos.X + dx, pos.Y + dy), DepthID.Message);
                }
            }
        }
        public override void draw(Drawing d, int x, int y)
        {
            Vector posNew = new Vector(pos.X + x, pos.Y + y);
            if (!useTexture)
            {
                base.draw(d);
                if (content == "")
                {
                    new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(posNew.X + dx, posNew.Y + dy), DepthID.Message);
                }
            }
            else
            {
                d.Draw(posNew, DataBase.getTex(str), DepthID.Message);
                if (content == "")
                {
                    new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(posNew.X + dx, posNew.Y + dy), DepthID.Message);
                }
            }
        }
    }

    /// <summary>
    /// UnitTypeDataBaseとUnitTypeが存在する前提の、専用unitTypeの画像のbutton
    /// </summary>
    class UTDutButton : Coloum {
        private UnitType ut;
        /// <summary>
        /// コンストラクタ。UnitTypeDataBaseがあることを確認してください。
        /// </summary>
        /// <param name="_str">UnitTypeDataBaseの中身にアクセスするためのkey string</param>
        /// <param name="_content">もし文字を書くなら</param>
        /// <param name="_reply">Buttonが押された時の返事Command</param>
        public UTDutButton(int _x, int _y, string _str, string _content, Command _reply,int _dx = default_distance, int _dy = 0) : base(_x, _y, _str, _reply)
        {
            content = _content;
            dx = _dx; dy = _dy;
            ut = DataBase.getUnitType(str);
        }



        public override void draw(Drawing d)
        {
            if (str == "")
            {
                d.Draw(pos, DataBase.getTex(ut.texture_name), DataBase.getRectFromTextureNameAndIndex(ut.texture_name, ut.texture_min_id), DepthID.Message);
            }
            if (content == "")
            {
                new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(pos.X + dx, pos.Y + dy), DepthID.Message);
            }

        }
        public override void draw(Drawing d, int x, int y)
        {
            Vector posNew = new Vector(pos.X + x, pos.Y + y);
            if (str == "")
            {
                d.Draw(posNew, DataBase.getTex(ut.texture_name), DataBase.getRectFromTextureNameAndIndex(ut.texture_name, ut.texture_min_id), DepthID.Message);
            }
            if (content == "")
            {
                new RichText(content, FontID.Medium, Color.White).Draw(d, new Vector(posNew.X + dx, posNew.Y + dy), DepthID.Message);
            }
        }
    }
}
