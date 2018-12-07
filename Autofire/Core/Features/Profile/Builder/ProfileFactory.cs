using System;
using System.Collections.Generic;
using System.Linq;
using Autofire.Core.Features.Profile.Model;
using Action = Autofire.Core.Features.Profile.Model.Action;

namespace Autofire.Core.Features.Profile.Builder
{
    class ProfileFactory
    {
        public IProfile Create(string name, string description, IList<(string, string, string[])> macroDetails)
        {

            return new Model.Profile()
            {
                Name = name,
                Description = description,
                Coverage = CoverageMode.Focus,
                Macros = macroDetails.Select(CreateMacro).ToList()
            };
        }

        private IMacro CreateMacro((string, string , string[]) macroDetail)
        {
            return new Macro()
            {
                Name = macroDetail.Item1,
                Hotkey = macroDetail.Item2,
                ExecutionMode = ExecutionMode.Loop,
                Actions = macroDetail.Item3.Select(CreateAction).ToList()
            };
        }

        private IAction CreateAction(string k)
        {
            return new Action()
            {
                Name = String.Empty,
                Interval = 0.21m,
                Key = k
            };
        }
    }
}
