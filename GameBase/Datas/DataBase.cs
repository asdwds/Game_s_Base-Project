using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart {
    /// <summary>
    /// 不変なデータをまとめたクラス
    /// </summary>
    class DataBase　{

        #region Variable
        public static readonly int WindowDefaultSizeX = Game1.WindowSizeX;
        public static readonly int WindowDefaultSizeY = Game1.WindowSizeY;
        public static readonly int WindowSlimSizeY = 720;
        public static readonly int BarIndexNum = 5;
        public static readonly int[] BarWidth = new[] { 22, 22, 22, 40, 18 };
        public static readonly int[] BarHeight = new[] { 6, 23, 16, 4, 6 };
        public static readonly int HexWidth = 180;
        public static readonly int HexHeight = 200;
        public static List<Texture2D> hex;
        public static List<Texture2D> box_flame;
        public enum BarIndex
        {
            Study, Unit, Minimap, Status, Arrange
        }
        public static readonly Vector[] BarPos = new[] {
            new Vector(0d, 0d), new Vector(0d, 96d), new Vector(0d, 704d), new Vector(352d, 0d), new Vector(992d, 0d)
        };
        public static readonly int MAP_MAX = 10;
        
        #endregion
        #region singleton
        static DataBase database_singleton = new DataBase();

        static DataBase() {

        }
        private DataBase() { }
        #endregion

        #region Method
        #endregion
    }// DataBase end
}// namespace end
