using Microsoft.Extensions.Configuration;
using Service.Models.TestCaseExecution;
using Service.Repositories.TestCaseExecution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationAPI.Helper
{
    public class ProtractorService
    {
        public void ExecuteTestSuite(IConfiguration iConfig, ITestCaseExecutionRepository testCaseExecutionRepository, TestSuite testSuite)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";

                try
                {
                    testSuite.ExecutionStatus = ExecutionStatusSetup.Fail;
                    testSuite.RowID = testCaseExecutionRepository.UpsertTestSuiteExecutionResult(testSuite);

                    if (testSuite.RowID != default(int?) || testSuite.RowID != 0)
                    {
                        StartSeleniumServer(iConfig);

                        var command = String.Empty;

                        if (testSuite.ProjectName == "MAP-Internal Portal")
                        {
                            command = $@"/K cd { iConfig.GetSection("AppConfiguration").GetSection("InternalPortalPath").Value }&protractor config.js --suite { testSuite.SuiteTypeName }  --params.Suite_Name { testSuite.SuiteTypeName } --params.UserName { testSuite.Username }";
                        }
                        else if (testSuite.ProjectName == "MAP-External Portal")
                        {
                            command = $@"/K cd { iConfig.GetSection("AppConfiguration").GetSection("ExternalPortalPath").Value }&protractor config.js --suite { testSuite.SuiteTypeName } --params.Suite_Name { testSuite.SuiteTypeName } --params.UserName { testSuite.Username } --params.Suite_Execution_ID { testSuite.RowID }";
                        }

                        process.StartInfo.Arguments = $"{ command } & timeout 5 & exit";

                        process.Start();
                        //process.WaitForExit(); 
                    }
                }
                catch (Exception ex)
                {
                    testSuite.ExecutionStatus = ExecutionStatusSetup.Fail;
                    testCaseExecutionRepository.UpsertTestSuiteExecutionResult(testSuite);
                }
            }
        }

        private void StartSeleniumServer(IConfiguration iConfig)
        {
            try
            {
                var json = new WebClient().DownloadString(iConfig.GetSection("AppConfiguration").GetSection("SeleniumServer").Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/K webdriver-manager start";
                    process.Start();
                }
                Thread.Sleep(5000);
            }
        }
    }
}
