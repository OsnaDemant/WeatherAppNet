using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.UnitTests.Helper;
using WeatherServiceLibrary;
using Microsoft.Extensions.Configuration;
using WeatherServiceLibrary.Entities;
using Newtonsoft.Json;
using WeatherServiceLibrary.Exceptions;

namespace WeatherServiceLibrary.UnitTests
{
    [TestClass]
    public class DataDownloadTests
    {
        [TestMethod]
        public async Task DataDownloadTests_CorrectHttpStatus_RefreshWeatherhReturnsCode200()
        {
            //Arrange
            var weatherData = new WeatherData();

            weatherData.Visibility = 1000;
            var weatherDataJson = JsonConvert.SerializeObject(weatherData);
            var apiKey = "testKey";

            var handlerStub = new DelegatingHandlerStub(System.Net.HttpStatusCode.OK, weatherDataJson);
            var client = new HttpClient(handlerStub);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Loose);
            httpClientFactoryMock.Setup(x => x.CreateClient(string.Empty))
                .Returns(client);

            var configMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configMock.Setup(x => x["ApiKey"])
                .Returns(apiKey);

            var sut = new DataDownload(configMock.Object, httpClientFactoryMock.Object);

            //Act
            var response = await sut.RefreshWeather("Szczecin");

            //Asssert
            // Assert.IsNotNull(response);
            Assert.AreEqual(response.WeatherData.Visibility, weatherData.Visibility);
            httpClientFactoryMock.Verify(x => x.CreateClient(String.Empty), Times.Once);

        }
        [TestMethod]
        public async Task DataDownloadTests_UnCorrectHttpStatus_RefreshWeatherReturnsCode401()
        {
            //Arrange
            var weatherData = new WeatherData();
            weatherData.Visibility = 1000;
            var weatherDataJson = JsonConvert.SerializeObject(weatherData);
            var apiKey = "testKey";

            var handlerStub = new DelegatingHandlerStub(System.Net.HttpStatusCode.Unauthorized, weatherDataJson);
            var client = new HttpClient(handlerStub);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Loose);
            httpClientFactoryMock.Setup(x => x.CreateClient(string.Empty))
                .Returns(client);

            var configMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configMock.Setup(x => x["ApiKey"])
                .Returns(apiKey);

            var sut = new DataDownload(configMock.Object, httpClientFactoryMock.Object);

            //Act
            //Assert

            await Assert.ThrowsExceptionAsync<UnauthorizedException>(() => sut.RefreshWeather("Szczecin"));
        }

        [TestMethod]
        public async Task DataDownloadTests_UnnownErrorRetrivingData_RefreshWeatherReturnsSpecifiedMessage()
        {
            //Arrange
            var weatherData = new WeatherData();

            weatherData.Visibility = 1000;
            var weatherDataJson = JsonConvert.SerializeObject(weatherData);
            var apiKey = "testKey";

            var handlerStub = new DelegatingHandlerStub(System.Net.HttpStatusCode.NotFound, weatherDataJson);
            var client = new HttpClient(handlerStub);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Loose);
            httpClientFactoryMock.Setup(x => x.CreateClient(string.Empty))
                .Returns(client);

            var configMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configMock.Setup(x => x["ApiKey"])
                .Returns(apiKey);

            var sut = new DataDownload(configMock.Object, httpClientFactoryMock.Object);

            //Act
            //Assert

            await Assert.ThrowsExceptionAsync<ApplicationException>(() => sut.RefreshWeather("Szczecin"));
        }


    }
}
