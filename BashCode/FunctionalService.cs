namespace BashCode
{
    using System.Collections.Generic;
    using System.Linq;

    public class FunctionalService
    {
        private readonly IWhateverAdapter whateverAdapter;

        public FunctionalService(IWhateverAdapter whateverAdapter)
        {
            this.whateverAdapter = whateverAdapter;
        }

        public void DoSomething(Next callback)
        {
            var messages = new List<string>();
            var whateversForTreatment = new List<WhateverModel>();
            var whatevers = this.whateverAdapter.FindAllByLastModification();
            if (!whatevers.Any())
            {
                messages.Add("Aucun whatever trouvé.");
            }

            foreach (var whatever in whatevers)
            {
                var step = whatever.Step;

                if (step >= (int)Step.InProgress)
                {
                    messages.Add("whatever déjà traité.");
                }
                else
                {
                    whateversForTreatment.Add(whatever);
                }
            }

            callback(messages);
        }
    }
}