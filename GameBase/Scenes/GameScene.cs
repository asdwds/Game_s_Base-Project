using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    /// <summary>
    /// ゲーム開始後の処理を書いたクラス
    /// </summary>
    class GameScene : Scene {
        // ボックスウィンドウ（ユニットボックスとか）のリスト
        List<BoxWindow> bars;

        public GameScene(SceneManager s)
            : base(s) {
            bars = new List<BoxWindow>();
            for(int i = 0; i < DataBase.BarIndexNum; i++) {
                bars.Add(new BoxWindow(DataBase.BarPos[i],DataBase.BarWidth[i],DataBase.BarHeight[i]));
            }
        }

        public override void SceneDraw(Drawing d) {
            for (int i = 0; i < DataBase.BarIndexNum; i++) {
                switch ((DataBase.BarIndex)i) {
                    case DataBase.BarIndex.Study:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Unit:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Minimap:
                        if (Settings.WindowStyle == 1)
                            bars[i]._pos = DataBase.BarPos[i];
                        else
                            bars[i]._pos = new Vector(DataBase.BarPos[i].X, DataBase.BarPos[i].Y - 240);
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Status:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Arrange:
                        bars[i].Draw(d);
                        break;
                }
            }
        }
        public override void SceneUpdate() {
            base.SceneUpdate();

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select)) Delete = true;
        }
    }
}
