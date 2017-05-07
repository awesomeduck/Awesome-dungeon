using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeoneerAssets
{
    public interface IInteractable
    {
        string Description { get; }
        string YesButtonText { get; }
        string NoButtonText { get; }
        string YesFinishedText { get; }
        string NoFinishedText { get; }
        bool IsRepeatable { get; }
        void OnYes(InteractionArgs p);

        void OnNo(InteractionArgs p);
    }
}