using System.Collections.Generic;
using Banchou.Pawn;

namespace Banchou.Board {
    public class BoardState {
        public Dictionary<string, PawnState> Pawns = new Dictionary<string, PawnState>();
        public BoardState() { }
        public BoardState(BoardState prev) {
            Pawns = prev?.Pawns ?? new Dictionary<string, PawnState>();
        }
    }
}