namespace BashCode.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using Shouldly;

    using Xunit;

    public class FunctionalServiceShould
    {
        [Fact]
        public void TriggeredCallBackWithErrorsWhenHaveNotWhateverModels()
        {
            var actualMessages = new List<string>();
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(Enumerable.Empty<WhateverModel>().ToList);
            Mock<Next> callbackMock = new Mock<Next>();
            callbackMock.Setup(callback => callback(It.IsAny<IReadOnlyList<string>>()))
                        .Callback<IReadOnlyList<string>>(messages => { actualMessages = messages.ToList(); });

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            var actualMessage = actualMessages.ShouldHaveSingleItem();
            actualMessage.ShouldBe("Aucun whatever trouvé.");
        }

        [Fact]
        public void TriggeredCallBackWithErrorsWhenWhateverModelHasCreatedStep()
        {
            var actualMessages = new List<string>();
            var createdWhatever = new WhateverModel { Step = (int)Step.Created };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { createdWhatever });
            Mock<Next> callbackMock = new Mock<Next>();
            callbackMock.Setup(callback => callback(It.IsAny<IReadOnlyList<string>>()))
                        .Callback<IReadOnlyList<string>>(messages => { actualMessages = messages.ToList(); });

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);
            
            var actualMessage = actualMessages.ShouldHaveSingleItem();
            actualMessage.ShouldBe("whatever déjà traité.");
        }

        [Fact]
        public void TriggeredCallBackWithErrorsWhenWhateverModelHasInProgressStep()
        {
            var actualMessages = new List<string>();
            var inProgressWhatever = new WhateverModel { Step = (int)Step.InProgress };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { inProgressWhatever });
            Mock<Next> callbackMock = new Mock<Next>();
            callbackMock.Setup(callback => callback(It.IsAny<IReadOnlyList<string>>()))
                        .Callback<IReadOnlyList<string>>(messages => { actualMessages = messages.ToList(); });

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            var actualMessage = actualMessages.ShouldHaveSingleItem();
            actualMessage.ShouldBe("whatever déjà traité.");
        }

        [Fact]
        public void TriggeredCallBackWithoutErrorsWhenWhateverModelHasInitialStep()
        {
            List<string> actualMessages = null;
            var initialWhatever = new WhateverModel { Step = (int)Step.Initial };
            Mock<IWhateverAdapter> whateverAdapterMock = new Mock<IWhateverAdapter>();
            whateverAdapterMock.Setup(adapter => adapter.FindAllByLastModification())
                               .Returns(new List<WhateverModel> { initialWhatever });
            Mock<Next> callbackMock = new Mock<Next>();
            callbackMock.Setup(callback => callback(It.IsAny<IReadOnlyList<string>>()))
                        .Callback<IReadOnlyList<string>>(messages => { actualMessages = messages.ToList(); });

            FunctionalService functionalService = new FunctionalService(whateverAdapterMock.Object);
            functionalService.DoSomething(callbackMock.Object);

            actualMessages.ShouldBeEmpty();
        }
    }
}