using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the table derived domain values successfully.
    /// </summary>
    public class TableVariableDomainSolverWithWholeColumnRangeShould
    {
        /// <summary>
        /// Gets or sets the test subject.
        /// </summary>
        public WorkspaceModel Subject { get; set; }

        [SetUp]
        public void Initialize()
        {
            Subject = new ContrivedTableSharedDomainWorkspaceBuilder().CreateWorkspace();
        }

        [Test]
        public void SolveWithListModelReturnsStatusSuccess()
        {
            var actualResult = Subject.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithTableReturnsSnapshotWithValidResult()
        {
            var actualResult = Subject.Solve();
            var aLabel = actualResult.Snapshot.GetLabelByVariableName("a");
            Assert.That(aLabel.Value, Is.EqualTo("Bodie"));
        }
    }
}
