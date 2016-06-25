using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    /// <summary>
    /// プレイ画面上のバーなどのクラス
    /// </summary>
    class BoxWindow {
        // 描画する位置（左上）の座標
        public Vector _pos;
        // 横と縦のマス目(16x16)の数
        int _width, _height;

        public BoxWindow(Vector pos, int w, int h) {
            _pos = pos;
            _width = w;
            _height = h;
        }

        public void Draw(Drawing d) {
            // ボックスの背景を表示している
            // 左上と右上
            d.Draw(_pos,DataBase.box_flame[0],DepthID.Status);
            d.Draw(_pos + new Vector((_width - 1) * 16d, 0d), DataBase.box_flame[2], DepthID.Status);
            // 上下の中央
            for (int i = 1; i < _width - 1;i++) {
                d.Draw(_pos + new Vector(i * 16d, 0d), DataBase.box_flame[1], DepthID.Status);
                d.Draw(_pos + new Vector(i * 16d, (_height - 1) * 16d), DataBase.box_flame[7], DepthID.Status);
            }
            // 左下と右下
            d.Draw(_pos + new Vector(0d, (_height - 1) * 16d), DataBase.box_flame[6], DepthID.Status);
            d.Draw(_pos + new Vector((_width - 1) * 16d, (_height - 1) * 16d), DataBase.box_flame[8], DepthID.Status);
            // 左右の中央
            for (int i = 1; i < _height - 1; i++) {
                d.Draw(_pos + new Vector(0, i * 16), DataBase.box_flame[3], DepthID.Status);
                d.Draw(_pos + new Vector((_width - 1) * 16d, i * 16d), DataBase.box_flame[5], DepthID.Status);
            }
            // 真ん中
            for (int i = 1; i < _width - 1; i++)
                for (int j = 1; j < _height - 1; j++)
                    d.Draw(_pos + new Vector(i * 16d, j * 16d), DataBase.box_flame[4], DepthID.Status);

        }
    }
}
