using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart
{/*
    class ForBarrage_Partial_Source_Code
    {
        #region Do not Copy
        //---コピー不要---------------
        //---コピー不要---------------
        public static int leftside = 280;
        public static int rightside = 1000;


        //---コピー不要---------------
        //---コピー不要---------------

        #endregion


        static protected bool bossEngaged = false;

        // enemysなどの配列もstaticにしているかのチェック?、staticになっているし、mapを渡すのもやめるとか?
        /// animation nameとしても扱える. 設定するとbossLifeBarAnimeも作られる
        protected string bossLifeBarTextureName {
            set {
                bossLifeBarAnime = null;
                bossLifeBarAnime = new AnimationAdvanced(DataBase.getAniD(value));
            }
        }
        protected AnimationAdvanced bossLifeBarAnime=null;
        protected Vector bossLifeBarAnimationLeftTopPos = new Vector(102,24);
        protected Vector bossLifeGaugeLeftTopDisplacement=new Vector(102,24);
        protected Vector bossLifeGaugeLeftTopPos { get { return bossLifeBarAnimationLeftTopPos + bossLifeGaugeLeftTopDisplacement; } }
        protected Vector bossLifeGaugeSize=new Vector(1077,37);
        protected Color bossLifeGaugeColor = Color.Crimson;

        /// <summary>
        /// ゲーム中のmapでどのような動作を行う/行っているか　の定義を示す。
        /// </summary>
        public const string updated="-updated",fullStopped = "-fulStop", playerStopped = "-plaStop", enemysStopped = "-eneStop",
            bossStopped = "-bosStop", bothSideMove = "-boSidMove";
        
        //実はEnumにはEnum.Parseがあり、intもしくはstringをenumに変えれるかを試してくれる。
        //成功したらenumの値になる。

        static protected string mapState = ""; //初期は空として、その都度変えていく 
        static protected int leftsideTo = 280; // bothSideMoveの時、左辺がどこに行くのかを決める
        static protected int rightsideTo = 1000; // bothSideMoveの時、右辺がどこに行くのかを決める
        static protected int sideMoveSpeed = 4; // 両サイドが動くときの速度
        /// <summary>
        /// 両サイドが0,1280まで広がり、最後のboss戦画面になる。
        /// </summary>
        static public void EngagingTrueBoss()
        {
            bossEngaged = true;
            leftsideTo = 0; rightsideTo = 1280;
            mapState += bothSideMove;
        }
        
        

        public void Draw(Drawing d)
        {

            if (bossLifeBarAnime != null) {
                d.DrawBox(bossLifeGaugeLeftTopPos, bossLifeGaugeSize, bossLifeGaugeColor, DepthID.Status);
                bossLifeBarAnime.Draw(d, bossLifeBarAnimationLeftTopPos, DepthID.Status);
            }


        }



        //------------------------
        // player class
        public string animationName;
        protected AnimationAdvanced animationAd;

        /// <summary>
        /// animationを使用するなら、texture_nameは animationNameと同じにする。
        /// </summary>
        public void draw(Drawing d) // ###############################   new    ############
        {
            animationAd.Draw(d, new Vector(x - animationAd.X / 2, y - animationAd.Y / 2), DepthID.Player);
            //d.Draw(new Vector(x - DataBase.getTex(texture_name).Width / 2,y - DataBase.getTex(texture_name).Height / 2) , DataBase.getTex(texture_name),DepthID.Player);
        }
    }//class end
*/
}// namespace end
