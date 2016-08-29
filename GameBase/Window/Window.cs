using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class Window
    {
        #region public Variable
        public int x, y;
        public int w, h;
        #endregion

        #region private Variable
        private List<RichText> richTexts=new List<RichText>();
        /// <summary>
        /// i+1番目のrichtextは　i番目のRichTextの左下の点+richTextsRelativePos[i+1]　の位置にある。
        /// </summary>
        private List<Vector> richTextsRelativePos = new List<Vector>();
        private List<string> texturePaths = new List<string>();
        private List<Vector> texturesPos = new List<Vector>();
        private int NumberOfCharasEachLine= 20;
        #endregion

        #region constructor
        public Window(int _x,int _y,int _w,int _h) {
            x = _x;
            y = _y;
            w = _w;
            h = _h;
        }
        #endregion

        public void draw(Drawing d)
        {
            if (richTexts.Count() > 0)
            {
                richTexts[0].Draw(d, new Vector(x + richTextsRelativePos[0].X, y + richTextsRelativePos[0].Y), DepthID.Message);
                double ix = x + richTextsRelativePos[0].X;
                for (int i = 1; i < richTexts.Count(); i++)
                {
                    ix += richTextsRelativePos[i].X;
                    richTexts[i].Draw(d, new Vector( ix, richTexts[i-1].Y+richTextsRelativePos[i].Y), DepthID.Message);
                }
            }
            if (texturePaths.Count() > 0)
            {
                for (int i = 1; i < richTexts.Count(); i++)
                {
                    d.Draw(new Vector(texturesPos[0].X, texturesPos[0].Y), DataBase.TexturesDictionary[texturePaths[0]] , DepthID.Message);
                }
            }
        }
        #region Method
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
        #endregion
    } // class Window end


    class Window_UnitType : Window
    {
        public UnitType ut;
        public List<int> utIntList=new List<int>();
        public List<string> utStringList = new List<string>();
        public List<Coloum> coloums = new List<Coloum>();

        public Window_UnitType(UnitType _ut, int _x, int _y, int _w, int _h) : base(_x, _y, _w, _h)
        {
            ut = _ut;
            setup_unitType_window();
        }

        /// <summary>
        /// これはUnitTypeの設定に応じて書き換えが必要あるでしょう。
        /// </summary>
        public void setup_unitType_window()
        {
            int dy = 12;
            int ny = y;
            clear_old_data_and_put_in_now_data();

            /*texture_max_id, //0th
                texture_min_id, 
                maxhp,          
                maxatk,         
                passableType,   
                */
            int n = 0;
            coloums.Add(create_blank(Command.apply_int,x,ny,"texture_max_id",utIntList[n].ToString() ));
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
        public Blank create_blank(Command c,int x,int ny,string str,string content) {
            return new Blank(x, ny, str, content, (int)c);
        }
        public void clear_old_data_and_put_in_now_data() {
            utIntList.Clear();
            utStringList.Clear();
            utIntList.AddRange(ut.getIntData());
            utStringList.AddRange(ut.getStringData());
        }

    }
}// namespace CommonPart End
