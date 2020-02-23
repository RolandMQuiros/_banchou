using System.Collections.Generic;
using Banchou.Pawn;

namespace Banchou.Board {
    public class BoardState {
        public Dictionary<string, PawnState> Pawns = new Dictionary<string, PawnState>();

        public BoardState(
            BoardState prev = null,
            Dictionary<string, PawnState> pawns = null
        ) {
            Pawns = pawns ?? prev?.Pawns ?? new Dictionary<string, PawnState>();
        }
    }
}