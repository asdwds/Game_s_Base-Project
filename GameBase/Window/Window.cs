using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart
{
    class Window
    {
        #region public Variable
        public int x, y;
        public int w, h;
        public bool mouse_dragable = false;
        /// <summary>
        /// このwindowを持つSceneなどに伝えたいCommandが書いている。
        /// </summary>
        public Command commandForTop;
        #endregion

        #region private Variable
        private List<RichText> richTexts = new List<RichText>();
        /// <summary>
        /// i+1番目のrichtextは　i番目のRichTextの左下の点+richTextsRelativePos[i+1]　の位置にある。
        /// </summary>
        private List<Vector> richTextsRelativePos = new List<Vector>();
        private List<string> texturePaths = new List<string>();
        /// <summary>
        /// スクリーン上ではなく、windowの左上の(x,y)座標を(0,0)とした座標である。
        /// </summary>
        private List<Vector> texturesPos = new List<Vector>();
        private int NumberOfCharasEachLine = 20;
        #endregion

        #region constructor
        public Window(int _x, int _y, int _w, int _h) {
            x = _x;
            y = _y;
            w = _w;
            h = _h;
        }
        #endregion
        #region update
        public virtual void update(KeyManager k = null, MouseManager m = null)
        {
            if (k != null)
            {
                update_with_key_manager(k);
            }
            if (m != null)
            {
                update_with_mouse_manager(m);
            }
        }
        public virtual void update_with_key_manager(KeyManager k)
        { }
        public virtual void update_with_mouse_manager(MouseManager m)
        {
            if (mouse_dragable == true && PosInside(m.MousePosition()))
            {
                x += (int)(m.MousePosition().X - m.OldMousePosition().X);
                y += (int)(m.MousePosition().Y - m.OldMousePosition().Y);
            }
        }
        #endregion update
        public virtual void draw(Drawing d)
        {
            d.DrawBox(new Vector(x, y), new Vector(w, h), Color.Black, DepthID.Message);
            if (richTexts.Count() > 0)
            {
                richTexts[0].Draw(d, new Vector(x + richTextsRelativePos[0].X, y + richTextsRelativePos[0].Y), DepthID.Message);
                double ix = x + richTextsRelativePos[0].X;
                for (int i = 1; i < richTexts.Count(); i++)
                {
                    ix += richTextsRelativePos[i].X;
                    richTexts[i].Draw(d, new Vector(ix, richTexts[i - 1].Y + richTextsRelativePos[i].Y), DepthID.Message);
                }
            }
            if (texturePaths.Count() > 0)
            {
                for (int i = 0; i < texturePaths.Count(); i++)
                {
                    d.Draw(new Vector(x+texturesPos[0].X, y+texturesPos[0].Y), DataBase.getTex(texturePaths[i]), DepthID.Message);
                }
            }
        }
        #region Method
        public bool PosInside(Vector pos)
        {
            if(pos.X<x+w && pos.X > x && pos.Y<y+h && pos.Y>y) { return true; }
            return false;
        }
        public void AddRichText(string text, Vector _vector) {
            AddRichText(text, _vector, NumberOfCharasEachLine);
        }
        public void AddRichText(string text, Vector _vector, int m) {
            // m is "max number of chars in a line"
            richTexts.Add(new RichText(new PoorString(text, m).str));
            richTextsRelativePos.Add(_vector);
        }

        public void AddTexture(string texture_path, Vector _vector)
        {
            AddTexture(texture_path, _vector, NumberOfCharasEachLine);
        }
        public void AddTexture(string texture_path, Vector _vector, int m)
        {
            // m is "max number of chars in a line"
            texturePaths.Add(texture_path);
            texturesPos.Add(_vector);
        }
        
        public virtual void AddColoum(Coloum c) { }
        public virtual int getColoumiContent_int(int i) { return DataBase.InvaildColoumContentReply_int; }
        public virtual string getColoumiContent_string(int i) { return DataBase.InvaildColoumContentReply_string; }
        public virtual string getNowColoumContent_string() { return DataBase.InvaildColoumContentReply_string; }
        public virtual int getNowColoumContent_int() { return DataBase.InvaildColoumContentReply_int; }
        #endregion
    } // class Window end

    class Window_WithColoum : Window
    {
        public int now_coloums_index = 0;
        /// <summary>
        /// Window uses key/mouseManager. Not its coloums.
        /// </summary>
        public bool keyResponseToWindow = true, mouseResponseToWindow = true;

        protected List<Coloum> coloums = new List<Coloum>();
        public Window_WithColoum(int _x, int _y, int _w, int _h) : base(_x, _y, _w, _h)
        { }

        public override void AddColoum(Coloum c) { coloums.Add(c); }
        public override int getColoumiContent_int(int i)
        {
            if (coloums.Count > i){
                int ans;
                if (int.TryParse(coloums[i].content, out ans))
                {
                    return ans;
                }
            }
            return DataBase.InvaildColoumContentReply_int;
        }
        public override string getColoumiContent_string(int i)
        {
            if (coloums.Count > i)
            {
                return coloums[i].content;
            }
            return DataBase.InvaildColoumContentReply_string;
        }
        public override string getNowColoumContent_string() { return getColoumiContent_string(now_coloums_index);}
        public override int getNowColoumContent_int() { return getColoumiContent_int(now_coloums_index); }

        protected Blank create_blank(Command c, int x, int ny, string str, string content)
        {
            return new Blank(x, ny, str, content, c);
        }
        protected Button create_button(Command c, int x, int ny, string str, string content,bool useTexture)
        {
            return new Button(x, ny, str, content, c, useTexture );
        }
        protected virtual void deal_with_command(Command c)
        {
            if (c != Command.nothing)
            {
                left_coloum();
                commandForTop = c;
            }
            
        }
        public override void draw(Drawing d)
        {
            base.draw(d);
            foreach (Coloum c in coloums) { c.draw(d,x,y); }
        }
        #region update
        public override void update(KeyManager k, MouseManager m)
        {
            base.update(k, m);
            if (!keyResponseToWindow && !mouseResponseToWindow)
            {
                deal_with_command(coloums[now_coloums_index].update(k, m));
            }
        }
        public override void update_with_key_manager(KeyManager k) {
            if (keyResponseToWindow)
            {
                if (coloums.Count > 0)
                {
                    if (k.IsKeyDown(KeyID.Down))
                    {
                        now_coloums_index++;
                        if (now_coloums_index >= coloums.Count) { now_coloums_index = 0; }
                    }
                    else if (k.IsKeyDown(KeyID.Up))
                    {
                        now_coloums_index--;
                        if (now_coloums_index < 0) { now_coloums_index = coloums.Count - 1; }
                    }
                    if (k.IsKeyDown(KeyID.Select))
                    {
                        selected();
                    }
                }// if has any coloum or not
            }
        }//update_with_key_manager end
        public override void update_with_mouse_manager(MouseManager m)
        {
            if (mouseResponseToWindow)
            {
                if (coloums.Count > 0)
                {
                    if (m.IsButtomDown(MouseButton.Left))
                    {
                        for (int ii = 0; ii < coloums.Count; ii++)
                        {
                            if (coloums[ii].PosInside(m.MousePosition()))
                            {
                                now_coloums_index = ii;
                                selected();
                                return;
                            }
                        }
                    }// if has any coloum or not
                }
                base.update_with_mouse_manager(m);
            }
        }//update_with_mouse_manager end
        #endregion
        protected virtual void selected()
        {
            keyResponseToWindow = false;
            mouseResponseToWindow = false;
            coloums[now_coloums_index].is_selected();
        }
        private void left_coloum() { keyResponseToWindow = mouseResponseToWindow = true; }
    }//class Window_WithColoum end

    abstract class WindowAsPages
    {
        public int x, y;
        public int maximumWindowIndex = 0;
        public int nowWindowIndex = 0;
        protected List<Window> WindowPages = new List<Window>();
        protected Blank nowWindowIndexBlank;
        protected Button toPrevious, toNext;
        /// <summary>
        /// Pageの番号指定の欄とボタン、　とWindow実体の距離。
        /// </summary>
        protected int dx = 10, dy = 10;
        public WindowAsPages(int _x, int _y)
        {
            x = _x; y = _y;
            toPrevious = new Button(x, y, "", "pre", Command.previousPage,false, 0, 0);
            nowWindowIndexBlank = new Blank(x + dx, y, "now page:", "0", Command.apply_int);
            toNext = new Button(x + nowWindowIndexBlank.w + 2 * dx, y, "", "next", Command.nextPage, false,0, 0);
        }
        public virtual void update(KeyManager k, MouseManager m)
        {
            Command c = Command.nothing;
            if (WindowPages.Count > 0) {
                if (WindowPages.Count > 1)
                {
                    if (m != null)
                    {
                        if (m.IsButtomDown(MouseButton.Left))
                        {
                            if (toPrevious.PosInside(m.MousePosition())) { c = toPrevious.is_applied(); }
                            if (toNext.PosInside(m.MousePosition())) { c = toNext.is_applied(); }
                            if (nowWindowIndexBlank.PosInside(m.MousePosition())) { c = nowWindowIndexBlank.is_applied(); }
                            deal_with_command(c);
                        }
                    }
                }
                WindowPages[nowWindowIndex].update(k, m);
            }
        }
        protected virtual void deal_with_command(Command c)
        {
            if (c == Command.nothing) { return; }
            if (c == Command.previousPage) {
                c = Command.apply_int;
                nowWindowIndexBlank.change_content( (nowWindowIndex-1).ToString()  );
            }
            if (c == Command.nextPage)
            {
                c = Command.apply_int;
                nowWindowIndexBlank.change_content((nowWindowIndex + 1).ToString());
            }
            if (c == Command.apply_int)
            {
                nowWindowIndex = int.Parse(nowWindowIndexBlank.content) ;
            }
        }
        public virtual void draw(Drawing d)
        {
            if (WindowPages.Count > 0) { WindowPages[nowWindowIndex].draw(d); }
        }

    }

    /// <summary>
    /// DataBaes.utDataBase is required; a poorly made
    /// </summary>
    class Window_utsList : Window_WithColoum
    {
        protected const int white_space_size = 40;
        /// <summary>
        /// UTDutButtonはUnitTypeDataBaseとUnitTypeが存在することが前提で使えるもの
        /// </summary>
        protected List<UTDutButton> UTDutButtons = new List<UTDutButton>();
        #region constructor
        public Window_utsList(int _x, int _y, int _w, int _h) : base(_x, _y, _w, _h)
        {
            setUp_UTDutButtons();
        }
        #endregion

        private void setUp_UTDutButtons()
        {
            if (UTDutButtons.Count > 0) { foreach (UTDutButton ub in UTDutButtons) { ub.clear(); } }
            UTDutButtons.Clear();
            //now UTDutButtons is an empty list
            int nx = white_space_size, ny = white_space_size;
            int max_dy = 0;
            for (int i = 0; i < DataBase.getUTDcount(); i++)
            {
                AddUTDut(nx, ny, DataBase.utDataBase.UnitTypeList[i].typename);
                max_dy = Math.Max(max_dy, UTDutButtons[i].h);
                nx += UTDutButtons[i].w;
                if (nx + white_space_size + UTDutButtons[i].w > this.w)
                {
                    nx = white_space_size;
                    ny += max_dy;
                }
            }
        }
        public void reloadUTDutButtons() {
            if (DataBase.getUTDcount() == UTDutButtons.Count) {
                int i = 0;
                foreach (UnitType ut in DataBase.utDataBase.UnitTypeList) {
                    UTDutButtons[i].changeStrTo(ut.typename);
                }
            }
        }
        public void AddUTDut(int x, int y, string str, string content = "", Command c = Command.UTDutButtonPressed)
        {
            UTDutButtons.Add(createUTDutButton(x, y, str, content, c));
        }
        public UTDutButton createUTDutButton(int x, int y, string str,string content="",Command c=Command.UTDutButtonPressed) {
            return new UTDutButton(x,y,str,content,c);
        }
    }// class Window_utsList end

    class Window_UnitType : Window_WithColoum
    {
        public UnitType ut;
        public List<int> utIntList = new List<int>();
        public List<string> utStringList = new List<string>();
        
        public Window_UnitType(UnitType _ut, int _x, int _y, int _w, int _h) : base(_x, _y, _w, _h)
        {
            ut = _ut;
            setup_unitType_window();
        }

        /// <summary>
        /// これはUnitTypeの設定に応じて書き換えが必要あるでしょう。
        /// </summary>
        public void setup_unitType_window(UnitType _ut=null)
        {
            int dy = 12;
            int ny = y;
            clear_old_data_and_put_in_now_data();
            if (_ut != null) { ut = _ut; }
            /*texture_max_id, //0th
                texture_min_id, 
                maxhp,          
                maxatk,         
                passableType,   
                */
            int n = 0;
            coloums.Add(create_blank(Command.apply_int, x, ny, "texture_max_id", utIntList[n].ToString()));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "texture_min_id", utIntList[n].ToString()));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "maxhp", utIntList[n].ToString()));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "maxatk", utIntList[n].ToString()));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "passableType", utIntList[n].ToString()));
            n++; ny += dy;

            /* texture_name,
               typename,
               label,
            */
            n = 0;
            coloums.Add(create_blank(Command.apply_int, x, ny, "texture_name", utStringList[n]));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "typename", utStringList[n]));
            n++; ny += dy;
            coloums.Add(create_blank(Command.apply_int, x, ny, "label", utStringList[n]));
            n++; ny += dy;
        }

        public void clear_old_data_and_put_in_now_data() {
            utIntList.Clear();
            utStringList.Clear();
            utIntList.AddRange(ut.getIntData());
            utStringList.AddRange(ut.getStringData());
        }


    }//class Window_UnitType end

}// namespace CommonPart End
