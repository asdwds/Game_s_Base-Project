using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    abstract class SceneWithWindows :Scene
    {
        protected List<Window> windows = new List<Window>();

        public SceneWithWindows(SceneManager scene) : base(scene) { }

        abstract protected void setup_windows();
        protected virtual void close()
        {
            Delete = true;
            windows.Clear();
        }
        /// <summary>
        /// windowsのすべてのdrawとmousePositionを表示
        /// </summary>
        /// <param name="d"></param>
        public override void SceneDraw(Drawing d)
        {
            foreach (Window w in windows) { w.draw(d); }
            
        }//SceneDraw
        abstract protected void switch_windowsIcommand(int i);
        /// <summary>
        /// sceneのbaseのupdate()とwindowsのupdate
        /// </summary>
        public override void SceneUpdate()
        {
            base.SceneUpdate();
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].PosInside(mouse.MousePosition()))
                {
                    windows[i].update((KeyManager)Input, mouse);
                    switch_windowsIcommand(i);
                    if (windows.Count > i) { windows[i].commandForTop = Command.nothing; }
                }
                else
                {
                    windows[i].update();
                }
            }
        }//SceneUpdate() end
    }//class SceneWithWindows end

}//namespace CommonPart end
