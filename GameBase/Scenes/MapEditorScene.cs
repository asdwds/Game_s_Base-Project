using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart {
    /// <summary>
    /// マップ作成のシーンのクラス
    /// </summary>
    class MapEditorScene : Scene {
        static private List<List<Tile>> tiles = new List<List<Tile>>();
        static private List<Window> windows = new List<Window>();
        static public bool ready { get; private set; } = false;
        static public MapDataSave mapDataS { get; private set; } = null;
        /// <summary>
        /// draw X,Y line. 1-draw X,Y, 0-draw nothing,
        /// </summary>
        static private int drawLine = 1; 
        /// <summary>
        /// 現ゲーム画面のマップ描画区画の左下の位置 が 表すマップの座標。
        /// </summary>
        static public int lbx = 0, lby = 0;
        /// <summary>
        /// MapEditorScene中、ゲーム画面のこのｘ軸ｙ軸から mapの描画が始まります。
        /// </summary>
        static public int leftsideX = 240, topsideY = 0,rightsideX=1040,bottonsideY=720; // これはゲーム画面基準なので、左上が0となります。
        /// <summary>
        /// 現ゲーム画面のマップ描画区画の左上の位置 が 表すマップの座標y。
        /// </summary>
        static public int lty{
            get
            {
                if (mapDataS != null) return lby + (bottonsideY - topsideY) / mapDataS.Yrate;
                else return lby + (bottonsideY - topsideY);
            }
        }
        static public int ltx { get { return lbx; } }

        public MapEditorScene(SceneManager s) : base(s)
        {
            setup_windows();
        }
        /// <summary>
        /// once changed this, make sure of data -those saved in file and used to edit
        /// </summary>
        public void setup_windows() {
            int nx = 0;int ny = 0;
            int dx = 0; 
            //windows[0] starts
            windows.Add(new Window_WithColoum(20, 20, 120, 120));
            ((Window_WithColoum) windows[0] ).AddColoum(new Coloum(nx, ny, "version: "+DataBase.ThisSystemVersionNumber.ToString(), Command.nothing));
            ny += 20;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "MapFileName: ", DataBase.BlankDefaultContent, Command.apply_string));
            nx = 5; ny += 20; dx = 10;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "x: ",DataBase.BlankDefaultContent, Command.apply_int));
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx+dx, ny, "y: ", DataBase.BlankDefaultContent, Command.apply_int));
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx+dx*3, ny, "MapName: ", DataBase.BlankDefaultContent, Command.apply_string));
            nx = 80;  ny += 15;
            ((Window_WithColoum)windows[0]).AddColoum(new Button(nx, ny, "", "ApplyMap", Command.CreateNewMapFile,false));
            nx = 10;
            ((Window_WithColoum)windows[0]).AddColoum(new Button(nx, ny, "", "LoadMap", Command.LoadMapFile, false));
            nx = 5; ny += 15;dx = 20;
            windows[0].AddColoum(new Button(nx, ny, "", "open UTD", Command.openUTD, false));
            nx += dx;
            windows[0].AddColoum(new Button(nx, ny, "", "open AniD", Command.openAniD, false));
            nx = 5; ny += 20; dx = 20;
            
            nx += dx;
            windows[0].AddColoum(new Button(nx, ny, "", "add Texture", Command.addTex, false));
            // windows[0] is finished.

            // windows[1] starts
            windows.Add(new Window_utsList(20, ny + 20, 150, 150));
        }


        private void addTex()
        {
            Console.WriteLine("Type in the path from Content correctly.");
            string str=Console.ReadLine();
            if (DataBase.TexturesDataDictionary.ContainsKey(str))
            {
                Console.Write(" already inside the DataBase\n");
            }else
            {
                DataBase.tdaA(str);
            }
        }
        private void openUTD()
        {
            new UTDEditorScene(scenem);
        }
        /// <summary>
        /// make up a mapDataS or apply changes to the mapDataS
        /// </summary>
        private void initializeMap() {
            int i = 0;
            i++;
            string _mapFileName = windows[0].getColoumiContent_string(i);
            i++;
            int _map_maxX = windows[0].getColoumiContent_int(i);
            i++;
            int _map_maxY = windows[0].getColoumiContent_int(i);
            i++;
            string _mapName = windows[0].getColoumiContent_string(i);
            if (mapDataS == null)
            {
                mapDataS = new MapDataSave(_mapFileName,_mapName,_map_maxX,_map_maxY);
            }else if(mapDataS.fileName==_mapFileName){ mapDataS.changeTo(_mapFileName, _mapName, _map_maxX, _map_maxY); }
            else if (mapDataS.fileName!=_mapFileName) {
                Console.WriteLine("create New Map with Name: " + _mapFileName);
                Console.Write("Yes / No : ");
                string res = Console.ReadLine();
                if (res == "Yes") { mapDataS.changeTo(_mapFileName, _mapName, _map_maxX, _map_maxY); }
                else if(res=="No"){// res is "No"
                    // do nothing,
                }else { Console.Write("Invaild response as "+res+" .\nPlease Only Use \"Yes\" or \"No\" .(No Need space and \")"); }
            }
        }


        public override void SceneDraw(Drawing d) {

            if (mapDataS != null)
            {
                #region map line draw
                switch (drawLine)
                {
                    case 1:
                        float lineWidth = 1;
                        int dx = 5,dy = 5;
                        Vector xb = new Vector(leftsideX, bottonsideY);
                        Vector xt = new Vector(leftsideX, topsideY);
                        if(lbx%dx == 0) {
                            d.DrawLine(xb,xt, lineWidth, Color.White, DepthID.Map);
                        }else { xb.X += (lbx % dx)*mapDataS.Xrate; }

                        for (int i = 0; i < (rightsideX - leftsideX) / mapDataS.Xrate/dx; i++)
                        {
                            xb.X += dx*mapDataS.Xrate; xt.X = xb.X;
                            d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map);
                        }
                        xb.X = leftsideX; xt.X = rightsideX;
                        xb.Y = bottonsideY;xt.Y = xb.Y;
                        if (lby % dx == 0)
                        {
                            d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map);
                        }
                        else { xb.Y += (xb.Y % dy)*mapDataS.Yrate; }

                        for (int j = 0; j < (topsideY - bottonsideY) / mapDataS.Yrate / dy; j++)
                        {
                            xb.Y += dy*mapDataS.Yrate; xt.Y = xb.Y;
                            d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map);
                        }
                        break;
                    default:
                        break;
                }//switch what kind of draw end
                #endregion
                #region draw Unit
                foreach(Unit u in mapDataS.utList)
                {
                    u.draw(d);
                }
                #endregion

            }// if map is not null end

        }//SceneDraw

        public override void SceneUpdate() {
            base.SceneUpdate();
            for(int i = 0; i < windows.Count; i++)
            {
                if (windows[i].PosInside(mouse.MousePosition()) ) {
                    windows[i].update((KeyManager)Input, mouse);
                    switch (windows[i].commandForTop)
                    {
                        case Command.CreateNewMapFile:

                            break;
                        case Command.openUTD:

                            break;
                        case Command.addTex:
                            addTex();
                            break;
                        case Command.nothing:
                            break;
                        default:
                            Console.WriteLine("window" + i + " " + "strangeCommand");
                            break;

                    }
                }else
                {
                    windows[i].update();
                }
            }
        }//SceneUpdate() end
    }//class MapEditorScene end


}//namespace CommonPart End
