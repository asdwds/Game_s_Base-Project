using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;

namespace CommonPart {

    public enum Command { left_and_go_back = -101, nothing = -100, apply_int = 110, apply_string = 111,
        button_on = 112, button_off = 113, previousPage = 114, nextPage = 115, Scroll=116,tru_fals=117,
        selectInScroll=118,closeThis=119,reloadScroll=120,
        openUTD=200, UTDutButtonPressed=201, /*204 missed*/
        openAniD =202, addTex=203, /* 205 missed,*/ playAnimation=206, newAniD = 207, applyAniD=208,// open animation DataBase, add Texture,play animation,
        specialIntChange1=301,specialIntChange2=302,
        CreateNewMapFile =1001,LoadMapFile=1002,
    };

    /// <summary>
    /// Commandとそれと対応した整数、文字列データを格納するための物
    /// </summary>
    class CommandData
    {
        public readonly Command c;
        public readonly int[] ints;
        public readonly string[] strings;
        public CommandData(Command _c,int[] its=null,string[] strs=null)
        {
            c = _c;
            if (its != null) { ints = new int[its.Length]; for (int i = 0; i < its.Length; i++) ints[i] = its[i]; }
            if (strs != null) { strings = new string[strs.Length]; for (int j = 0; j < strs.Length; j++) strings[j] = strs[j]; }
        }
        public CommandData(Command _c, string[] strs) : this(_c, null, strs) { }
    }

    /// <summary>
    /// 不変なデータをまとめたクラス
    /// </summary>
    class DataBase : IDisposable {
        #region about Editor

        /// <summary>
        /// このDataBaseなどに使われている読み込み、Editorでのファイルの読み方法が何時の物かの判断に使われる。確実に大きく変化したら更新していくように。
        /// 日付になっている。年月日で, 9月は 09
        /// </summary>
        public static readonly int ThisSystemVersionNumber = 161010;
        public enum VersionNumber { SixTeenTenTen=161010,};
        /// <summary>
        /// used between UnitType, etc.-- ut1;ut2
        /// </summary>
        public static char interval_of_each_type = ';';
        /// <summary>
        /// used between int and string , etc.--int ...ij,string kl...
        /// </summary>
        public static char interval_of_each_datatype = ',';
        /// <summary>
        /// used before every array , etc.--int ...ij&int k-z&intA-F;string GH&string I-P...
        /// </summary>
        public static char interval_of_array = '&';
        #endregion

        #region UTD
        public static string utFileName = "uts.dat";
        public static FileStream ut_file;
        public static readonly UnitTypeDataBase utDataBase;
        #endregion
        #region Textures
        public static readonly string texDFileName = "texNames.dat";

        /// <summary>
        /// 必ずTexturesDataDictionaryに読み込まれる画像.
        /// </summary>
        public static readonly string defaultBlankTextureName = "None";

        /// <summary>
        /// string is its path, maybe from "Content".  and also string key contains a size of texture's single unit
        /// </summary>
        //keyを使って読み込みできるので、class化していない。そのままバイナリ-ファイルからkeyを読み取り、Content.Loadをする。
        public static Dictionary<string, Texture2Ddata> TexturesDataDictionary = new Dictionary<string, Texture2Ddata>();

