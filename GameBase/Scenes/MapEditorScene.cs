using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart {
    /// <summary>
    /// マップ作成のシーンのクラス
    /// </summary>
    class MapEditorScene : BasicEditorScene {
        static private List<List<Tile>> tiles = new List<List<Tile>>();
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
        static public int leftsideX = 240, topsideY = 0,rightsideX=1040,bottonsideY=Game1._WindowSizeY; // これはゲーム画面基準なので、左上が0となります。
        /// <summary>
        /// 現ゲーム画面のマップ描画区画の左上の位置 が 表すマップの座標y。
        /// </summary>
        static public int lty{
            get
            {
                if (mapDataS != null) return lby + (int)((bottonsideY - topsideY) / MapDataSave.Yrate);
                else return lby + (bottonsideY - topsideY);
            }
        }
        static public int ltx { get { return lbx; } }
        static public int rbx { get {
                if (mapDataS != null) return lbx + (int)((rightsideX - leftsideX) / MapDataSave.Xrate);
                else return lbx + (rightsideX - leftsideX);
            }
        }
        static public int rby { get { return lby; } }

        public MapEditorScene(SceneManager s) : base(s)
        {
            setup_windows();
        }

        public override void setup_windows() {
            int nx = 0;int ny = 0;
            int dx = 0; int dy = 25;
            //windows[0] starts
            windows.Add(new Window_WithColoum(0, 0, 240, 180));
            
            ((Window_WithColoum) windows[0] ).AddColoum(new Coloum(nx, ny, "version: "+DataBase.ThisSystemVersionNumber.ToString(), Command.nothing));
            ny += dy;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "MapFileName: ", DataBase.BlankDefaultContent, Command.apply_string));
            nx = 5; ny += dy; dx = 90;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "x: ",DataBase.BlankDefaultContent, Command.apply_int));
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx+dx*7/5, ny, "y: ", DataBase.BlankDefaultContent, Command.apply_int));
            ny += dy;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "Xrate: ", "1", Command.apply_int));
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx + dx *7/5, ny, "Yrate: ", "1", Command.apply_int));
            ny += dy;
            ((Window_WithColoum)windows[0]).AddColoum(new Blank(nx, ny, "MapName: ", DataBase.BlankDefaultContent, Command.apply_string));
            ny += dy;
            nx = 10;
            ((Window_WithColoum)windows[0]).AddColoum(new Button(nx, ny, "", "LoadMap", Command.LoadMapFile, false));
            nx += dx;
            ((Window_WithColoum)windows[0]).AddColoum(new Button(nx, ny, "", "ApplyMap", Command.CreateNewMapFile,false));
            nx = 5; ny += dy;
            windows[0].AddColoum(new Button(nx, ny, "", "open UTD", Command.openUTD, false));
            nx += dx+10;
            windows[0].AddColoum(new Button(nx, ny, "", "open AniD", Command.openAniD, false));
            nx = 5; ny += dy;
            windows[0].AddColoum(new Button(nx, ny, "", "add Texture", Command.addTex, false));

            // windows[0] is finished.
            dy = 48; ny += dy;
            // windows[1] starts
            windows.Add(new Window_utsList(20, ny, 200, 300));
        }

        protected void openUTD()
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
            int _xrate = windows[0].getColoumiContent_int(i);
            i++;
            int _yrate = windows[0].getColoumiContent_int(i);
            i++;
            string _mapName = windows[0].getColoumiContent_string(i);
            if (mapDataS == null)
            {
                mapDataS = new MapDataSave(_mapFileName,_mapName,_map_maxX,_map_maxY,_xrate,_yrate);
            }else if(mapDataS.fileName==_mapFileName){ mapDataS.changeTo(_mapFileName, _mapName, _map_maxX, _map_maxY,_xrate,_yrate); }
            else if (mapDataS.fileName!=_mapFileName) {
                Console.WriteLine("create New Map with Name: " + _mapFileName);
                Console.Write("Yes / No : ");
                string res = Console.ReadLine();
                if (res == "Yes") { mapDataS.changeTo(_mapFileName, _mapName, _map_maxX, _map_maxY,_xrate,_yrate); }
                else if(res=="No"){// res is "No"
                    // do nothing,
                }else { Console.Write("Invaild response as "+res+" .\nPlease Only Use \"Yes\" or \"No\" .(No Need space and \")"); }
            }
        }


        public override void SceneDraw(Drawing d) {
            base.SceneDraw(d);
            if (mapDataS != null)
            {
                #region map line draw
                switch (drawLine)
                {
                    case 1:
                        float lineWidth = 1;
                        int dx = 5,dy = 5;
                        Vector xb = new Vector(leftsideX, Math.Min(bottonsideY+lby*MapDataSave.Yrate,bottonsideY));
                        Vector xt = new Vector(leftsideX, Math.Max(xb.Y-mapDataS.max_y*MapDataSave.Yrate,topsideY));
                        if(lbx%dx == 0) {
                            //d.DrawLine(xb,xt, lineWidth, Color.White, DepthID.Map);
                        }else { xb.X += (lbx % dx)*MapDataSave.Xrate; }

                        for (int i = 0; dx*i <= (mapDataS.max_x - lbx) && dx*i <= (rightsideX - leftsideX) / MapDataSave.Xrate; i++)
                        {
                            if (dx*i >= -lbx) { d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map); }
                            xb.X += dx * MapDataSave.Xrate; xt.X = xb.X;
                        }
                        xb.X = Math.Max(leftsideX-lbx*MapDataSave.Xrate,leftsideX); xt.X = Math.Min(leftsideX+(mapDataS.max_x-lbx)*MapDataSave.Xrate,xb.X+mapDataS.max_x*MapDataSave.Xrate);
                        xb.Y = bottonsideY;xt.Y = xb.Y;
                        if (lby % dy == 0)
                        {
                            //d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map);
                        }
                        else { xb.Y += (xb.Y % dy)*MapDataSave.Yrate; }

                        for (int j = 0; dy*j <= (mapDataS.max_y-lby) && dy*j <= (bottonsideY - topsideY) / MapDataSave.Yrate; j++)
                        {
                            if (dy*j >= -lby) { d.DrawLine(xb, xt, lineWidth, Color.White, DepthID.Map); }
                            xb.Y -= dy * MapDataSave.Yrate; xt.Y = xb.Y;
                        }
                        break;
                    default:
                        break;
                }//switch what kind of draw end
                #endregion
                #region draw Unit
                foreach(Unit u in MapDataSave.utList)
                {
                    u.draw(d);
                }
                #endregion

            }// if map is not null end

        }//SceneDraw

        public override void SceneUpdate()
        {
            base.SceneUpdate();
            update_mapDataS();
        }
        protected virtual void update_mapDataS()
        {
            if (mouse.IsButtomDown(MouseButton.Left) ) {
                Vector mPos = mouse.MousePosition();
                if (mPos.X < rightsideX && mPos.X > leftsideX && mPos.Y < bottonsideY && mPos.Y > topsideY)
                {
                    Vector o_mPos = mouse.OldMousePosition();
                    lbx +=(int)( o_mPos.X - mPos.X);
                    lby +=(int)( mPos.Y - o_mPos.Y); // マップのyは画面の逆になっている。
                }
            }
            MapDataSave.update_map_xy(ltx, lty, rbx, rby);
        }
        protected override void switch_windowsIcommand(int i)
        {
            switch (windows[i].commandForTop)
            {
                case Command.CreateNewMapFile:
                    initializeMap();
                    break;
                case Command.openUTD:
                    openUTD();
                    break;
                case Command.openAniD:
                    //openAniD();
                    break;
                case Command.addTex:
                    addTex();
                    break;
                case Command.nothing:
                    break;
                default:
                    Console.WriteLine("window" + i + " " + windows[i].commandForTop);
                    break;
            }
        }


    }//class MapEditorScene end

}//namespace CommonPart End
