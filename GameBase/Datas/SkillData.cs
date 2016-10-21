using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    /// <summary>
    /// 大きいスキルの区分
    /// </summary>
    public enum SkillGenreL { none = 0, generation = 1 }
    /// <summary>
    /// 細かいスキルの区分
    /// </summary>
    public enum SkillGenreS { none = 0, shot, laser, circle }

    abstract class SkillData
    {
        public int cooldownFps;
        public string skillName;
        public SkillGenreL sgl;
        public SkillGenreS sgs;

        public SkillData(string _skillName, SkillGenreL _sgl, SkillGenreS _sgs, int _cooldownFps)
        {
            skillName = _skillName;
            sgl = _sgl;
            sgs = _sgs;
            cooldownFps = _cooldownFps;
        }
    }//class SkillData end
    class Skill
    {
        public SkillData skilldata;
        public string name { get; set; }
        public int coolDown;
        public Skill(SkillData _skilldata) {
            skilldata = _skilldata;
            name = skilldata.skillName;
        }
        
        public void refresh() { coolDown =0; }
        public void use() {
            coolDown = skilldata.cooldownFps;
        }
    }
}
