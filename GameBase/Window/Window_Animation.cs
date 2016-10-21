using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    /// <summary>
    /// AnimationAdvancedを自分の中に描画できるwindow.
    /// </summary>
    class Window_Animation : Window_WithColoum
    {
        string aAdData_name;
        /// <summary>
        /// アニメーションデータの名前から作れたAnimatioAdvanced,を含むcoloumsのwindow
        /// </summary>
        /// <param name="ad_name">アニメーションデータのフルネーム</param>
        /// <param name="_x">このwindowの画面上のx</param>
        /// <param name="_y">windowの画面上のy</param>
        /// <param name="_w">windowの幅,アニメーションに応じて自動調整しません</param>
        /// <param name="_h">windowの高さ</param>
        public Window_Animation(string ad_name,int _x, int _y, int _w, int _h) : base(_x, _y, _w, _h)
        {
            aAdData_name = ad_name;
            setup_window();
        }

        public void setup_window()
        {
            AddColoum(new AnimationColoum(10,10,aAdData_name,aAdData_name,Command.nothing) );
        }
        public override void update(KeyManager k, MouseManager m)
        {
            base.update(k, m);
        }
        public override void draw(Drawing d)
        {
            ((AnimationColoum)coloums[0]).draw(d, x, y);
        }
    }
}