        /// <summary>
        /// TexturesDataDictionaryにTexture2Ddataを追加するメッソド。
        /// </summary>
        private static void tda(string name)
        {

            /*Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
            Console.WriteLine(Directory.GetCurrentDirectory());
            Directory.SetCurrentDirectory(Content.RootDirectory);
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(File.Exists(name));*/
            try {
                Texture2D t=Content.Load<Texture2D>(name);
                if (!TexturesDataDictionary.ContainsKey(name))
                {
                    TexturesDataDictionary.Add(name, new Texture2Ddata(t, name));
                }else
                {
                    Console.WriteLine("tda:Exist in Dictionary: " + name);
                }
            }
            catch{ Console.WriteLine("tda: load error" + name); return; }

        }
        /// <summary>
        /// TexturesDataDictionaryにTexture2Ddataを追加するメッソド。
        /// </summary>
        public static void tdaA(string name)
        {
            try
            {
                Texture2D t = Content.Load<Texture2D>(name);
                if (!TexturesDataDictionary.ContainsKey(name))
                {
                    TexturesDataDictionary.Add(name, new Texture2Ddata(t, name));
                    Console.WriteLine(name + " added.");
                }
                else
                {
                    Console.WriteLine("tdaA:Exist in Dictionary: " + name);
                }
            }
            catch { Console.WriteLine("tdaA: load error" + name); return; }
            /*
             * Directory.SetCurrentDirectory(DirectoryWhenGameStart);
            if (Directory.Exists("Content"))
            {
                Directory.SetCurrentDirectory("Content");
                if (File.Exists(name))
                {
                    Console.WriteLine("Found in " + Directory.GetCurrentDirectory());
                    TexturesDataDictionary.Add(name, new Texture2Ddata(Content.Load<Texture2D>(name), name));
                }
                else
                {
                    Console.WriteLine("Tex " + name + "is Null. Maybe Not Found directly in the Content?");
                }
            }else {
                Console.WriteLine("tdaA: CurrentDirectory -" + Directory.GetCurrentDirectory());
                TexturesDataDictionary.Add(name, new Texture2Ddata(Content.Load<Texture2D>(name), name));
            }//Contentを見つけていない場合
            */
        }

        #endregion
        #region Animation
        public static AnimationDataAdvanced defaultBlankAnimationData;
        public const string defaultAnimationNameAddOn = "-stand";
        static string aniDFileName = "animationNames.dat";
        public static Dictionary<string, AnimationDataAdvanced> AnimationAdDataDictionary = new Dictionary<string, AnimationDataAdvanced>();

        /// <summary>
        /// TexturesDataDictionaryが構成できてからこれをcall/使用してください。
        /// </summary>
        private static void setup_Animation() {
            defaultBlankAnimationData = new AnimationDataAdvanced("defaultblank", 1000, 1, defaultBlankTextureName);
            FileStream aniD_file = File.Open(aniDFileName, FileMode.OpenOrCreate);
            aniD_file.Position = 0;
            BinaryReader aniD_br = new BinaryReader(aniD_file);
            while (aniD_br.BaseStream.Position < aniD_br.BaseStream.Length)
            {
                try
                {
                    bool repeat = aniD_br.ReadBoolean();
                    int min_index = aniD_br.ReadInt32();
                    int max_index = aniD_br.ReadInt32();
                    int length = aniD_br.ReadInt32();
                    int[] frames = new int[length];
                    for (int i = 0; i < length; i++) { frames[i] = aniD_br.ReadInt32(); }
                    //ints end, strings start
                    string animeName = aniD_br.ReadString();
                    string textureName = aniD_br.ReadString();
                    string preName = aniD_br.ReadString();
                    string nexN = aniD_br.ReadString();
                    AnimationAdDataDictionary.Add(animeName, new AnimationDataAdvanced(animeName, frames, textureName, repeat));
                }
                catch (EndOfStreamException e) { break; }
            }
            aniD_br.Close(); aniD_file.Close();

        }
        private void save_Animation()
        {
            FileStream aniD_file = File.Open(aniDFileName, FileMode.Create);
            aniD_file.Position = 0;

            BinaryWriter aniD_bw = new BinaryWriter(aniD_file);
            foreach (AnimationDataAdvanced ad in AnimationAdDataDictionary.Values)
            {
                aniD_bw.Write(ad.repeat);
                foreach (int d in ad.getIntsData()) { aniD_bw.Write(d); }
                foreach (string str in ad.getStringsData()) { aniD_bw.Write(str); }
            }
            aniD_bw.Close(); aniD_file.Close();
        }
        #endregion

        private static ContentManager Content;
        public static string DirectoryWhenGameStart;
        /// <summary>
        /// ゲーム初期化後にゲームが見ているDirectoryからDatasのフォルダを開くか作って開く
        /// </summary>
        public static void goToFolderDatas()
        {
            Directory.SetCurrentDirectory(DirectoryWhenGameStart);
            if (!Directory.Exists("Datas")) { Directory.CreateDirectory("Datas"); }
            Directory.SetCurrentDirectory("Datas");
        }
        #region about Coloum
        public const string BlankDefaultContent = "ClickAndType";
        public const string ButtonDefaultContent = "Click";
        public const int InvaildColoumContentReply_int = -99999;
        public const string InvaildColoumContentReply_string = "fobagnufabo";
        #endregion

