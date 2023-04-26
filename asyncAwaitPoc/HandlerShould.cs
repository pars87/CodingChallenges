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

        repo.Save().Returns(async _ =>
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
    public async Task TestThatSaveIsAwaited()
    {
        // Arrange
        var repo = Substitute.For<IRepository>();
        var test = Substitute.For<ITestInterface>();
        var sut = new Handler(repo);

        repo.Save().Returns(async _ =>
        {
            await Task.Delay(1);
            test.AwaitA();
        });

        // Act
        await sut.ExecuteSave();

        // Assert
        test.Received().AwaitA();

    }
}

public interface ITestInterface
{
    void AwaitA();
    void AwaitB();
}
