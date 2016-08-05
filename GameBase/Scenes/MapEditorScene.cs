using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart {
    /// <summary>
    /// マップ作成のシーンのクラス
    /// </summary>
    class MapEditorScene : Scene {
        static private List<UnitType>;
        static private List<TileType>;
        static private List<Window>;
        public MapEditorScene(SceneManager s)　: base(s)
        {

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
