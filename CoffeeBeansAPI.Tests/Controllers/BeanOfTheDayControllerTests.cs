using CoffeeBeanAPI.Controllers;
using CoffeeBeanAPI.Data;
using CoffeeBeanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeBeanAPI.Tests.Controllers
{
    public class BeanOfTheDayControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly BeanOfTheDayController _controller;
        private readonly List<Bean> _testBeans;

        public BeanOfTheDayControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _controller = new BeanOfTheDayController(_context);
            _testBeans = CreateTestBeans();
            
            _context.Beans.AddRange(_testBeans);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private List<Bean> CreateTestBeans()
        {
            return new List<Bean>
            {
                new Bean
                {
                    Id = Guid.NewGuid(),
                    _id = "1",
                    Name = "Test Bean 1",
                    Country = "Colombia",
                    colour = "Dark Roast",
                    Cost = "15.99",
                    Description = "Test Description 1",
                    Image = "test1.jpg",
                    isBOTD = false
                },
                new Bean
                {
                    Id = Guid.NewGuid(),
                    _id = "2",
                    Name = "Test Bean 2",
                    Country = "Brazil",
                    colour = "Medium Roast",
                    Cost = "20.50",
                    Description = "Test Description 2",
                    Image = "test2.jpg",
                    isBOTD = false
                }
            };
        }

        [Fact]
        public async Task GetBeanOfTheDay_ReturnsSameBean_ForSameDay()
        {
            // Act
            var result1 = await _controller.GetBeanOfTheDay();
            var result2 = await _controller.GetBeanOfTheDay();

            // Assert
            var bean1 = Assert.IsType<Bean>(result1.Value);
            var bean2 = Assert.IsType<Bean>(result2.Value);
            Assert.Equal(bean1.Id, bean2.Id);
            Assert.True(bean1.isBOTD);
        }

        [Fact]
        public async Task GetBeanOfTheDay_UpdatesPreviousBeanStatus()
        {
            // Arrange
            var previousBean = _testBeans.First();
            previousBean.isBOTD = true;
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBeanOfTheDay();

            // Assert
            var newBeanOfDay = Assert.IsType<Bean>(result.Value);
            var oldBean = await _context.Beans.FindAsync(previousBean.Id);
            Assert.False(oldBean.isBOTD);
            Assert.True(newBeanOfDay.isBOTD);
        }

        [Fact]
        public async Task GetBeanOfTheDay_DoesNotSelectPreviousBean()
        {
            // Arrange
            var firstResult = await _controller.GetBeanOfTheDay();
            var firstBean = Assert.IsType<Bean>(firstResult.Value);

            // Modify the date to simulate next day
            var botd = await _context.BeanOfTheDays.FirstAsync();
            botd.SelectedDate = DateTime.UtcNow.Date.AddDays(-1);
            await _context.SaveChangesAsync();

            // Act
            var secondResult = await _controller.GetBeanOfTheDay();
            var secondBean = Assert.IsType<Bean>(secondResult.Value);

            // Assert
            Assert.NotEqual(firstBean.Id, secondBean.Id);
        }
    }
}