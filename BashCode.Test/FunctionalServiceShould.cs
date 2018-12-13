namespace BashCode.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Xunit;

    public class FunctionalServiceShould
    {
        [Fact]
        public void TriggeredCallBackWithErrorsWhenHaveNotWhateverModels()
        {
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(Enumerable.Empty<WhateverModel>().ToList);
            Mock<Next> callbackMock = new Mock<Next>();

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            callbackMock.Verify(
                callback => callback(
                    It.Is<IReadOnlyList<string>>(
                        messages => messages.SingleOrDefault(message => message == "Aucun whatever trouvé.") != null)),
                Times.Once);
        }

        [Fact]
        public void TriggeredCallBackWithErrorsWhenWhateverModelHasCreatedStep()
        {
            var createdWhatever = new WhateverModel { Step = (int)Step.Created };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { createdWhatever });
            Mock<Next> callbackMock = new Mock<Next>();

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            callbackMock.Verify(
                callback => callback(
                    It.Is<IReadOnlyList<string>>(
                        messages => messages.SingleOrDefault(message => message == "whatever déjà traité.") != null)),
                Times.Once);
        }

        public void ShouldTriggeredCallBackWithErrorsWhenWhateverModelHasInProgressStep()
        {
            var inProgressWhatever = new WhateverModel { Step = (int)Step.InProgress };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { inProgressWhatever });
            Mock<Next> callbackMock = new Mock<Next>();

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            callbackMock.Verify(
                callback => callback(
                    It.Is<IReadOnlyList<string>>(
                        messages => messages.SingleOrDefault(message => message == "whatever déjà traité.") != null)),
                Times.Once);
        }

        public void TriggeredCallBackWithoutErrorsWhenWhateverModelHasInitialStep()
        {
            var initialWhatever = new WhateverModel { Step = (int)Step.Initial };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { initialWhatever });
            Mock<Next> callbackMock = new Mock<Next>();

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            callbackMock.Verify(
                callback => callback(It.Is<IReadOnlyList<string>>(messages => !messages.Any())),
                Times.Once);
        }
    }
}