        #region GameScreen
        public const int WindowDefaultSizeX = 1280;
        public const int WindowSlimSizeX = 720;
        public const int WindowDefaultSizeY = 720;
        public const int WindowSlimSizeY = 480;

        #endregion

        #region Unload And Save
        public void Dispose()
        {
            Console.WriteLine("DisposeDataBase");
            Directory.SetCurrentDirectory(DirectoryWhenGameStart);
            if (!Directory.Exists("Datas")) { Directory.CreateDirectory("Datas"); }
            Directory.SetCurrentDirectory("Datas");
            Console.WriteLine(Directory.GetCurrentDirectory());
            ut_file.Close();
            #region texture
            FileStream texD_file = File.Open(texDFileName, FileMode.Create,FileAccess.Write);
            texD_file.Position = 0;

            BinaryWriter texD_bw = new BinaryWriter(texD_file);
            foreach (string s in TexturesDataDictionary.Keys)
            {
                texD_bw.Write(s);
            }
            texD_bw.Close(); texD_file.Close();
            #endregion
            #region anime
            save_Animation();
            #endregion
            AnimationAdDataDictionary.Clear();
            TexturesDataDictionary.Clear();

            Content = null;
        }
        #endregion

        #region singleton and setup
        public static DataBase database_singleton = new DataBase();
        //public DataBase get() { return database_singleton; }
        static DataBase()
        {
            DirectoryWhenGameStart = Directory.GetCurrentDirectory();

            if (Directory.GetCurrentDirectory() == "Datas") { }
            else if (!Directory.Exists("Datas")) { Directory.CreateDirectory("Datas"); }
            Directory.SetCurrentDirectory("Datas");
            Console.WriteLine(Directory.GetCurrentDirectory());
            ut_file = File.Open(utFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader ut_br = new BinaryReader(ut_file);
            utDataBase = new UnitTypeDataBase(ut_br);
            ut_br.Close();
        }
        private DataBase() { }
        #endregion

        /// <summary>
        /// Game1からのCotentを使って、DataBaseの内容を埋める.Tex/Animationはここで読み込む
        /// </summary>
        /// <param name="content"></param>
        public static void Load_Contents(ContentManager c)
        {
            Content = c;
            #region textures
            FileStream texD_file = File.Open(texDFileName, FileMode.OpenOrCreate, FileAccess.Read);
            texD_file.Position = 0;
            BinaryReader texD_br = new BinaryReader(texD_file);
            while (texD_br.BaseStream.Position < texD_br.BaseStream.Length)
            {
                try
                {
                    string n = texD_br.ReadString();
                    if (n != defaultBlankTextureName)
                    {
                        tda(n);
                    }
                }
                catch (EndOfStreamException e) { break; }
            }
            texD_br.Close(); texD_file.Close();
            tda(defaultBlankTextureName);
            #endregion
            #region animation
            setup_Animation();
            #endregion
        }

        #region Method
        public static void addAniD(AnimationDataAdvanced ad) {
            if (AnimationAdDataDictionary.ContainsKey(ad.animationDataName))
            {
                Console.WriteLine("addAniD: " + ad.animationDataName + " exists.");
            }
            else {
                AnimationAdDataDictionary.Add(ad.animationDataName,ad);
            }
        }
        public static void RemoveAniD(string name,string addOn) {
            if (addOn == null)
            {
                AnimationAdDataDictionary.Remove(name);
            }else {
                AnimationAdDataDictionary.Remove(name + addOn);
            }
        }
        public static bool existsAniD(string name, string addOn)
        {
            if (addOn == null) return AnimationAdDataDictionary.ContainsKey(name);
            else return AnimationAdDataDictionary.ContainsKey(name + addOn);
        }
        public static AnimationDataAdvanced getAniD(string name, string addOn = null)
        {
            if (addOn == null && AnimationAdDataDictionary.ContainsKey(name))
            {
                return AnimationAdDataDictionary[name];

            }
            else if (AnimationAdDataDictionary.ContainsKey(name + addOn))
            {
                return AnimationAdDataDictionary[name + addOn];
            }

            if (AnimationAdDataDictionary.ContainsKey(name + defaultAnimationNameAddOn))
            {
                return AnimationAdDataDictionary[name + defaultAnimationNameAddOn];
            }
            else
            {
                return defaultBlankAnimationData;
            }
        }
        public static Texture2D getTex(string name)
        {
            if (TexturesDataDictionary.ContainsKey(name))
            {
                return TexturesDataDictionary[name].texture;
            }
            else
            {
                return TexturesDataDictionary[defaultBlankTextureName].texture;
            }
        }
        public static Texture2Ddata getTexD(string name) {
            if (name == null || name == "" || !TexturesDataDictionary.ContainsKey(name)) { return TexturesDataDictionary[defaultBlankTextureName]; }
            else { return TexturesDataDictionary[name]; }
        }
        public static Rectangle getRectFromTextureNameAndIndex(string name, int id)
        {
            int w = TexturesDataDictionary[name].w_single;
            int h = TexturesDataDictionary[name].h_single;
            int x = id % TexturesDataDictionary[name].x_max  *w;
            int y = id / TexturesDataDictionary[name].x_max * h;
            if (id >= TexturesDataDictionary[name].x_max * TexturesDataDictionary[name].y_max) { x = y = 0; }
            return new Rectangle(x, y, w, h);
        }
        /*
        public static Rectangle getRectFromTextureNameAndIndex(string name, int id) {
            int w = 0, h = 0;
            int r = 0;
            while (r < name.Length)
            {
                if (!char.IsNumber(name[r])) { r++; }
                else { break; }
            }//最初の数字のところまで行く。
            while (r < name.Length)
            {
                if (char.IsNumber(name[r])) { w = w * 10 + (int)name[r] - (int)'0'; r++; }
                else { r++; break; }
            }//widthを読む
            while (r < name.Length)
            {
                if (char.IsNumber(name[r])) { h = h * 10 + (int)name[r] - (int)'0'; r++; }
                else { break; }
            }//heightを読む
            if (w == 0) { w = getTex(name).Width; }
            if(h==0) { h = getTex(name).Height; }
            int max_forX = TexturesDataDictionary[name].getTex().Width / w;// how many single textures on a line
            int max_forY = TexturesDataDictionary[name].getTex().Height / h;// how many rows of textures
            int x = id % max_forX * w;
            int y = id / max_forX * h;
            if (id >= max_forX * max_forY) { x = y = 0; }
            return new Rectangle(x, y, w, h);
        }*/
        //上のメソッドの別バージョン、多分使わない。
        public static UnitType getUnitType(string typename)
        {
            if (typename == null)
            {
                if (utDataBase.UnitTypeList.Count > 0) return utDataBase.UnitTypeList[0];
                else return utDataBase.CreateBlankUt();
            }
            else { return utDataBase.getUnitTypeWithName(typename); }
        }
        public static int getUTDcount() { return utDataBase.UnitTypeList.Count; }
        #endregion
    }// DataBase end

