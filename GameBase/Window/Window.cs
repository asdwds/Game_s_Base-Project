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
}// namespace CommonPart End
