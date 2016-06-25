using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;

namespace CommonPart
{
    class Unit
    {
        #region public
        public int x_index;
        public int y_index;
        public int w;
        public int h;
        public int real_w;
        public int real_h;
        public double zoom_rate;
        public int hp;
        public UnitType unit_type;
        public int[] skills;
        public int[] effects;
        #endregion

        #region private
        private int frame_now;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(int x_index, int y_index, UnitType unit_type,int hp)
        {
            this.x_index = x_index;
            this.y_index = y_index;
            this.unit_type = unit_type;
            this.hp = hp;
        }
        public Unit(int x_index, int y_index, UnitType unit_type) :this(x_index, y_index,unit_type, unit_type.maxhp)
        {        }

        #region method
        public  void add_skill()
        {

        }
        
        public void chage_unit_type(UnitType unit_type2)
        {
            unit_type = unit_type2;
        }

        public void moveto_now(int x_index2,int y_index2)
        {
            x_index = x_index2;
            y_index = y_index2;

        }

        public UnitType getUnitype()
        {
            return unit_type;
        }
        #endregion

    }// Unit end
}// namespace CommonPart End
