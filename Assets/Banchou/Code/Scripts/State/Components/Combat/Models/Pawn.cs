using System.Linq;
using System.Collections.Generic;

namespace Banchou {
    public class Pawn {
        public interface IQueuedCommand { }
        public string ID = default(string);
        public int Health = default(int);
        public IEnumerable<IQueuedCommand> Commands = Enumerable.Empty<IQueuedCommand>();

        public Pawn(
            Pawn prev = null,
            string id = null,
            int? health = null,
            IEnumerable<IQueuedCommand> commands = null
        ) {
            ID = id ?? prev?.ID ?? ID;
            Health = health ?? prev?.Health ?? Health;
            Commands = commands ?? prev?.Commands ?? Commands;
        }
    }
}