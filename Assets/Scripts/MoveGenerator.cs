using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGenerator
{
    private static readonly Vector2Int[] rookDirections = {
        new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)
    };

    private static readonly Vector2Int[] bishopDirections = {
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)
    };

    public static List<ChessMove> getLegalMoves(BoardState state, PieceColor color) {
        var legalMoves = new List<ChessMove>();

        foreach(var unfilteredMove in getUnfilteredMoves(state, color)) {
            var stateCheck = state.cloneBoard();
            stateCheck.applyMove(unfilteredMove);

            if(!isInCheck(stateCheck, color)) legalMoves.Add(unfilteredMove);
        }

        return legalMoves;
    }

    public static bool isInCheck(BoardState state, PieceColor color) {
        var kingPos = state.findKing(color);

        if(kingPos.x == -1) return true;

        var opponentColor = state.opponent(color);

        foreach(var possibleOpponentMove in getUnfilteredMoves(state, opponentColor)) {
            if(possibleOpponentMove.to == kingPos) return true;
        }

        return false;
    }

    public static bool isCheckmate(BoardState state, PieceColor color) {
        return isInCheck(state, color) && getLegalMoves(state, color).Count == 0;
    }

    public static bool isStalemate(BoardState state, PieceColor color) {
        return !isInCheck(state, color) && getLegalMoves(state, color).Count == 0;
    }

    private static List<ChessMove> getUnfilteredMoves(BoardState state, PieceColor color) {
        var unfilteredMoves = new List<ChessMove>();

        for (int col = 0; col < 8; col++) {
            for (int row = 0; row < 8; row++) {
                var currentTile = state.board[col, row];
                if (!currentTile.HasValue || currentTile.Value.color != color) continue;

                var initialPos = new Vector2Int(col, row);
                foreach (var destination in getTargetsForPiece(state, initialPos, currentTile.Value.type, currentTile.Value.color)) {
                    unfilteredMoves.Add(new ChessMove(initialPos, destination));
                }
            }
        }
        return unfilteredMoves;
    }

    private static List<Vector2Int> getTargetsForPiece(BoardState state, Vector2Int initialPos, PieceType type, PieceColor color) {

    }
}
