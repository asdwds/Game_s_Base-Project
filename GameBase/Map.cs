using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart {
    /// <summary>
    /// ステージごとのマップのオブジェクト
    /// </summary>
    class Map {
        #region Variable
        List<List<int>> mapData;
        #endregion
        #region Method
        public Map() {
            List<int> tmp = new List<int>(DataBase.MAP_MAX);
            mapData = new List<List<int>>();
            for (int i = 0; i < DataBase.MAP_MAX; i++)
                tmp.Add(0);
            for (int i = 0; i < DataBase.MAP_MAX; i++)
                mapData.Add(tmp);
        }
        public void Draw(Drawing d, Vector camera) {
            for (int i = 0; i < DataBase.MAP_MAX; i++) {
                double drawx = DataBase.HexWidth * i - camera.X;
                for (int j = 0; j < DataBase.MAP_MAX; j++) {
                    double drawy = DataBase.HexHeight * 3 / 4 * j - camera.Y;
                    if (drawy < Game1._WindowSizeY && drawy >= -DataBase.HexHeight
                        && drawx < Game1._WindowSizeX && drawx >= -DataBase.HexWidth * 3 / 2) {
                        if (j % 2 == 1) {
                            d.Draw(new Vector(drawx + DataBase.HexWidth / 2, drawy), DataBase.hex[mapData[i][j]], DepthID.BackGroundFloor);
                        }
                        else {
                            d.Draw(new Vector(drawx, drawy), DataBase.hex[mapData[i][j]], DepthID.BackGroundFloor);
                        }
                    }
                    else if(drawy > Game1._WindowSizeY)
                    {
                        break;
                    }
                }
                if (drawx - DataBase.HexWidth * 3 / 2 > Game1._WindowSizeX)
                {
                    break;
                }
            }
        }
        #endregion
    }// class end
}// namespace end
