using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary.UnitTests
{
    [TestClass]
    public class WeatherSerivceTests
    {
        [TestMethod]
        public async Task WeatherSericeTests_CorrectDataInRepository_ServiceReturnsDatabaseData()
        {
            // Arrange
            var apiKey = "test";
            var returnedList = new[]
            {
                new WeatherDataQuery
                {
                    CityName = "szczecin",
                    Id=0,
                    Time = DateTime.Now-TimeSpan.FromMinutes(1),
                    WeatherData = new WeatherData() {Id =2137}
                },
                new WeatherDataQuery
                {
                    CityName = "warszawa",
                    Id=0,
                    Time = DateTime.Now-TimeSpan.FromMinutes(3),
                    WeatherData = new WeatherData()
                }
            };

            var weatherDataRepositoryMock = new Mock<IWeatherDataRepository>(MockBehavior.Loose);
            weatherDataRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(returnedList.AsQueryable());

            var configMock = new Mock<IConfiguration>(MockBehavior.Loose);
            configMock.Setup(x => x["ApiKey"])
                .Returns(apiKey);

            var sut = new WeatherService(
                configMock.Object,
                weatherDataRepositoryMock.Object,
                null);
            // Act

            var result = await sut.GetWeather("Szczecin", false);
            // Assert

            weatherDataRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            Assert.AreEqual(sut.apikey, apiKey);
            Assert.AreEqual(result.Id, 2137);
        }

        public async Task WeatherSericeTests_NoDataInRepository_ServiceDownloadsData()
        {
            // Arrange
            var apiKey = "testKey";
            var returnedList = new WeatherDataQuery[0];

            var weatherDataRepositoryMock = new Mock<IWeatherDataRepository>(MockBehavior.Loose);
            weatherDataRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(returnedList.AsQueryable());

            var configMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configMock.Setup(x => x["ApiKey"])
                .Returns(apiKey);

            var httpClient = new HttpClient();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Loose);
            httpClientFactoryMock.Setup(x => x.CreateClient())
                .Returns(httpClient);

            var sut = new WeatherService(
                configMock.Object,
                weatherDataRepositoryMock.Object,
                httpClientFactoryMock.Object);
            // Act

            var result = await sut.GetWeather("Szczecin", false);
            // Assert

            weatherDataRepositoryMock.Verify(x => x.GetAll(), Times.Once);
            Assert.AreEqual(sut.apikey, apiKey);
            Assert.AreEqual(result.Id, 2137);
        }

        [TestMethod]
        public void TestNoNameLol()
        {
            string reversedName = "";


            var stringHelperMock = new Mock<IStringHelper>(MockBehavior.Strict);
            stringHelperMock.Setup(x => x.Reverse(It.IsAny<string>()))
                .Returns<string>(x=>
                {
                    reversedName = new string(x.Reverse().ToArray());
                    return reversedName;
                });


            var sut = new NewTest(stringHelperMock.Object);

            var testName = "EloElo234";

            var result = sut.GetMyNameReversed(testName);

            Assert.AreEqual(result, reversedName);
           
        }
    }
}