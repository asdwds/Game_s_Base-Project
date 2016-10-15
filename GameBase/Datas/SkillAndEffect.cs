using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class SkillData {
        public string typename { get; protected set; }
        public int coolDown { get; protected set; }
        public SkillData(string _typename, ) {
            typename = _typename;
        }
    }
    class Skill
    {
        public SkillData skilldata;
        public string name { get; set; }
        public int coolDown;
        public Skill(SkillData _skilldata) {
            skilldata = _skilldata;
            name = skilldata.typename;
        }
        
        public void refresh() { coolDown =0; }
        public void use() {
            coolDown = skilldata.coolDown;
        }
    }
}
