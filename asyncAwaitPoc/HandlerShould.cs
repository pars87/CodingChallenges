using FluentAssertions;
using NSubstitute;
using Xunit;

namespace asyncAwaitPoc;

public class HandlerShould
{
    [Fact]
    public async Task TestThatSavesAreAwaitedInOrder()
    {
        // Arrange
        var repo = Substitute.For<IRepository>();
        var test = Substitute.For<ITestInterface>();
        var sut = new Handler(repo);

        repo.Save()
            .Returns(async _ =>
            {
                await Task.Delay(100);
                test.AwaitA();
            }, async _ =>
            {
                await Task.Delay(1);
                test.AwaitB();
            });

        // Act
        await sut.ExecuteDoubleSave();

        // Assert
        Received.InOrder(() =>
        {
            test.AwaitA();
            test.AwaitB();
        });
    }

    [Fact]
    public async Task TestThatSavesAreAwaitedInOrderVersion2()
    {
        // Arrange
        var repo = Substitute.For<IRepository>();
        var sut = new Handler(repo);

        var isAwaitedA = false;
        var isAwaitedB = false;

        repo.Save()
            .Returns(async _ =>
            {
                await Task.Delay(1);
                isAwaitedA = true;

            }, async _ =>
            {
                await Task.Delay(100);
                isAwaitedB = true;
            });

        // Act
        await sut.ExecuteDoubleSave();

        // Assert
        await repo.Received(2).Save();

        isAwaitedA.Should().BeTrue();
        isAwaitedB.Should().BeTrue();
    }

    [Fact]
    public async Task TestThatSaveIsAwaited()
    {
        // Arrange
        var repo = Substitute.For<IRepository>();
        var test = Substitute.For<ITestInterface>();
        var sut = new Handler(repo);

        repo.Save()
            .Returns(async _ =>
            {
                await Task.Delay(1);
                test.AwaitA();
            });

        // Act
        await sut.ExecuteSave();

        // Assert
        test.Received().AwaitA();
    }

    [Fact]
    public async Task TestThatSaveIsAwaitedVersion2()
    {
        // Arrange
        var repo = Substitute.For<IRepository>();
        var sut = new Handler(repo);

        var isAwaited = false;
        repo.Save()
            .Returns(async _ =>
            {
                await Task.Delay(1);
                isAwaited = true;
            });

        // Act
        await sut.ExecuteSave();

        // Assert
        await repo.Received(1).Save();
        isAwaited.Should().BeTrue();
    }

    public interface ITestInterface
    {
        void AwaitA();
        void AwaitB();
    }
}