using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;

namespace CommonPart
{
    class UnitType
    {
        #region public
        public string name;
        public int maxhp { get; private set; }
        public int maxatk { get; private set; }
        //public int //something// { get; private set; }
        public int texture_max_id { get; private set; }
        public int texture_min_id { get; private set; }
        #endregion

        #region private
        private string typename;
        private string label;
        private int passableType;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        
        public UnitType(string name,string　typename,string label,int maxhp,int maxatk,int texture_max_id,int texture_min_id)
        {
            this.name = name;
            this.typename = typename;
            this.label = label;
            this.maxhp = maxhp;
            this.maxatk = maxatk;
            this.texture_max_id = texture_max_id;
            this.texture_min_id = texture_min_id;
        }

        /// <summary>
        /// 関数
        /// </summary>

        public bool passable()
        {
            return true;//要変更
        }

        public string getTypename()
        {
            return typename;
        }

        public string getlabel()
        {
            return label;
        }
    }
}
