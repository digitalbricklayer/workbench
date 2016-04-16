using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Test using the 4 Queens problem on a 4x4 chessboard.
    /// </summary>
    [TestFixture]
    public class FourQueensTests
    {
        private const int ExpectedQueens = 4;

        [Test]
        [Ignore("Not implemented the needed constraints.")]
        public void SolveWithFourQueensModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        [Ignore("Not implemented the needed constraints.")]
        public void SolveWithChessboardVisualizerAssignsFourQueens()
        {
            var sut = CreateWorkspace();
            sut.Solve();
            var chessboardVisualizer = (ChessboardVisualizerModel) sut.Solution.GetVisualizerFor("board");
            var allQueenSquares = chessboardVisualizer.GetSquaresOccupiedBy(PieceType.Queen);
            Assert.That(allQueenSquares, Has.Count.EqualTo(ExpectedQueens));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("4 Queens Model")
                                          .AddAggregate("board", ExpectedQueens, $"1..{ExpectedQueens}")
                                          .WithChessboardVisualizerBindingTo("board")
                                          .Build();

            var count = (AggregateVariableModel) workspace.Model.GetVariableByName("board");
            for (var i = 1; i <= count.AggregateCount; i++)
            {
                for (var j = 1; j <= i; j++)
                {
//                    workspace.Model.AddConstraint(new ConstraintModel($"board[{i}] <> board[{j}]"));
//                    workspace.Model.AddConstraint(new ConstraintModel($"board[{i}] + 1 != board[{j}] + 1"));
//                    workspace.Model.AddConstraint(new ConstraintModel($"board[{i}] - 1 != board[{j}] - 1"));
                }
            }

            return workspace;
        }
    }
}
