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
        AnimationDataAdvanced ad { get { return DataBase.getAniD(aAdData_name); } }
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
        /// <summary>
        /// このclassのcommand処理はこのwindow内部の処理を行い、後はSceneに任せたいためbase.を呼ぶ
        /// </summary>
        /// <param name="c"></param>
        protected override void deal_with_command(Command c)
        {
            switch (c)
            {
                case Command.tru_fals:

                    break;
                case Command.playAnimation:
                    ((AnimationColoum)coloums[0]).play();
                    break;
            }
            base.deal_with_command(c);
        }
        public void setup_window()
        {
            //repeat,min,max,length,frames,name,texname,pre,next
            int nx = 10, ny = 200; int dy = 24;
            AddColoum(new AnimationColoum(nx, ny, aAdData_name, aAdData_name, Command.replayAnimation));

            nx = 10; ny = 10;
            AddColoum(new Button(nx, ny, "repeat:", ad.repeat.ToString(), Command.tru_fals, false));
            ny += dy;
            AddColoum(new Blank(nx, ny, "min_tex:", ad.min_texture_index.ToString(), Command.apply_int));
            ny += dy;
            AddColoum(new Blank(nx, ny, "max_tex:", ad.max_texture_index.ToString(), Command.apply_int));
            ny += dy;
            AddColoum(new Coloum(nx, ny, "frames:",  Command.nothing));
            ny += dy;
            for(int i = 0; i < ad.frames.Length;i++) {
                AddColoum(new Blank(nx, ny, i.ToString()+":", ad.frames[i].ToString(), Command.apply_int));
                ny += dy;
            }
            nx = 100; ny = 10;
            AddColoum(new Blank(nx, ny, "name:",ad.animationDataName, Command.apply_string));
            ny += dy;
            AddColoum(new Blank(nx, ny, "texname:", ad.texture_name, Command.apply_string));
            ny += dy;
            AddColoum(new Blank(nx, ny, "pre_ani_name:", ad.pre_animation_name, Command.apply_string));
            ny += dy;
            AddColoum(new Blank(nx, ny, "next_ani_name:", ad.next_animation_name, Command.apply_string));
            ny += dy;

        }

    }
}
