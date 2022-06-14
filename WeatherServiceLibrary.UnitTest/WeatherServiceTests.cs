using Castle.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary;
using WeatherServiceLibrary.Entities;


namespace WeatherServiceLibrary.UnitTest;


[TestClass]
public class WeatherServiceTests
{

    [TestMethod]
    public async Task WeatherServiceTests_CorrectDataFromServer_DataDownloadReturnsServerData()
    {
        // Arrange
        var CityName = "Szczecin";
        // var apiKey = "test";
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

        var dataDownload = new Mock<IDataDownload>(MockBehavior.Loose);
        dataDownload
            .Setup(x => x.RefreshWeather(CityName))
            .ReturnsAsync(returnedList.FirstOrDefault());


        var sut = new WeatherService(
            dataDownload.Object,
            weatherDataRepositoryMock.Object
            );
        // Act

        var result = await sut.GetWeather("Szczecin", false);
        // Assert

        weatherDataRepositoryMock.Verify(x => x.GetAll(), Times.Once);

        Assert.AreEqual(2137, result.Id);
    }

    [TestMethod]
    public async Task WeatherServiceTests_CorrectDataAfterOneHourInRepository_ServiceReturnsNewDataAfterOneHour()
    {
        //arrange
        var CityName = "Szczecin";
        var databaseList = new[]
        {
                new WeatherDataQuery
                {
                    CityName = "szczecin",
                    Id=0,
                    Time = DateTime.UtcNow-TimeSpan.FromMinutes(1),
                    WeatherData = new WeatherData() {Name = CityName, Id=0}
                },
                new WeatherDataQuery
                {
                    CityName = "warszawa",
                    Id=1,
                    Time = DateTime.UtcNow-TimeSpan.FromMinutes(61),
                    WeatherData = new WeatherData() {Name = "warszawa"  ,Id=1}
                }
            };

        var weatherDataRepositoryMock = new Mock<IWeatherDataRepository>(MockBehavior.Loose);
        weatherDataRepositoryMock
            .Setup(x => x.GetAll())
            .Returns(databaseList.AsQueryable());

        var downloadedData = new WeatherDataQuery
        {
            CityName = "warszawa",
            Id = 2,
            Time = DateTime.UtcNow,
            WeatherData = new WeatherData() { Name = "warszawa", Id = 2 }
        };

        var dataDownloadMock = new Mock<IDataDownload>(MockBehavior.Loose);
        dataDownloadMock
            .Setup(x => x.RefreshWeather(It.IsAny<string>()))
            .ReturnsAsync(downloadedData);

        var sut = new WeatherService(
            dataDownloadMock.Object,
            weatherDataRepositoryMock.Object
            );
        //act
        var result = await sut.GetWeather("Warszawa", false);

        //assert
        dataDownloadMock.Verify(x => x.RefreshWeather(It.IsAny<string>()));

        Assert.AreEqual(result.Id, downloadedData.Id);
        Assert.AreNotEqual(databaseList[1].Id, result.Id);
    }

    [TestMethod]
    public async Task WeatherServiceTests_CorrectDataWhenRefreshDataIsTrue_ServiceReturnsDatabaseData()
    {
        //arrange
        var CityName = "Szczecin";
        var databaseList = new[]
       {
                new WeatherDataQuery
                {
                    CityName = "szczecin",
                    Id=0,
                    Time = DateTime.UtcNow-TimeSpan.FromMinutes(1),
                    WeatherData = new WeatherData() {Name = CityName, Id=0}
                },
                new WeatherDataQuery
                {
                    CityName = "warszawa",
                    Id=1,
                    Time = DateTime.UtcNow-TimeSpan.FromMinutes(1),
                    WeatherData = new WeatherData() {Name = "warszawa"  ,Id=1}
                }
            };
        var weatherDataRepositoryMock = new Mock<IWeatherDataRepository>(MockBehavior.Loose);
        weatherDataRepositoryMock
            .Setup(x => x.GetAll())
            .Returns(databaseList.AsQueryable());

        var downloadedData = new WeatherDataQuery
        {
            CityName = "warszawa",
            Id = 2,
            Time = DateTime.UtcNow,
            WeatherData = new WeatherData() { Name = "warszawa", Id = 2 }
        };

        var dataDownloadMock = new Mock<IDataDownload>(MockBehavior.Loose);
        dataDownloadMock
            .Setup(x => x.RefreshWeather(It.IsAny<string>()))
            .ReturnsAsync(downloadedData);

        var sut = new WeatherService(
            dataDownloadMock.Object,
            weatherDataRepositoryMock.Object
            );

        //act
        var result = await sut.GetWeather("Warszawa",true);

        //assert
        dataDownloadMock.Verify(x => x.RefreshWeather(It.IsAny<string>()));

        Assert.AreEqual(result.Id, downloadedData.Id);
        Assert.AreNotEqual(databaseList[1].Id, result.Id);
    }
}