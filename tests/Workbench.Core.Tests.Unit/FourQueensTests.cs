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
        public void SolveWithFourQueensModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithFourQueensModelSolutionContainsFourQueens()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var actualColumnValues = actualResult.Snapshot.GetAggregateVariableValueByName("cols").Values;
            Assert.That(actualColumnValues, Is.Unique);
            Assert.That(actualColumnValues, Is.All.GreaterThanOrEqualTo(1)
                                                  .And
                                                  .LessThanOrEqualTo(ExpectedQueens));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("4 Queens Model")
                                          .AddAggregate("cols", ExpectedQueens, $"1..{ExpectedQueens}")
                                          .WithConstraintAllDifferent("cols")
                                          .Build();

            var columnsVariable = (AggregateVariableModel) workspace.Model.GetVariableByName("cols");
            for (var i = 0; i < columnsVariable.AggregateCount; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    workspace.Model.AddConstraint(new ExpressionConstraintModel($"cols[{i+1}] != cols[{j+1}]"));
                    workspace.Model.AddConstraint(new ExpressionConstraintModel($"cols[{i+1}] + {i+1} != cols[{j+1}] + {j+1}"));
                    workspace.Model.AddConstraint(new ExpressionConstraintModel($"cols[{i+1}] - {i+1} != cols[{j+1}] - {j+1}"));
                }
            }

            return workspace;
        }
    }
}
