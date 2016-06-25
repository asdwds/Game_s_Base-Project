using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    class Map: WorkerWithPos {
        // へクスの二次元配列
        private List<List<Hex>> hexes;

        protected Map(WorkerManager w)
            : base(w, new Vector(0,0)) {
            Pos = new Vector(0, 0);
        }
        public override void Update() {
            // TODO
        }
        public override WorkerType Type { get; }
        public override void Draw(Drawing d) {
            // TODO
        }
    }
}
