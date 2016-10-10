﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    /// <summary>
    /// Scrollをwindowのcoloumsにいれて、update()していけば上手く作動するようにしたい。
    /// またこれはwindowみたいにcoloumを持ち、scroll barもある
    /// </summary>
    class Scroll : Coloum
    {
        // pos は始点のx,y。常にスクロール全体の左上の座標に当たる
        //w, hは scrollの全長となる,大きい方が自動にスクロールする方向となる。スクロールbarの長さは今表示できる項目数/総項目数になる

        /// <summary>
        /// 今のスクロールbarが全体のどこに位置するかを決める。0から1を前から後ろと扱い、最初の見える項目の番号と関連する
        /// </summary>
        public double percent = 0;

        /// <summary>
        /// 今のスクロールbarの位置.xを取得できる/スクロールbarの位置.xを設定できる
        /// </summary>
        public double nx { get { if (vertical) return pos.X + dx; else return pos.X + dx + w * percent; }
            protected set
            {
                if (vertical) { //pos.X = value+dx;
                } else
                {
                    if (value >= pos.X + dx + barlength) { percent = barlength / w; }
                    else { percent = (value - dx - pos.X) / w; }
                }
            }
        }
        /// <summary>
        /// 今のスクロールbarの位置.yを取得できる/スクロールbarの位置.yを設定できる
        /// </summary>
        public double ny { get { if (vertical) return pos.Y + dy; else return pos.Y + dy + (h - barlength) * percent; }
            protected set {
                if (!vertical) {   //pos.Y = value;
                } else
                {
                    if (value >= pos.Y + dy + barlength) { percent = barlength / h; }
                    else { percent = (value - dy - pos.Y) / h; }
                }
            }
        }
        /// <summary>
        /// scroll bar の長さ
        /// </summary>
        public int barlength {
            get {
                if (vertical) {
                    if (coloums.Count > visiable_n) return visiable_n * h / (coloums.Count + 1);
                    else { return 0; }
                } else
                {
                    if (coloums.Count > visiable_n) return visiable_n * w / (coloums.Count + 1);
                    else { return 0; }
                }
            }
        }
        /// <summary>
        /// スクロールの全長を取得する
        /// </summary>
        public int l { get { if (vertical) return h; else return w; } }
        /// <summary>
        /// 今スクロールの中で選ばれている要項の番号.PosInsideで定義される
        /// </summary>
        public int now_coloum_index = -1;//-1の時はスクロールbarを操っている。
        const int scrollbar_index = -1;
        /// <summary>
        /// 画面上に見えるこのスクロールの要項の数
        /// </summary>
        public int visiable_n;
        /// <summary>
        /// このスクロールにある全要項
        /// </summary>
        public List<Coloum> coloums;
        /// <summary>
        /// 黙認の最小スクロールbarの大きさ。
        /// </summary>
        const int default_size = 16;
        const int min_h = 16;
        /// <summary>
        /// タイトルとscrollの距離である. strはタイトルになる
        /// </summary>
        const int default_title_dx = 5, default_title_dy = 10;
        protected bool vertical
        {
            get
            {
                if (h >= w) { return true; }
                else { return false; }
            }
        }

        /*public Scroll(int _ox, int _oy, string _str, int _n,int _h, int _w = default_size,int _dx=default_title_dx,int _dy=default_title_dy) : base(_ox, _oy, _str, Command.Scroll)
        {
            h = _h; w = _w;visiable_n = _n;
        }*/
        /// <summary>
        /// 要項の個別サイズと、画面に見える要項の数でscrollを作る
        /// </summary>
        /// <param name="ox">スクロールの始点x</param>
        /// <param name="oy">スクロールの始点x</param>
        /// <param name="w">横スクロールの場合に長さになる、以外はスクロールバーの幅とみなせる</param>
        /// <param name="_str">タイトル</param>
        /// <param name="_sh">要項間の間隔</param>
        /// <param name="_n">全長に対応する要項の数</param>
        public Scroll(int _ox, int _oy, string _str, int _sh, int _n, int _w = default_size,bool _vertical=true,int _dx=default_title_dx,int _dy=default_title_dy)
          : base(_ox, _oy, _str, Command.Scroll)
        //  : this(_ox, _oy, _str,_n, _sh * _n, _w,_dx,_dy)
        {
            visiable_n = _n;
            if (_vertical) { h = _sh * visiable_n; w = _w; }
            else { w = _sh * visiable_n; h = _w; }
            dx = _dx; dy = _dy;
        }

        public virtual void addColoum(Coloum c) {
            coloums.Add(c);
            if (default_size * coloums.Count > l)
            {
                if (vertical) { h = default_size * coloums.Count; }
                else { w = default_size * coloums.Count; }
            }
        }

        public override bool PosInside(Vector v, int ax, int ay) {
            if (v.X < nx + ax + w && v.X >= nx + ax - 1 && v.Y < ny + ay + h && v.Y >= ny + ay - 1)
            {
                now_coloum_index = scrollbar_index;
                return true;
            }
            else {
                for (int i = (int)(percent * coloums.Count); i < (int)(percent * coloums.Count) + visiable_n; i++)
                {
                    if (coloums[i].PosInside(v, (int)(ax + nx), (int)(ay + ny)))
                    {
                        select_index(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public override bool PosInside(Vector v) { return PosInside(v, 0, 0); }
        protected void select_index(int i)
        {
            now_coloum_index = i;
            if (now_coloum_index != scrollbar_index)
            {
                content = coloums[i].content;
            }
        }
        public override Command update_with_key_manager(KeyManager k)
        {
            if (k.IsKeyDown(KeyID.Select))
            {
                coloums[now_coloum_index].is_selected();
                return is_applied();
            }
            return Command.nothing;
        }
        public override Command update_with_mouse_manager(MouseManager m) {
            if (m != null)
            {
                if (now_coloum_index == scrollbar_index && m.IsButtomDown(MouseButton.Left) ) {
                    // scroll bar の移動
                    if (vertical) { ny += m.MousePosition().Y - m.OldMousePosition().Y; }
                    else { nx += m.MousePosition().X - m.OldMousePosition().X; }
                    reply = Command.Scroll;

                }
                else if(m.IsButtomDownOnce(MouseButton.Left)) // Mouseがscroll全体の中に入っているかは、windowで判断している。
                {
                    // とある要項を選択した。
                    content = coloums[now_coloum_index].content;
                    reply = coloums[now_coloum_index].reply;
                } 
            }
            return reply;
        }

        public override void draw(Drawing d,int ax,int ay)
        {
            base.draw(d, ax, ay);
            for(int i = (int)(coloums.Count*percent); i < (int)(coloums.Count * percent)+visiable_n; i++)
            {
                coloums[i].draw(d,(int)(ax-nx),(int)(ay-ny));
            }
        }
        public override void draw(Drawing d) { draw(d, 0, 0); }
    }
}
