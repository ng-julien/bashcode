namespace BashCode
{
    using System.Collections.Generic;

    public interface IWhateverAdapter
    {
        IReadOnlyList<WhateverModel> FindAllByLastModification();
    }
}