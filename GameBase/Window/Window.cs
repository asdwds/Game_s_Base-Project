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
        private List<Vector> richTextsPositions = new List<Vector>();
        private List<int> textureIDs = new List<int>();
        private List<Vector> texturesPositions = new List<Vector>();
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

        #region Method
        public void AddRichText(string text, Vector _vector) {
            AddRichText(text, _vector, NumberOfCharasEachLine);
        }
        public void AddRichText(string text, Vector _vector, int m) { 
        // m is "max number of chars in a line"
            richTexts.Add(new RichText(new PoorString(text, m).str));
            richTextsPositions.Add(_vector);
        }

        public void AddTexture(int texture_id, Vector _vector)
        {
            AddTexture(texture_id, _vector, NumberOfCharasEachLine);
        }
        public void AddTexture(int texture_id, Vector _vector, int m)
        {
            // m is "max number of chars in a line"
            textureIDs.Add(texture_id);
            texturesPositions.Add(_vector);
        }
        #endregion
    } // class Window end
}// namespace CommonPart End