    class Texture2Ddata
    {
        #region variables
        /// <summary>
        /// Textureのファイル名を使って得た、画像の1コマの width, height
        /// </summary>
        public int w_single { get; private set; } = 0;
        public int h_single { get; private set; } = 0;
        public int x_max { get; private set; }
        public int y_max { get; private set; }
        public Texture2D texture { get; private set; }
        public string texName { get; private set; }
        #endregion
        #region constructor
        public Texture2Ddata(Texture2D tex, string name)
        {
            int r = 0;//nameのstringとしての位置　変数。
            texture = tex;
            texName = name;
            while (r < name.Length)
            {
                if (!char.IsNumber(name[r])) { r++; }
                else { break; }
            }//最初の数字のところまで行く。
            while (r < name.Length)
            {
                if (char.IsNumber(name[r])) { w_single = w_single * 10 + (int)name[r] - (int)'0'; r++; }
                else { r++; break; }
            }//widthを読む
            while (r < name.Length)
            {
                if (char.IsNumber(name[r])) { h_single = h_single * 10 + (int)name[r] - (int)'0'; r++; }
                else { break; }
            }//heightを読む
            if (w_single == 0) { w_single = texture.Width; }
            if (h_single == 0) { h_single = texture.Height; }
            x_max = texture.Width / w_single;
            y_max = texture.Height / h_single;
        }
        #endregion
    }
}// namespace end
