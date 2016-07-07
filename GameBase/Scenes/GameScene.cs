using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart {
    /// <summary>
    /// ゲーム開始後の処理を書いたクラス
    /// </summary>
    class GameScene : Scene {
        #region Variable
        // ボックスウィンドウ（ユニットボックスとか）のリスト
        List<WindowBox> bars;
        StringBox testbox;

        #endregion
        #region Method
        public GameScene(SceneManager s)
            : base(s) {
            bars = new List<WindowBox>();
            for(int i = 0; i < DataBase.BarIndexNum; i++) {
                bars.Add(new WindowBox(DataBase.BarPos[i],DataBase.BarWidth[i],DataBase.BarHeight[i]));
            }
            testbox = new StringBox(new Vector(300,300), 40, 5);
            testbox.AddString("この文字列はテスト表示です。", new Vector(310,310));
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
                        if (Settings.WindowStyle == 1 && bars[i].windowPosition.Y != DataBase.BarPos[i].Y)
                            bars[i].windowPosition = DataBase.BarPos[i];
                        else if(Settings.WindowStyle == 0 && bars[i].windowPosition.Y == DataBase.BarPos[i].Y)
                            bars[i].windowPosition = new Vector(DataBase.BarPos[i].X, DataBase.BarPos[i].Y - 240);
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
            testbox.Draw(d);
        }
        public override void SceneUpdate() {
            base.SceneUpdate();

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select)) Delete = true;
        }
        #endregion
    }// class end
}// namespace end
