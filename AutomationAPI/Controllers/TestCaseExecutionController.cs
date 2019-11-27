using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutomationAPI.Helper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Models.TestCaseExecution;
using Service.Repositories.TestCaseExecution;

namespace AutomationAPI.Controllers
{
    [EnableCors("cors")]
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class TestCaseExecutionController : Controller
    {
        private readonly ITestCaseExecutionRepository _testCaseExecutionRepository;
        private readonly IConfiguration configuration;

        public TestCaseExecutionController(IConfiguration iConfig, ITestCaseExecutionRepository testCaseExecutionRepository)
        {
            configuration = iConfig;
            _testCaseExecutionRepository = testCaseExecutionRepository;
        }

        [HttpGet]
        public IActionResult GetTestSuiteDetails(string username)
        {
            try
            {
                return Ok(_testCaseExecutionRepository.GetTestSuiteDetails(username));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        public IActionResult GetTestSuiteResults()
        {
            try
            {
                return Ok(_testCaseExecutionRepository.GetTestSuiteResults());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        public IActionResult GetTestScenarioResults(int suiteExecutionID)
        {
            try
            {
                return Ok(_testCaseExecutionRepository.GetTestScenarioResults(suiteExecutionID));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public IActionResult ExecuteTestSuite([FromBody]List<TestSuite> testSuiteList)
        {
            try
            {
                var protractorService = new ProtractorService();
                foreach (var item in testSuiteList)
                {
                    protractorService.ExecuteTestSuite(configuration, _testCaseExecutionRepository, item);
                }
                return Ok(10);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}