using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart {
    /// <summary>
    /// マップ作成のシーンのクラス
    /// </summary>
    class UTDEditorScene : Scene {
        static private List<Window> windows = new List<Window>();
        static public bool ready { get; private set; } = false;
        static public Window_UnitType window_ut;

        public UTDEditorScene(SceneManager s) : base(s)
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
            windows.Add(new Window_WithColoum(20, 20, 90, 90));
            ((Window_WithColoum) windows[0] ).AddColoum(new Coloum(nx, ny, "version: "+DataBase.ThisSystemVersionNumber.ToString(), Command.nothing));
            nx = 5; ny += 15;dx = 30;
            windows[0].AddColoum(new Button(nx, ny, "", "close UTD", Command.closeUTD, false));
            nx += dx;
            windows[0].AddColoum(new Button(nx, ny, "", "add Texture", Command.addTex, false));
            // windows[0] is finished.

            // windows[1] starts
            windows.Add(new Window_utsList(20, ny + 20, 150, 150));

            // windows[2] starts
            window_ut= new Window_UnitType(DataBase.getUnitType(null),60, ny + 20, 150, 150);
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

        public override void SceneDraw(Drawing d) {

           
        }//SceneDraw

        public override void SceneUpdate() {
            base.SceneUpdate();

            #region update windows
            int chosenIndex = 0;
            for(int i = 0; i < windows.Count; i++)
            {
                if (windows[i].PosInside(mouse.MousePosition()) ) {
                    chosenIndex = i;
                }else
                {
                    windows[i].update();
                }

            }
            windows[chosenIndex].update((KeyManager)Input, mouse);
            switch (windows[chosenIndex].commandForTop)
            {
                case Command.addTex:
                    addTex();
                    break;
                case Command.nothing:
                    break;
                case Command.UTDutButtonPressed:
                    window_ut.setup_unitType_window(
                        DataBase.getUnitType(windows[chosenIndex].getNowColoumContent_string())  );
                    break;
                default:
                    Console.WriteLine("UTD:window" + chosenIndex + " " + "strangeCommand");
                    break;

            }
            #endregion
        }//SceneUpdate() end

    }//class UTDEditorScene end


}//namespace CommonPart End
