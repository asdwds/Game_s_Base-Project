using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    class TileType　{
        #region 変数
        public string name;
        private string type_name;
        private string label;
        public int texture_max_id { get; private set; }
        public int texture_min_id { get; private set; }
        public int passable_type { get; private set; }
        #endregion
        #region 関数
        TileType(string Type_name, string Label, int Texture_max_id, int Texture_min_id, int Passable_type) {
            type_name = Type_name;
            label = Label;
            texture_max_id = Texture_max_id;
            texture_min_id = Texture_min_id;
            passable_type = Passable_type;
        }
        public bool passable() {
            return true;
        }
        public string getLabel() { return label; }
        #endregion
    }
}
