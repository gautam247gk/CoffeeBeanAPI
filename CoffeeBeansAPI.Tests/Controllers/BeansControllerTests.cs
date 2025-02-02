using CoffeeBeanAPI.Controllers;
using CoffeeBeanAPI.Data;
using CoffeeBeanAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeBeanAPI.Tests.Controllers
{
    public class BeansControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly BeansController _controller;
        private readonly List<Bean> _testBeans;

        public BeansControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _controller = new BeansController(_context);
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
                    Name = "Test Espresso",
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
                    Name = "Test Arabica",
                    Country = "Brazil",
                    colour = "Medium Roast",
                    Cost = "20.50",
                    Description = "Test Description 2",
                    Image = "test2.jpg",
                    isBOTD = true
                }
            };
        }

        [Fact]
        public async Task GetBeans_ReturnsAllBeans()
        {
            // Act
            var result = await _controller.GetBeans();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Bean>>>(result);
            var beans = Assert.IsAssignableFrom<IEnumerable<Bean>>(actionResult.Value);
            Assert.Equal(2, beans.Count());
        }

        [Fact]
        public async Task GetBean_WithValidId_ReturnsBean()
        {
            // Arrange
            var expectedBean = _testBeans.First();

            // Act
            var result = await _controller.GetBean(expectedBean.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Bean>>(result);
            var bean = Assert.IsType<Bean>(actionResult.Value);
            Assert.Equal(expectedBean.Id, bean.Id);
            Assert.Equal(expectedBean.Name, bean.Name);
        }

        [Fact]
        public async Task GetBean_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetBean(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBean_WithValidBean_ReturnsCreatedAtAction()
        {
            // Arrange
            var newBean = new Bean
            {
                Id = Guid.NewGuid(),
                _id = "3",
                Name = "New Bean",
                Country = "Ethiopia",
                colour = "Light Roast",
                Cost = "25.99",
                Description = "New Test Bean",
                Image = "new.jpg"
            };

            // Act
            var result = await _controller.PostBean(newBean);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Bean>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Bean>(createdAtActionResult.Value);
            Assert.Equal(newBean.Id, returnValue.Id);
            Assert.Equal(newBean.Name, returnValue.Name);
        }

        [Fact]
        public async Task SearchBeans_WithValidName_ReturnsMatchingBeans()
        {
            // Act
            var result = await _controller.SearchBeans("Espresso", null, null, null, null, null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Bean>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var beans = Assert.IsType<List<Bean>>(okResult.Value);
            Assert.Single(beans);
            Assert.Contains(beans, b => b.Name.Contains("Espresso"));
        }

        [Fact]
        public async Task SearchBeans_WithPriceRange_ReturnsMatchingBeans()
        {
            // Act
            var result = await _controller.SearchBeans(null, null, null, null, 15m, 21m);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Bean>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var beans = Assert.IsType<List<Bean>>(okResult.Value);
            Assert.Equal(2, beans.Count);
            Assert.All(beans, b => Assert.True(decimal.Parse(b.Cost) >= 15m && decimal.Parse(b.Cost) <= 21m));
        }

        [Fact]
        public async Task DeleteBean_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var beanToDelete = _testBeans.First();

            // Act
            var result = await _controller.DeleteBean(beanToDelete.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Beans.FindAsync(beanToDelete.Id));
        }

        [Fact]
        public async Task PutBean_WithValidUpdate_ReturnsNoContent()
        {
            // Arrange
            var beanToUpdate = _testBeans.First();
            beanToUpdate.Name = "Updated Name";

            // Act
            var result = await _controller.PutBean(beanToUpdate.Id, beanToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedBean = await _context.Beans.FindAsync(beanToUpdate.Id);
            Assert.Equal("Updated Name", updatedBean.Name);
        }
    }
}