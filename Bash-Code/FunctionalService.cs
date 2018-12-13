namespace BashCode
{
    using System.Collections.Generic;
    using System.Linq;

    public delegate void Next(IReadOnlyList<string> messages);

    public class FunctionalService
    {
        private const string CodeBase = "ARM";

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

            foreach (var whatever in whatevers)
            {
                var step = whatever.Step;

                if (step >= (int)Step.InProgress)
                {
                    messages.Add("whatever déjà traité.");
                }

                if (step < (int)Step.InProgress)
                {
                    whateversForTreatment.Add(whatever);
                }
            }

            if (!messages.Any() && !whateversForTreatment.Any())
            {
                messages.Add("Aucun whatever trouvé.");
            }

            callback(messages);
        }
    }
}