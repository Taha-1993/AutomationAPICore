using Service.Models.TestCaseExecution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repositories.TestCaseExecution
{
    public interface ITestCaseExecutionRepository
    {
        IEnumerable<dynamic> GetTestSuiteDetails(string username);

        IEnumerable<dynamic> GetTestSuiteResults();

        IEnumerable<dynamic> GetTestScenarioResults(int suiteExecutionID);

        int UpsertTestSuiteExecutionResult(TestSuite testSuite);
    }
}
