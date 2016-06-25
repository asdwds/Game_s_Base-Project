using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    /// <summary>
    /// ユニットのクラス
    /// </summary>
    class Unit: Worker {

        protected Unit(WorkerManager w)
            : base(w) {
            // TODO
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
