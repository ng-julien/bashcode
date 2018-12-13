namespace BashCode
{
    using System.Collections.Generic;

    public interface IWhateverAdapter
    {
        void Create(IReadOnlyList<WhateverModel> whateverModels);

        IReadOnlyList<WhateverModel> FindAllByLastModification();
    }
}