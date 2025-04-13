using CarsMVC25.Data;
using CarsMVC25.Data.Entities;
using CarsMVC25.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;

namespace CarsMVC25.Tests
{
    public class DataTests
    {
        private Mock<DbSet<Car>> _mockSet;
        private Mock<ApplicationDbContext> _mockContext;
        private CrudRepository<Car> _repository;
        private List<Car> _data;

        [SetUp]
        public void Setup()
        {
            _data = new List<Car>
        {
            new Car { Id = 1, Brand = "Toyota", ModelName = "Corolla", Year = 2020 },
            new Car { Id = 2, Brand = "Honda", ModelName = "Civic", Year = 2019 }
        };

            var asyncData = _data.AsQueryable().ToAsyncEnumerable();

            _mockSet = new Mock<DbSet<Car>>();

            // This supports .ToListAsync() and other async methods
            _mockSet.As<IAsyncEnumerable<Car>>()
                    .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                    .Returns((CancellationToken _) => asyncData.GetAsyncEnumerator());

            // Support LINQ operations
            _mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(_data.AsQueryable().Provider);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(_data.AsQueryable().Expression);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(_data.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

            _mockContext = new Mock<ApplicationDbContext>(options);
            _mockContext.Setup(c => c.Set<Car>()).Returns(_mockSet.Object);

            _repository = new CrudRepository<Car>(_mockContext.Object);
        }

        [Test]
        public async Task AddAsync_Should_Add_Car()
        {
            var newCar = new Car { Id = 3, Brand = "Ford", ModelName = "Focus", Year = 2021 };

            await _repository.AddAsync(newCar);

            _mockSet.Verify(m => m.AddAsync(newCar, It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_Should_Remove_Car_If_Exists()
        {
            var car = _data.First();
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(car);

            await _repository.DeleteByIdAsync(1);

            _mockSet.Verify(m => m.Remove(car), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_Should_Not_Remove_Car_If_Not_Found()
        {
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Car)null);

            await _repository.DeleteByIdAsync(99);

            _mockSet.Verify(m => m.Remove(It.IsAny<Car>()), Times.Never);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task GetAllAsync_Should_Return_All_Cars()
        {
            var result = await _repository.GetAllAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetByFilterAsync_Should_Return_Filtered_Cars()
        {
            var result = await _repository.GetByFilterAsync(c => c.Brand == "Toyota");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Toyota", result.First().Brand);
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_Car_If_Found()
        {
            var car = _data.First();
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(car);

            var result = await _repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Toyota", result.Brand);
        }

        [Test]
        public async Task UpdateAsync_Should_Modify_Car_And_Save()
        {
            var car = new Car { Id = 2, Brand = "Mazda", ModelName = "3", Year = 2022 };

            _mockContext.Setup(c => c.Entry(car)).Returns(Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Car>>());

            await _repository.UpdateAsync(car);

            _mockSet.Verify(m => m.Update(car), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}