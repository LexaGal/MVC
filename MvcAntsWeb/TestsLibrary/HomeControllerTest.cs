using System.Net.Http;
using System.Web.Http;
using Algorithm.Controllers;
using AntsAlg.AntsAlgorithm.Algorithm;
using DatabaseAccess.Repository.Abstract;
using DatabaseAccess.Repository.Concrete;
using Entities.DatabaseModels;
using Moq;
using NUnit.Framework;

namespace TestsLibrary
{
    [TestFixture]
    class HomeControllerTest
    {
        public void ProcessChoosenItems_Ok()
        {
            StandartAlgorithmBuilder builder = Mock.Of<StandartAlgorithmBuilder>();
            IParametersRepository parameters = Mock.Of<ParametersRepository>();
            IDistMatricesRepository distMatrices = Mock.Of<DistMatricesRepository>();
            IFlowMatricesRepository flowMatrices = Mock.Of<FlowMatricesRepository>();
            IResultsInfoRepository resultsInfo = Mock.Of<ResultsInfoRepository>();

            // Arrange
            var controller = new HomeController(builder, parameters, distMatrices, flowMatrices, resultsInfo);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            ResultInfo result = new ResultInfo {Id = 10};
            Assert.IsTrue(response.Id == result.Id);
            Assert.AreEqual(10, response.Id);
        }
    }
}
