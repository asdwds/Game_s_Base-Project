#define SAVEDATA_NOCHECK
using System.IO;
using System;
using System.Collections.Generic;

namespace CommonPart
{
   
    class MapDataSave
    {
        #region variables concerns FileStream
        private FileStream MapFileStream;
        protected string fileName;
        public string mapName;
        /// <summary>
        /// starts from 0 !, maximum (x,y) on the map
        /// </summary>
        public int max_x, max_y;
        private BinaryReader br;
        private BinaryWriter bw;
        #endregion

        #region variable concerns Map in the screen 
        /// <summary>
        /// マップの1マスの長さはスクリーンのXrate、マップの1マスの高さはスクリーンのYrateだけに相当する。
        /// ect.マップの(0,0)座標はスクリーンの(Xrate長,Yrate高)の長方形にあたる。
        /// </summary>
        static public int Xrate = 4, Yrate = 4;

        /// <summary> 
        /// Sceneから更新されるべきのスクリーン上に見えるmapの左上と右下の map上の座標
        /// </summary>
        static public int ltx=0, lty=0, rbx=0, rby=0;

        /// <summary>
        /// unitのListである。
        /// </summary>
        static public List<Unit> utList = new List<Unit>();
        #endregion

        #region constructor
        /// <summary>
        /// This Does Not make a File! use CreateFile() to save the MapData
        /// </summary>
        /// <param name="_fileName">マップがファイルとしての名前</param>
        /// <param name="_mapName">マップの名前</param>
        /// <param name="mx">マップの最大x座標</param>
        /// <param name="my">マップの最大y座標</param>
        public MapDataSave(string _fileName,string _mapName,int mx,int my)
        {
            fileName = _fileName;
            mapName = _mapName;
            max_x = mx; max_y = my;
            initialize();
        }
        /// <summary>
        /// !! only you already have the File whose name is _fileName.
        /// </summary>
        /// <param name="_fileName"></param>
        public MapDataSave(string _fileName)
        {
            getToDirectoryDatas();
            if (File.Exists(_fileName))
            {
                MapFileStream = File.Open(_fileName, FileMode.OpenOrCreate);
                MapFileStream.Position = 0;
                br = new BinaryReader(MapFileStream);
                int version = br.ReadInt32();
                switch (version)
                {
                    case 160910:
                        mapName = br.ReadString();
                        max_x = br.ReadInt32();
                        max_y = br.ReadInt32();
                        while (br.BaseStream.Position < br.BaseStream.Length)
                        {
                            bool it_is_int_now = true;
                            List<int> ints = new List<int>();
                            List<string> strings = new List<string>();
                            while (br.BaseStream.Position < br.BaseStream.Length)
                            {
                                if(br.PeekChar() == DataBase.interval_of_each_type)//unitとunitのデータの狭間 
                                {
                                    br.ReadChar();
                                    break;
                                }else if(it_is_int_now==true && br.PeekChar()==DataBase.interval_of_each_datatype)//intとstringの狭間
                                {
                                    it_is_int_now = false;// it is string now!
                                    br.ReadChar();
                                }else if (it_is_int_now)
                                {
                                    ints.Add(br.ReadInt32());
                                }else// it_is_sstring_now
                                {
                                    strings.Add(br.ReadString());
                                }
                            }// while reading one unit end
                            utList.Add(new Unit(ints, strings));
                        }//while reading all unit end

                        // BinaryReader has Read All
                        br.Close();
                        break;
                    default:
                        Console.Write("Unknown Datas here");
                        break;
                }
            }

        }
        #endregion

        /// <summary>
        /// 画面上に現れるMapの初期化、ファイルはまだ作っていない
        /// </summary>
        public void initialize()
        {

        }

        static public void update_map_xy(Vector lt,Vector rb)
        {
            ltx = (int)(lt.X); 

        }
        public void changeTo(string _fileName,string _mapName,int mx,int my) {
            mapName = _mapName;
            fileName = _fileName;
            max_x = mx;
            max_y = my;

        }
        public bool PosInsideMap(Vector pos)
        {
            if(pos.X>=0 && pos.X<=max_x && pos.Y>=0 && pos.Y <= max_y)
            {
                return true;
            }else
            {
                return false;
            }
        }
        /// <summary>
        /// go to "Datas"
        /// </summary>
        public void getToDirectoryDatas()
        {
            if (Directory.GetCurrentDirectory() != DataBase.DirectoryWhenGameStart)
            {
                Directory.SetCurrentDirectory(DataBase.DirectoryWhenGameStart);
            }
            if (!File.Exists("Datas"))
            {
                File.Create("Datas");
            }
            Directory.SetCurrentDirectory("Datas");
        }
        /// <summary>
        /// Mapの情報を書き込むファイルを作る
        /// </summary>
        public void createFile()
        {
            getToDirectoryDatas();
            MapFileStream = File.Open(fileName, FileMode.OpenOrCreate);
        }

        public void WriteAll() {
            MapFileStream.Position = 0;
            bw = new BinaryWriter(MapFileStream);
            bw.Write(DataBase.ThisSystemVersionNumber);
            bw.Write(mapName);
            bw.Write(max_x);bw.Write(max_y);

            #region Write Units
            if (utList.Count > 0) {
                for(int i = 0; i < utList.Count; i++)
                {
                    if (PosInsideMap(new Vector(utList[i].x_index, utList[i].y_index)))
                    {
                        List<int> ints = utList[i].getListIntData();
                        string[] strings = utList[i].getStringData();
                        for (int j = 0; j < ints.Count; j++)
                        {
                            bw.Write(ints[j]);
                        }
                        bw.Write(DataBase.interval_of_each_datatype);//like intintint,stringstringstring
                        for (int k = 0; k < strings.Length; k++)
                        {
                            bw.Write(strings[k]);
                        }
                        bw.Write(DataBase.interval_of_each_type);// like Unit[i] , Unit[i+1],
                    }
                }
            }
            #endregion

        }
        public void close() // contains Dispose()
        {
            MapFileStream.Close();
            mapName = null;
            fileName = null;
        }


    }

}// namespace end