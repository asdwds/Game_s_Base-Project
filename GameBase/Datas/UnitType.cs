using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommonPart;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart
{
    class UnitTypeDataBase
    {
        static int versionOfEditor;

        public List<UnitType> UnitTypeList = new List<UnitType>();
        public Dictionary<string, UnitType> UnitTypeDictionary = new Dictionary<string, UnitType>();
        public UnitTypeDataBase(BinaryReader br)
        {
            setup_from_BinaryReader(br);
        }

        public void setup_from_BinaryReader(BinaryReader br)
        {
            if (br == null) { return; }
            int n = 0;
            versionOfEditor = br.Read();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                
                try
                {
                    bool next_is_str = false;
                    List<int> intdatas = new List<int>();
                    List<string> stringdatas = new List<string>();
                    while (br.BaseStream.Position < br.BaseStream.Length) // load in every int and string data for one unit type 
                    {
                        if (!next_is_str && br.PeekChar() == DataBase.interval_of_each_datatype)//次はstringのデータ
                        //intのデータの一部を読み込み,それがちょうどintervalと一致するすることもあり得る
                        //これはまだ解決されていません。
                        {
                            next_is_str = true;
                            br.ReadChar();
                        }
                        else if (next_is_str && br.PeekChar() == DataBase.interval_of_each_type)//次のUTに移る
                            //stringdataがなくてもintervalは必ずあります。
                        {

                            next_is_str = false;
                            UnitTypeList.Add(new UnitType(intdatas, stringdatas, n));
                            n++;
                            br.ReadChar();
                            break;
                        }
                        else
                        {
                            if (next_is_str)//stringを読み込む
                            {
                               
                                stringdatas.Add(br.ReadString());
                            }
                            else
                            {
                                intdatas.Add(br.ReadInt32());
                            }
                        }
                    }// while end. one UnitType has been created and added to the List
                }
                catch (EndOfStreamException e)
                {
                    break;
                }
            }// Finished reading file. List should be alright.
            foreach (UnitType ut in UnitTypeList)
            {
                UnitTypeDictionary.Add(ut.typename, ut);
            }//Dictionaryをつくる
        }// end of setup

        public void save_into_BinaryWriter(BinaryWriter bw)
        {
            bw.Write(DataBase.ThisSystemVersionNumber);
            foreach (UnitType ut in UnitTypeList)
            {
                foreach (int i in ut.getIntData())
                {
                    bw.Write(i);
                }
                bw.Write(DataBase.interval_of_each_datatype);
                foreach (string str in ut.getStringData())
                {
                    bw.Write(str);
                }
            }
            bw.Write(DataBase.interval_of_each_type);
        }

        #region method
        public UnitType CreateBlankUt()
        {
            return new UnitType("test " + UnitTypeList.Count.ToString(), DataBase.defaultBlankTextureName, "test", 1, 1, 0,0);
        }
        public void Add(UnitType ut)
        {
            UnitTypeList.Add(ut);
            UnitTypeDictionary.Add(ut.typename, ut);
        }
        public void Remove(UnitType ut)
        {
            UnitTypeDictionary.Remove(ut.typename);
            UnitTypeList.Remove(ut);
        }
        public void RemoveAt(int id)
        {
            UnitTypeDictionary.Remove(UnitTypeList[id].typename);
            UnitTypeList.RemoveAt(id);
        }
        public UnitType getUnitTypeWithName(string name)
        {
            if (UnitTypeDictionary.ContainsKey(name))
            {
                return UnitTypeDictionary[name];
            }else
            {
                Console.WriteLine("UTD error: " + name + " does no exist.");
                return CreateBlankUt();
            }
        }
        #endregion

    }// class UnitTypeDictionary end

    class UnitType
    {
        #region public
        /// <summary>
        /// このUTのジャンルとなる。0-アニメーションしない、1-アニメションする、2-skillを持つ、次は4,8,16..と2の乗数
        /// </summary>
        public int genre=0; 
        //異なるジャンルのコンストラクターを通過する度に、そのジャンルに応じた値を加算するといいでしょう。
        /// <summary>
        /// AnimationUnitTypeかどうか
        /// </summary>
        public bool animated { get { return genre % 2 == 1; } }

        public int index_in_List { get; protected set; }
        public int maxhp { get; protected set; }
        public int maxatk { get; protected set; }
        //public int //something// { get; protected set; }
        public string typename { get; protected set; }
        /// <summary>
        /// animation UnitTypeでは、animationのtexture_nameと同じ値になります
        /// </summary>
        public string texture_name { get; protected set; }        //UnitTypeDictionaryにペアとなるstringである。他と重複しないように設定する必要がある。
                                                                  //これとは別にUnitは独自のstring変数 name を持っています。
        public string label { get; protected set; } //ラベルは複数のUnitTypeが共通点を表すためにつかいます。stringとしてその部分文字列も使われるので、注意してほしい.
        public int texture_max_id { get; protected set; }
        public int texture_min_id { get; protected set; }
        /// <summary>
        /// animationにアクセスするためのkeyの一部
        /// </summary>
        public string animation_name;
        #endregion

        #region protected
        protected int passableType;
        #endregion

        #region constructor
        /// <summary>
        /// これはAnimation UnitType専用のコンストラクタ です.　animatedがtrueになります,animation_nameが代入されます
        /// </summary>
        protected UnitType(string _typename, string _texture_name, string _label, int _maxhp, int _maxatk) {
            animation_name = _texture_name;
            texture_name = DataBase.getAniD(animation_name).texture_name;
            typename = _typename; label = _label;
            maxhp = _maxhp;maxatk = _maxatk;
        }
        public UnitType(string typename, string _texture_name,string label, int maxhp, int maxatk, int texture_max_id, int texture_min_id)
        {
            this.typename = typename;
            this.label = label;
            this.maxhp = maxhp;
            this.maxatk = maxatk;
            this.texture_max_id = texture_max_id;
            this.texture_min_id = texture_min_id;
        }
        public UnitType(List<int> intdatas, List<string> stringdatas,int id) // id is the index of the UnitTypeList
        {
            index_in_List = id;

            int n = 0;
            genre = intdatas[n];
            if (animated) { Console.WriteLine("id:"+id+" animated as a UnitType!"); }
            n++;
            texture_max_id = intdatas[n];
            n++;
            texture_min_id = intdatas[n];
            n++;
            maxhp = intdatas[n];
            n++;
            maxatk = intdatas[n];
            n++;
            passableType = intdatas[n];
            n++;

            n = 0;
            texture_name= stringdatas[n];
            n++;
            typename = stringdatas[n];
            n++;
            label = stringdatas[n];
            n++;
        }
        #endregion

        #region get property in int[] + string[]
        //今のint[],string[]は、必要なデータかつ個数が決まっているのでこの様に書いている。
        //後に、例えば不特定多数のskillを覚えられるとするとskill_ids[]を返すものを作るといいでしょう。
        public virtual int[] getIntData()
        {
            return new int[] {
                genre,
                texture_max_id, 
                texture_min_id, 
                maxhp,          
                maxatk,         
                passableType,   

                //any other int variables should be added here
            };
        }
        public virtual string[] getStringData()
        {
            return new string[] {
                texture_name,
                typename,
                label,

                //any other int variables should be added here
            };
        }
        #endregion
        public virtual void drawIcon(Drawing d, Vector pos)
        {
            d.Draw(pos, DataBase.getTex(texture_name), DataBase.getRectFromTextureNameAndIndex(texture_name, texture_min_id), DepthID.Message);
        }
        #region Method
        public virtual bool passable()
        {
            return true;//要変更
        }
        #endregion
    }

    class AnimatedUnitType:UnitType
    {
        //texture_nameはanimationへのアクセスkeyです

        #region public
        public AnimationAdvanced animation;
        #endregion

        /// <summary>
        /// ほぼUnitTypeにあるAnimationUnitType専用のコンストラクタによって構成される
        /// </summary>
        /// <param name="_texture_name">animationにアクセスするための一部のkeyです</param>
        public AnimatedUnitType(string _typename, string _texture_name, string _label, int _maxhp, int _maxatk)
            :base(_typename,_texture_name,_label,_maxhp,_maxatk)
        {
            if (genre % 2 == 0) { genre += 1; }
        }

        public override void drawIcon(Drawing d, Vector pos)
        {
            if (animation != null)
            {
                animation.Draw(d, pos, DepthID.Message);
            }
        }
        public void playAnimation(string addOn) {
            animation =　new AnimationAdvanced(DataBase.getAniD(animation_name,addOn));
        }
    }
}
